# EventCalendar.OrchardCore

An [Orchard Core](https://orchardcore.net) module that adds events and a public month calendar.

## Features

- **`EventPart`** — attachable content part with start/end dates (UTC), all-day flag, location, link and HTML description.
- **`Event`** content type — Title + EventPart, creatable, listable, draftable, versionable.
- **`EventWidget`** content type — same fields, usable in zones and flows.
- **Public calendar** — month grid view at `/calendar/{year?}/{month?}` with previous/next navigation and an upcoming-events list.
- **Admin management** — *Content → Events* lists all events ordered by start date.
- **`EventPartIndex`** — YesSql map index for fast date-range queries.
- **`ManageEvents`** permission, granted to Administrator and Editor by default.

## Projects

| Path | Description |
|------|-------------|
| `src/OrchardCore.EventCalendar` | The module. |
| `samples/EventCalendar.Web` | Sample CMS host with AutoSetup, TheTheme and seeded events. |

## Run the sample

```bash
dotnet run --project samples/EventCalendar.Web
```

The site auto-installs (no setup screen) using the `EventCalendarDemo` recipe:

- Admin: `admin` / `Password1!`
- Theme: **TheTheme** (frontend), **TheAdmin** (admin)
- Homepage: the month calendar, seeded with sample events for June 2026.

## Versions

Targets `net10.0` and Orchard Core `3.0.0`.
