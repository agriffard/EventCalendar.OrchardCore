using OrchardCore.ContentManagement;
using OrchardCore.EventCalendar.Models;
using YesSql.Indexes;

namespace OrchardCore.EventCalendar.Indexes;

public class EventPartIndex : MapIndex
{
    public string ContentItemId { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public string DisplayText { get; set; } = string.Empty;

    public bool Published { get; set; }

    public bool Latest { get; set; }

    public DateTime StartUtc { get; set; }

    public DateTime EndUtc { get; set; }

    public bool AllDay { get; set; }

    public string? Location { get; set; }
}

public class EventPartIndexProvider : IndexProvider<ContentItem>
{
    public override void Describe(DescribeContext<ContentItem> context)
    {
        context.For<EventPartIndex>()
            .Map(contentItem =>
            {
                var part = contentItem.As<EventPart>();
                if (part?.StartUtc == null)
                {
                    return null!;
                }

                var start = part.StartUtc.Value;
                var end = part.EndUtc ?? start;
                if (end < start)
                {
                    end = start;
                }

                var displayText = contentItem.DisplayText ?? string.Empty;
                if (displayText.Length > 255)
                {
                    displayText = displayText[..255];
                }

                return new EventPartIndex
                {
                    ContentItemId = contentItem.ContentItemId,
                    ContentType = contentItem.ContentType,
                    DisplayText = displayText,
                    Published = contentItem.Published,
                    Latest = contentItem.Latest,
                    StartUtc = start,
                    EndUtc = end,
                    AllDay = part.AllDay,
                    Location = part.Location,
                };
            });
    }
}
