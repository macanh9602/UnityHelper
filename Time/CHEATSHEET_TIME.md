# Unity C# DateTime Cheatsheet
> Tổng hợp các format và lưu ý khi xử lý thời gian để tránh bug vùng miền và logic.

## 1. Nguyên tắc vàng (Golden Rules)
1. **Lưu trữ & Tính toán logic:** LUÔN dùng `DateTime.UtcNow`.
2. **Hiển thị UI:** Mới chuyển sang giờ địa phương `ToLocalTime()`.
3. **Parse String:** LUÔN dùng `CultureInfo.InvariantCulture`.
4. **Game Logic:** Không dùng `DateTime.Now` trực tiếp, hãy wrap nó lại (VD: `TimeManager.Now`) để dễ chống hack speed/time sau này.

## 2. Format String phổ biến
| Code | Output (Ví dụ) | Mô tả |
| :--- | :--- | :--- |
| `dd/MM/yyyy` | 25/08/2024 | Ngày/Tháng/Năm (Việt Nam) |
| `MM/dd/yyyy` | 08/25/2024 | Tháng/Ngày/Năm (Mỹ) |
| `yyyy-MM-dd` | 2024-08-25 | **ISO Standard** (Dùng để lưu file/JSON) |
| `HH:mm:ss` | 14:30:05 | Giờ 24h |
| `hh:mm tt` | 02:30 PM | Giờ 12h (Sáng/Chiều) |
| `fff` | 500 | Mili giây (Dùng để log bug) |

## 3. Snippets thường dùng

a.Countdown
TimeSpan remaining = targetTime - DateTime.UtcNow;
if (remaining.TotalSeconds <= 0) { /* Hết giờ */ }

// Hiển thị dạng 01:30:05
string uiText = string.Format("{0:D2}:{1:D2}:{2:D2}", 
    remaining.Hours, remaining.Minutes, remaining.Seconds);

b.Daily Reward Reset
bool isNewDay = (currentDate.Date > savedDate.Date);

c.

### So sánh ngày (Bỏ qua giờ)
```csharp
if (date1.Date == date2.Date) { /* Cùng ngày */ }