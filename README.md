PointHunter — Basit Konsol Skor Oyunu

Kısa açıklama

Bu proje, konsol ortamında çalışan basit bir "düşen nesneleri yakala" oyunudur. Oyuncu bir karakteri ("ben") ok tuşlarıyla hareket ettirir ve ekrandan düşen `*` veya `O` sembollerini yakalamaya çalışır. Oyun süresi dolunca veya hedef skora ulaşılınca biter.

Kontroller

- Sol/sağ/yukarı/aşağı ok tuşları veya WASD: karakteri hareket ettirir
- Escape (Esc): oyunu hemen sonlandırır

Oyun davranışı

- Oyuncu ekranda "ben" olarak görünür (3 karakter genişliğinde).
- Ekranın üstünden rastgele `*` veya `O` düşer.
- `*` yakalandığında skor +1, `O` yakalandığında skor +1 (mevcut sürümde her ikisi +1 olacak şekilde ayarlanmıştır).
- Oyun süresi ve hedef skor ulaşıldığında oyun sonlanır.

Loglama (debug)

- Oyunda gerçekleşen önemli olaylar `log.txt` dosyasına yazılır. Örnek tagler:
  - `INPUT_KEY` — tuş basımı
  - `PLAYER_MOVE` — oyuncu konum değişimi
  - `UPDATE` / `ITEM_MOVE` — obje spawn ve hareketi
  - `COLLISION_CHECK` / `COLLISION_HIT` — çarpışma kontrolleri
  - `SCORE_CHANGE` — skor güncellemesi
  - `GAME_END` — oyun bitişi
- `log.txt` uygulamanın çalıştığı dizinde oluşturulur.

İyi oyunlar!
