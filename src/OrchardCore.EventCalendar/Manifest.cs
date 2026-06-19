using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Event Calendar",
    Author = "agriffard",
    Website = "https://github.com/agriffard/EventCalendar.OrchardCore",
    Version = "1.0.0",
    Description = "Create and manage events; display them on a public month calendar.",
    Category = "Content"
)]

[assembly: Feature(
    Id = "OrchardCore.EventCalendar",
    Name = "Event Calendar",
    Description = "Provides the Event content part, content type, a public calendar view and admin management.",
    Category = "Content",
    Dependencies = new[]
    {
        "OrchardCore.Contents",
        "OrchardCore.ContentTypes",
        "OrchardCore.Title"
    }
)]
