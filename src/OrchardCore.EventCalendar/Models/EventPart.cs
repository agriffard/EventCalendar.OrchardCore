using OrchardCore.ContentManagement;

namespace OrchardCore.EventCalendar.Models;

public class EventPart : ContentPart
{
    public DateTime? StartUtc { get; set; }

    public DateTime? EndUtc { get; set; }

    public bool AllDay { get; set; }

    public string? Location { get; set; }

    public string? Url { get; set; }

    public string? Description { get; set; }
}
