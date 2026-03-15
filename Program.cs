using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ConsoleCatchGame
{
    class Program
    {
        static int Width = 40;
        static int Height = 20;

        static int playerX;
        static int playerY;
        static int score = 0;
        static bool isRunning = true;

        static Random rng = new Random();
        static List<FallingItem> items = new List<FallingItem>();

        static int gameDurationSeconds = 30;
        static int targetScore = 10;

        static string logFilePath = "log.txt";

        static void Main(string[] args)
        {
            InitializeConsole();
            InitializeGame();

            Log("SYSTEM", $"event=GameStart | playerX={playerX} | playerY={playerY} | score={score}");

            DateTime startTime = DateTime.Now;

            while (isRunning)
            {
                double elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;

                HandleInput();
                UpdateGame(elapsedSeconds);
                Draw();

                if (elapsedSeconds >= gameDurationSeconds)
                {
                    Log("GAME_END", $"reason=TimeUp | elapsed={elapsedSeconds:F1} | score={score}");
                    isRunning = false;
                }

                if (score >= targetScore)
                {
                    Log("GAME_END", $"reason=TargetScoreReached | score={score}");
                    isRunning = false;
                }

                Thread.Sleep(100);
            }

            Console.SetCursorPosition(0, Height + 1);
            Console.WriteLine($"Oyun bitti. Skor: {score}");
            Console.WriteLine("Loglar 'log.txt' dosyasına yazıldı.");
            Console.ReadKey();
        }

        static void InitializeConsole()
        {
            Console.CursorVisible = false;
            try
            {
                Console.SetWindowSize(Width + 2, Height + 5);
                Console.SetBufferSize(Width + 2, Height + 5);
            }
            catch
            {
            }
        }

        static void InitializeGame()
        {
            playerX = Width / 2;
            playerY = Height - 1;

            try
            {
                File.WriteAllText(logFilePath, "");
            }
            catch
            {
            }
        }

        static void HandleInput()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int oldX = playerX;
                int oldY = playerY;

                Log("INPUT_KEY", $"key={keyInfo.Key} | playerX={playerX} | playerY={playerY}");

                if (keyInfo.Key == ConsoleKey.LeftArrow && playerX > 0)
                    playerX--;
                else if (keyInfo.Key == ConsoleKey.RightArrow && playerX < Width - 3)
                    playerX++;
                else if (keyInfo.Key == ConsoleKey.UpArrow && playerY > 0)
                    playerY--;
                else if (keyInfo.Key == ConsoleKey.DownArrow && playerY < Height - 1)
                    playerY++;
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isRunning = false;
                    Log("GAME_END", $"reason=UserExit | key=Escape | score={score}");
                }

                if (oldX != playerX || oldY != playerY)
                {
                    Log("PLAYER_MOVE", $"key={keyInfo.Key} | oldX={oldX} | oldY={oldY} | newX={playerX} | newY={playerY}");
                }
            }
        }

        static void UpdateGame(double elapsedSeconds)
        {
            if (rng.NextDouble() < 0.15)
            {
                int x = rng.Next(0, Width);
                char symbol = rng.Next(0, 2) == 0 ? '*' : 'O';
                items.Add(new FallingItem { X = x, Y = 0, Symbol = symbol });

                Log("UPDATE", $"event=Spawn | symbol={symbol} | x={x} | y=0 | elapsed={elapsedSeconds:F1}");
            }

            for (int i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                int oldY = item.Y;

                item.Y++;

                Log("ITEM_MOVE", $"symbol={item.Symbol} | x={item.X} | oldY={oldY} | newY={item.Y}");

                bool isCollision = (item.Y == playerY && item.X >= playerX && item.X <= playerX + 2);
                Log("COLLISION_CHECK", $"symbol={item.Symbol} | itemX={item.X} | itemY={item.Y} | playerX={playerX} | playerY={playerY} | hit={isCollision}");

                if (isCollision)
                {
                    int oldScore = score;
                    score++;

                    Log("COLLISION_HIT", $"symbol={item.Symbol} | x={item.X} | y={item.Y}");
                    Log("SCORE_CHANGE", $"oldScore={oldScore} | newScore={score}");

                    items.RemoveAt(i);
                    continue;
                }

                if (item.Y >= Height)
                {
                    Log("UPDATE", $"event=OutOfBounds | symbol={item.Symbol} | x={item.X} | lastY={oldY}");
                    items.RemoveAt(i);
                }
                else
                {
                    items[i] = item;
                }
            }

            Log("STATE", $"elapsed={elapsedSeconds:F1} | score={score} | items={items.Count}");
        }

        static void Draw()
        {
            Console.SetCursorPosition(0, 0);

            Console.WriteLine($"Skor: {score}  Süre: {gameDurationSeconds} sn  Hedef Skor: {targetScore}");
            Console.WriteLine(new string('-', Width));

            char[,] buffer = new char[Height, Width];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    buffer[y, x] = ' ';

            foreach (var item in items)
            {
                if (item.Y >= 0 && item.Y < Height && item.X >= 0 && item.X < Width)
                    buffer[item.Y, item.X] = item.Symbol;
            }

            for (int y = 0; y < Height; y++)
            {
                int x = 0;
                while (x < Width)
                {
                    if (y == playerY && x == playerX && x + 2 < Width)
                    {
                        Console.Write("ben");
                        x += 3;
                    }
                    else
                    {
                        Console.Write(buffer[y, x]);
                        x++;
                    }
                }
                Console.WriteLine();
            }
        }

        static void Log(string tag, string message)
        {
            string paddedTag = tag.PadRight(12);
            string line = $"{DateTime.Now:HH:mm:ss.fff} [{paddedTag}] {message}";
            try
            {
                File.AppendAllText(logFilePath, line + Environment.NewLine);
            }
            catch
            {
            }
        }
    }

    struct FallingItem
    {
        public int X;
        public int Y;
        public char Symbol;
    }
}