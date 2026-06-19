using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace OrchardCore.EventCalendar;

public sealed class AdminMenu : INavigationProvider
{
    internal readonly IStringLocalizer S;

    public AdminMenu(IStringLocalizer<AdminMenu> stringLocalizer)
    {
        S = stringLocalizer;
    }

    public ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
        {
            return ValueTask.CompletedTask;
        }

        builder
            .Add(S["Content"], content => content
                .Add(S["Events"], S["Events"].PrefixPosition(), events => events
                    .Action("Index", "Admin", new { area = "OrchardCore.EventCalendar" })
                    .Permission(EventCalendarPermissions.ManageEvents)
                    .LocalNav()));

        return ValueTask.CompletedTask;
    }
}
