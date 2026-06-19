using OrchardCore.ContentManagement;

namespace OrchardCore.EventCalendar.ViewModels;

public class EventAdminListViewModel
{
    public List<EventAdminEntry> Events { get; set; } = [];
}

public class EventAdminEntry
{
    public ContentItem ContentItem { get; set; } = default!;

    public string ContentItemId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public DateTime StartUtc { get; set; }

    public DateTime EndUtc { get; set; }

    public bool AllDay { get; set; }

    public string? Location { get; set; }

    public bool Published { get; set; }
}
