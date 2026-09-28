# AGENTS.md

Guidance for AI agents (and humans) working in the YWML repository. Read this before making changes.

## 1. How I want you to work (read this first)

- **Split every plan into small, individual steps.** Do not dump one giant plan or batch a large refactor into a single phase.
- **Consult me before each step.** For every step, present the plan, explain how you intend to implement it, and wait for my approval before going ahead.
- **One step at a time.** Finish (and verify) the current step before proposing the next one.
- **Ask, don't assume.** If a requirement, scope, or approach is ambiguous, ask me instead of guessing.
- **Be concise.** Short, direct answers. Explain non-trivial commands before running them.
- **Never commit** unless I explicitly ask you to.

## 2. Project overview

YWML (Yo-kai Watch Mod Loader) is a mod loader for the Yo-kai Watch series on 3DS. It layers loose mod files (`include`, `mov`, `snd`, etc.) into the game's `romfs`, and supports installable "extensions" that add support for specific games.

### Solution layout

| Project | TFM | Purpose |
| --- | --- | --- |
| `YWML.Core` | `net10.0` | UI-free, cross-platform logic. No WinForms, no MAUI, no `MessageBox`, no `Application.Exit`. |
| `YWML.Desktop` | `net10.0-windows7.0` | WinForms UI (Windows 7+). References `YWML.Core`. |
| `YWML.Android` | `net10.0-android` | .NET MAUI UI (Android, `minSdk` 23). References `YWML.Core`. |
| `Tests/YWML.Tests.csproj` | `net10.0-windows7.0` | xUnit tests; references `YWML.Core` + `YWML.Desktop`. |

- `Tests/` is **gitignored** (line 367 of `.gitignore`) — keep tests local.
- `FAMerger` and `MigrateModForm` are **Desktop-only** and deliberately excluded from Android.
- All shared logic must live in `YWML.Core`; platform projects only contain UI/bindings/platform seams.

### Key seams

- `CGeneralUtils.Initialize(dataDir)` sets the data root; paths are derived properties (no `%APPDATA%` hardcoding).
- `CConfigManager` throws on failure instead of showing UI.
- Installation is abstracted via `IInstallDestination` (`FileSystemInstallDestination` on desktop, `SafInstallDestination` on Android).
- HTTP is testable via the `HttpMessageHandler` seam on `CExtensionDownloader` / `CExtension`.
- Extension payloads are **LZMA-Alone**: `[5-byte props][8-byte little-endian uncompressed size][data]`, decompressed with SharpCompress.

## 3. Build & test

From the repo root (`C:\Users\yehon\source\repos\YWML`):

```pwsh
dotnet build YWML.sln -c Release
dotnet test Tests/YWML.Tests.csproj -c Release
```

- **Always prefer `-c Release` for tests.** A running `YWML.Desktop` locks the Debug `YWML.Core.dll` and will fail the build.
- **Do not rebuild the Android Release APK after every change** — only package it when I explicitly ask.
- Android build/run from the CLI is unreliable while Visual Studio and/or an emulator hold file locks — build/run Android from Visual Studio.
- Do not `Select-Object -First/-Last` to trim command output; full output is captured to a file if large.
- Run the lint/format conventions of the repo before declaring work done, and verify with tests.

## 4. Coding conventions

- **Naming:** match the existing style exactly.
  - Classes/interfaces/enums/methods/properties: `PascalCase` (classes are often prefixed `C` or `S`, e.g. `CLoader`, `SInstallMode`).
  - Private fields: `_camelCase`.
  - Local variables/parameters: `camelCase`.
- **No backwards-compatibility shims.** When something is replaced, replace it cleanly and update all call sites; do not keep dead/legacy paths around.
- **Do not add new NuGet packages without asking me first.** Current Core packages: `FluentFTP`, `SharpCompress`, `Newtonsoft.Json`, `Samboy063.Tomlet`, `System.Text.Encoding.CodePages`.
- **No comments unless I ask for them.** Keep code self-explanatory.
- Prefer the existing helpers/utilities over introducing new patterns.
- Nullable and implicit usings are enabled in all projects.
- Keep `YWML.Core` free of UI and platform APIs.

