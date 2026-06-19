using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.EventCalendar.Indexes;
using YesSql.Sql;

namespace OrchardCore.EventCalendar;

public sealed class Migrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public Migrations(IContentDefinitionManager contentDefinitionManager)
    {
        _contentDefinitionManager = contentDefinitionManager;
    }

    public async Task<int> CreateAsync()
    {
        await _contentDefinitionManager.AlterPartDefinitionAsync("EventPart", part => part
            .Attachable()
            .WithDescription("Adds event details (dates, location, description) to a content type."));

        await _contentDefinitionManager.AlterTypeDefinitionAsync("Event", type => type
            .DisplayedAs("Event")
            .Creatable()
            .Listable()
            .Draftable()
            .Versionable()
            .Securable()
            .WithPart("TitlePart", part => part.WithPosition("0"))
            .WithPart("EventPart", part => part.WithPosition("1")));

        await _contentDefinitionManager.AlterTypeDefinitionAsync("EventWidget", type => type
            .DisplayedAs("Event Widget")
            .Stereotype("Widget")
            .WithPart("TitlePart", part => part.WithPosition("0"))
            .WithPart("EventPart", part => part.WithPosition("1")));

        await SchemaBuilder.CreateMapIndexTableAsync<EventPartIndex>(table => table
            .Column<string>("ContentItemId", column => column.WithLength(26))
            .Column<string>("ContentType", column => column.WithLength(255))
            .Column<string>("DisplayText", column => column.WithLength(255))
            .Column<bool>("Published")
            .Column<bool>("Latest")
            .Column<DateTime>("StartUtc")
            .Column<DateTime>("EndUtc")
            .Column<bool>("AllDay")
            .Column<string>("Location", column => column.WithLength(255)));

        await SchemaBuilder.AlterIndexTableAsync<EventPartIndex>(table => table
            .CreateIndex("IDX_EventPartIndex_Range", "Published", "StartUtc", "EndUtc"));

        await SchemaBuilder.AlterIndexTableAsync<EventPartIndex>(table => table
            .CreateIndex("IDX_EventPartIndex_Item", "ContentItemId"));

        return 1;
    }
}
