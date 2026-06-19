using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.EventCalendar.Drivers;
using OrchardCore.EventCalendar.Indexes;
using OrchardCore.EventCalendar.Models;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using YesSql.Indexes;

namespace OrchardCore.EventCalendar;

public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddContentPart<EventPart>()
            .UseDisplayDriver<EventPartDisplayDriver>();

        services.AddScoped<IPermissionProvider, EventCalendarPermissions>();
        services.AddScoped<INavigationProvider, AdminMenu>();
        services.AddScoped<IDataMigration, Migrations>();
        services.AddSingleton<IIndexProvider, EventPartIndexProvider>();
    }

    public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        routes.MapAreaControllerRoute(
            name: "EventCalendar",
            areaName: "OrchardCore.EventCalendar",
            pattern: "calendar/{year?}/{month?}",
            defaults: new { controller = "Calendar", action = "Index" });
    }
}
