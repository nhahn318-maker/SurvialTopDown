# Survival Top-Down

Prototype game survival góc nhìn top-down, thực hiện cho bài test Unity Game Developer của Wolffun.

## Thông tin project

- **Unity:** `2022.3.62f2 LTS`
- **Render Pipeline:** Universal Render Pipeline (URP)
- **Scene chính:** `Assets/Scenes/Main.unity`
- **Nền tảng đã build/test:** Android

## Cách mở và chạy

1. Mở project bằng Unity `2022.3.62f2`.
2. Mở scene `Assets/Scenes/Main.unity`.
3. Nhấn **Play** để chạy trong Editor.

## Điều khiển

| Nút / thao tác | Chức năng |
|---|---|
| Fixed Joystick | Di chuyển và xoay Player theo hướng di chuyển |
| SHOOT | Bắn đánh thường khi còn charge |
| BOMB | Đặt bom tại vị trí hiện tại của Player |
| DASH | Lướt theo hướng forward và gây nổ khi kết thúc |

Trong Unity Editor, kéo joystick và bấm các nút kỹ năng bằng chuột.

## Tiến độ theo yêu cầu đề bài

### 1. Mục tiêu và phạm vi — Đã hoàn thành

- Game top-down 3D trên mặt phẳng ngang, camera follow Player.
- Điều khiển bằng joystick ảo và các nút kỹ năng trên UI.
- Scene chính chạy trực tiếp trong Unity Editor.

### 2. Nhân vật (Player) — Đã hoàn thành

- HP khởi đầu `500`, tốc độ di chuyển `2 unit/giây`, tốc độ xoay `180 độ/giây`.
- Giáp khởi đầu `0`, Damage Multiplier khởi đầu `0`.
- Công thức sát thương nhận: `sát thương gốc - giáp`, tối thiểu bằng `0`.
- Công thức sát thương gây ra: `sát thương gốc x (1 + Damage Multiplier)`.
- Dùng `CharacterController`, joystick và hướng forward hiện tại cho các kỹ năng.

### 3. Kỹ năng nhân vật — Đã hoàn thành

- **Đánh thường:** bắn 3 viên theo các góc `-15`, `0`, `+15` độ; sát thương gốc `10`; tối đa 3 charge; hồi 1 charge mỗi 3 giây; giới hạn bắn 0.5 giây.
- **Bom:** đặt tại vị trí Player, nổ sau 2 giây, sát thương gốc `50`, bán kính `5`, cooldown `12` giây.
- **Dash:** lướt 3 unit trong 0.5 giây, nổ sát thương gốc `15` trong bán kính `3`, cooldown `6` giây.
- Có hiển thị cooldown và phản hồi animation/VFX/SFX cho kỹ năng phù hợp.

### 4. Kẻ địch — Đã hoàn thành

- **Melee Enemy:** HP `220`, speed `3`, đánh hình nón 50 độ trong tầm `1.3`, sát thương gốc `30`; tấn công xong đứng im 1 giây.
- **Ranged Enemy:** HP `180`, speed `2.7`, tiếp cận tới khoảng cách `3`, bắn đạn độc speed `10` trong tầm `5`; bắn xong đứng im 1 giây.
- Độc tick ngay khi trúng và mỗi giây trong 3 giây, tổng 4 tick; dính lại chỉ refresh thời gian, không stack.
- Có thanh máu world-space, animation hit/death và pool cho enemy.

### 5. Wave, kinh nghiệm và lên cấp — Đã hoàn thành

- Mỗi wave random `3-4` melee và `1-2` ranged enemy.
- Wave sau chỉ bắt đầu khi đã clear toàn bộ quái đang active.
- Spawn từng quái theo thời gian cấu hình trong `WaveDefaultConfig` để dễ tuning độ khó.
- Mỗi quái cho `30 EXP`; đủ `100 EXP` lên một cấp và giữ EXP dư.
- Khi lên cấp: +40 máu hiện tại, +40 máu tối đa, +2 giáp, +0.1 Damage Multiplier.
- Có feedback level-up và popup thông báo wave mới.

### 6. UI bắt buộc — Đã hoàn thành

- Thanh máu và level của Player.
- Joystick ảo, nút đánh thường/bom/dash và hiển thị cooldown/charge.
- Thanh máu world-space phía trên từng enemy.
- Màn hình Game Over và nút khởi động lại.


### 7. Bonus — Đã làm một phần

- [x] Camera shake khi bắn, dash và Player nhận sát thương.
- [x] VFX cho nổ bom, dash, projectile impact, độc và level-up.
- [x] SFX cho bắn, bom nổ, dash, Player bị đánh và bước chân.


