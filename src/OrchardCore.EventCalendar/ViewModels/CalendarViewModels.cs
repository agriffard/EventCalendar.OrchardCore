namespace OrchardCore.EventCalendar.ViewModels;

public class CalendarViewModel
{
    public int Year { get; set; }

    public int Month { get; set; }

    public string MonthName { get; set; } = string.Empty;

    public DateTime PrevMonth { get; set; }

    public DateTime NextMonth { get; set; }

    public IReadOnlyList<string> WeekDayNames { get; set; } = [];

    public List<List<CalendarDay>> Weeks { get; set; } = [];

    public List<CalendarEvent> UpcomingEvents { get; set; } = [];
}

public class CalendarDay
{
    public DateTime Date { get; set; }

    public bool InMonth { get; set; }

    public bool IsToday { get; set; }

    public List<CalendarEvent> Events { get; set; } = [];
}

public class CalendarEvent
{
    public string ContentItemId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public DateTime StartUtc { get; set; }

    public DateTime EndUtc { get; set; }

    public bool AllDay { get; set; }

    public string? Location { get; set; }
}
