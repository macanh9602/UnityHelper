using System;

public static class TimeManager
{
    // Cờ bật tắt chế độ Dev (dùng giờ máy) hay Product (dùng giờ server)
    public static bool UseServerTime = false;

    /// <summary>
    /// Lấy thời gian hiện tại chuẩn nhất của game.
    /// Nên dùng hàm này thay cho DateTime.Now hoặc DateTime.UtcNow trực tiếp.
    /// </summary>
    public static DateTime Now
    {
        get
        {
            if (UseServerTime)
            {
                // TODO: Return cached server time + real time since startup
                return DateTime.UtcNow; // Placeholder
            }
            return DateTime.UtcNow; // Luôn dùng UTC cho logic game!
        }
    }

    /// <summary>
    /// Lấy ngày hiện tại (đã reset về 00:00:00)
    /// </summary>
    public static DateTime Today => Now.Date;
}