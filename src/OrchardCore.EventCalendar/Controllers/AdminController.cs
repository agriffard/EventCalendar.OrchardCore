using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.EventCalendar.Indexes;
using OrchardCore.EventCalendar.ViewModels;
using YesSql;

namespace OrchardCore.EventCalendar.Controllers;

public class AdminController : Controller
{
    private readonly ISession _session;
    private readonly IAuthorizationService _authorizationService;

    public AdminController(
        ISession session,
        IAuthorizationService authorizationService)
    {
        _session = session;
        _authorizationService = authorizationService;
    }

    public async Task<IActionResult> Index()
    {
        if (!await _authorizationService.AuthorizeAsync(User, EventCalendarPermissions.ManageEvents))
        {
            return Forbid();
        }

        var entries = (await _session
            .QueryIndex<EventPartIndex>(x => x.Latest)
            .OrderByDescending(x => x.StartUtc)
            .ListAsync())
            .Select(x => new EventAdminEntry
            {
                ContentItemId = x.ContentItemId,
                Title = x.DisplayText,
                StartUtc = x.StartUtc,
                EndUtc = x.EndUtc,
                AllDay = x.AllDay,
                Location = x.Location,
                Published = x.Published,
            })
            .ToList();

        var model = new EventAdminListViewModel
        {
            Events = entries,
        };

        return View(model);
    }
}
