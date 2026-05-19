# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Quản lý Khách Sạn** — A Windows Forms desktop application for hotel room management, built with C# and .NET Framework 4.7.2. The UI uses the Guna.UI2 library for modern-styled WinForms components.

## Build & Run

Open and build via Visual Studio 2017+:
- Solution file: `Quan_ly_KS.sln`
- Build output: `Quan_ly_KS/bin/Debug/` or `bin/Release/`
- Entry point: `Program.cs` → `Application.Run(new Form1())`

MSBuild from command line:
```
msbuild Quan_ly_KS.sln /p:Configuration=Debug
```

NuGet packages are stored in `packages/` and referenced locally (no restore needed if directory exists).

## Database

- **Engine:** SQL Server LocalDB (SQL Server Express)
- **File:** `dbMyHotel.mdf` — path is hardcoded in `Quan_ly_KS/function.cs` (line ~17)
- The MDF file must exist at the configured path before the app can run

## Architecture

### Layers

| Layer | Files | Role |
|---|---|---|
| Presentation | `Form1.cs`, `Dashboard.cs`, `All User Control/UC_*.cs` | UI forms and user controls |
| Data Access | `function.cs` | All SQL Server interactions |

### `function.cs` — Central DAL

Three static methods handle all database I/O:
- `GetData(sql)` — SELECT → returns `DataSet`
- `SetData(sql)` — INSERT / UPDATE / DELETE
- `GetForCombo(sql)` — fills `ComboBox` controls

SQL queries are constructed and passed as strings directly from the UserControl classes.

### Navigation Pattern

`Dashboard.cs` hosts all UserControls (`UC_AddRoom`, `UC_CustomerRes`, `UC_CheckOut`, `UC_CustomerDetails`, `UC_Emloyee`). Navigation buttons toggle `.Visible` on the relevant control and update a `PanelMoving` indicator. Only one panel is shown at a time.

### Login

`Form1.cs` handles authentication before loading `Dashboard`. Credentials are validated against the database (check `Form1.cs` for the current approach).

## Key Source Files

- [Quan_ly_KS/function.cs](Quan_ly_KS/function.cs) — database connection and all query helpers
- [Quan_ly_KS/Dashboard.cs](Quan_ly_KS/Dashboard.cs) — main shell, navigation logic
- [Quan_ly_KS/Form1.cs](Quan_ly_KS/Form1.cs) — login form
- [Quan_ly_KS/All User Control/](Quan_ly_KS/All%20User%20Control/) — one `UC_*.cs` per feature (rooms, reservations, check-out, customers, employees)

## UI Library

**Guna.UI2.WinForms v2.0.3.5** — provides styled buttons, panels, and form decorations. Controls prefixed `guna` in Designer files come from this library. The DLL is in `packages/Guna.UI2.WinForms.2.0.3.5/`.
