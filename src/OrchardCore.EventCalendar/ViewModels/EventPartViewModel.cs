using OrchardCore.ContentManagement;
using OrchardCore.EventCalendar.Models;

namespace OrchardCore.EventCalendar.ViewModels;

public class EventPartViewModel
{
    public EventPart Part { get; set; } = default!;

    public ContentItem ContentItem { get; set; } = default!;
}
