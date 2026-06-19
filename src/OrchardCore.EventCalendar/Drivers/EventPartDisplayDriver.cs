using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.EventCalendar.Models;
using OrchardCore.EventCalendar.ViewModels;

namespace OrchardCore.EventCalendar.Drivers;

public sealed class EventPartDisplayDriver : ContentPartDisplayDriver<EventPart>
{
    public override IDisplayResult Display(EventPart part, BuildPartDisplayContext context)
    {
        return Initialize<EventPartViewModel>(GetDisplayShapeType(context), model =>
            {
                model.Part = part;
                model.ContentItem = part.ContentItem;
            })
            .Location("Detail", "Content:5")
            .Location("Summary", "Content:5");
    }

    public override IDisplayResult Edit(EventPart part, BuildPartEditorContext context)
    {
        return Initialize<EventPartEditViewModel>(GetEditorShapeType(context), model =>
        {
            model.StartUtc = part.StartUtc;
            model.EndUtc = part.EndUtc;
            model.AllDay = part.AllDay;
            model.Location = part.Location;
            model.Url = part.Url;
            model.Description = part.Description;
        });
    }

    public override async Task<IDisplayResult> UpdateAsync(EventPart part, UpdatePartEditorContext context)
    {
        var model = new EventPartEditViewModel();
        await context.Updater.TryUpdateModelAsync(model, Prefix);

        part.StartUtc = model.StartUtc;
        part.EndUtc = model.EndUtc;
        part.AllDay = model.AllDay;
        part.Location = model.Location?.Trim();
        part.Url = model.Url?.Trim();
        part.Description = model.Description;

        if (part.StartUtc == null)
        {
            context.Updater.ModelState.AddModelError(nameof(model.StartUtc), "The start date is required.");
        }

        if (part.StartUtc.HasValue && part.EndUtc.HasValue && part.EndUtc < part.StartUtc)
        {
            context.Updater.ModelState.AddModelError(nameof(model.EndUtc), "The end date must be on or after the start date.");
        }

        return Edit(part, context);
    }
}