## 5. Design language

### General

- Clean, rounded feel; branding uses a monospace font. No bright accent color — Desktop keeps its light system look, Android is dark and neutral.
- Buttons use default system visual styling (`UseVisualStyleBackColor = true`).
- Keep layouts consistent with neighboring forms/pages; do not invent new fonts or colors per screen.

### Desktop (WinForms)

- **Brand / titles / headers:** `Consolas` (monospace, regular). E.g. the big `YWML` label (`48F`), form titles (`24F`), version label (`11.25F`, italic).
- **Body / labels / buttons / inputs / tree & combo:** `Yu Gothic UI Semibold`, `9.75F`, `Bold`.
- **Secondary/helper italic hints:** `Consolas`, `8F`, `Italic`.
- **Text boxes:** `Arial Rounded MT Bold`, `9.75F`, `Regular`.
- **Colors:** light surface via `BackColor = SystemColors.ButtonHighlight`; no custom `BackColor`/`ForeColor` on controls except brand labels (`SystemColors.ActiveCaptionText`). Use `UseVisualStyleBackColor = true` on buttons.
- Keep this pattern when adding new controls/forms.

### Android (MAUI)

- `UserAppTheme = AppTheme.Dark` always (dark theme only).
- **Neutral/card aesthetic, no accent color** (mirrors Desktop's plain system look). Surfaces: page `#1f1f1f` (`OffBlack`), card/row `#1C1C1E` (`Secondary`), button `#2C2C2E` (`ButtonSurface`), subtle stroke `#404040` (`Gray600`). See `Resources/Styles/Colors.xaml`.
- **Buttons** (`Resources/Styles/Styles.xaml`): `BaseButton` = dark card surface + `#404040` stroke + white text; `SmallButton` = compact variant (extension install/uninstall); `IconButton` = transparent small square for `✕`/`▲`/`▼`.
- **Page title label:** ALL-CAPS, `FontFamily = "monospace"`, `FontSize = 28`, centered (e.g. `LOAD`, `EXTENSIONS`, `SETTINGS`).
- **Labels:** bold via `FontAttributes.Bold`.
- **Helper/hint/status text:** `FontSize = 12`, `TextColor = Colors.Gray`.
- **Link text** (e.g. the "Can't find your game?" action): `#00BFFF` (`Link`).
- Shell bottom tabs: Load / Extensions / Settings. Install modes are labeled **"Remote 3DS install"** and **"Android Emulator install"**.
- Onboarding: `OnboardingPage` is a 3-step `CarouselView` (placeholder images `Resources/Images/onboarding_*.svg`); shown once via the `onboarding_done` preference, re-runnable from Settings.

### Version & constants

- `CGeneralUtils.APP_VERSION` (currently `1.2.0`) is the single source of truth for the displayed version.
- Default FTP port is `5000`.
- Extension library source: `https://pastebin.com/raw/3CfZWnxb`.

## 6. Environment gotchas

- A running `YWML.Desktop` locks the Debug output (`YWML.Core.dll`) → use `-c Release` for builds/tests, or close the app.
- Visual Studio locks Android fast-deploy assets; `dotnet clean YWML.sln` only half-cleans Android → manually delete `YWML.Android\obj` and `YWML.Android\bin` if needed.
- When packaging the Android Release APK for distribution, **delete `YWML.Android\obj`+`bin` and rebuild clean**. A .NET 10 resource-ID bug (dotnet/android#10563) can otherwise ship an APK whose theme doesn't resolve, crashing on launch with `TextAppearance`/`Theme.MaterialComponents`.
- Tests run with parallelization disabled and register `CodePagesEncodingProvider` via a module initializer.
