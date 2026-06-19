using OrchardCore.Security.Permissions;

namespace OrchardCore.EventCalendar;

public sealed class EventCalendarPermissions : IPermissionProvider
{
    public static readonly Permission ManageEvents = new("ManageEvents", "Manage events (create, edit, delete)");

    private readonly IEnumerable<Permission> _allPermissions =
    [
        ManageEvents,
    ];

    public Task<IEnumerable<Permission>> GetPermissionsAsync()
        => Task.FromResult(_allPermissions);

    public IEnumerable<PermissionStereotype> GetDefaultStereotypes() =>
    [
        new PermissionStereotype
        {
            Name = "Administrator",
            Permissions = [ManageEvents],
        },
        new PermissionStereotype
        {
            Name = "Editor",
            Permissions = [ManageEvents],
        },
    ];
}
