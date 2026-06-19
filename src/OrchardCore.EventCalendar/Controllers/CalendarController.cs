using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.EventCalendar.Indexes;
using OrchardCore.EventCalendar.ViewModels;
using OrchardCore.Modules;
using YesSql;

namespace OrchardCore.EventCalendar.Controllers;

public class CalendarController : Controller
{
    private readonly ISession _session;
    private readonly IClock _clock;

    public CalendarController(ISession session, IClock clock)
    {
        _session = session;
        _clock = clock;
    }

    public async Task<IActionResult> Index(int? year, int? month)
    {
        var now = _clock.UtcNow;
        var y = year is >= 1 and <= 9999 ? year.Value : now.Year;
        var m = month is >= 1 and <= 12 ? month.Value : now.Month;

        var monthStart = new DateTime(y, m, 1, 0, 0, 0, DateTimeKind.Utc);
        var monthEnd = monthStart.AddMonths(1);

        // Visible grid starts on the Monday on or before the 1st and ends after the Sunday on or after the last day.
        var gridStart = monthStart.AddDays(-(((int)monthStart.DayOfWeek + 6) % 7));
        var lastDay = monthEnd.AddDays(-1);
        var lastDayIndex = ((int)lastDay.DayOfWeek + 6) % 7; // 0 = Monday .. 6 = Sunday
        var gridEnd = lastDay.AddDays(6 - lastDayIndex).AddDays(1);

        var calendarEvents = (await _session
            .QueryIndex<EventPartIndex>(x =>
                x.Published &&
                x.EndUtc >= gridStart &&
                x.StartUtc < gridEnd)
            .ListAsync())
            .Select(x => new CalendarEvent
            {
                ContentItemId = x.ContentItemId,
                Title = x.DisplayText,
                StartUtc = x.StartUtc,
                EndUtc = x.EndUtc,
                AllDay = x.AllDay,
                Location = x.Location,
            })
            .ToList();

        var today = now.Date;
        var weeks = new List<List<CalendarDay>>();
        for (var cursor = gridStart; cursor < gridEnd; cursor = cursor.AddDays(7))
        {
            var week = new List<CalendarDay>();
            for (var d = 0; d < 7; d++)
            {
                var date = cursor.AddDays(d);
                var dayEvents = calendarEvents
                    .Where(e => e.StartUtc.Date <= date && e.EndUtc.Date >= date)
                    .OrderBy(e => e.AllDay ? 0 : 1)
                    .ThenBy(e => e.StartUtc)
                    .ToList();

                week.Add(new CalendarDay
                {
                    Date = date,
                    InMonth = date.Month == m && date.Year == y,
                    IsToday = date == today,
                    Events = dayEvents,
                });
            }

            weeks.Add(week);
        }

        var culture = CultureInfo.CurrentCulture;
        var dayNames = new List<string>();
        for (var d = 0; d < 7; d++)
        {
            var day = (DayOfWeek)(((int)DayOfWeek.Monday + d) % 7);
            dayNames.Add(culture.DateTimeFormat.GetAbbreviatedDayName(day));
        }

        var upcoming = calendarEvents
            .Where(e => e.EndUtc >= now)
            .OrderBy(e => e.StartUtc)
            .Take(10)
            .ToList();

        var model = new CalendarViewModel
        {
            Year = y,
            Month = m,
            MonthName = culture.DateTimeFormat.GetMonthName(m),
            PrevMonth = monthStart.AddMonths(-1),
            NextMonth = monthStart.AddMonths(1),
            WeekDayNames = dayNames,
            Weeks = weeks,
            UpcomingEvents = upcoming,
        };

        return View(model);
    }
}
