# Repository Guidelines

## Project Structure & Modules
- Code lives under `Assets/_DarkSeas/Scripts/` with submodules: `Core/`, `Gameplay/`, `Meta/`, `UI/`, `WorldGen/`.
- Tests are in `Assets/_DarkSeas/Tests/` split into `EditMode/` and `PlayMode/`.
- Content: `Assets/_DarkSeas/{Art,Audio,Data,Prefabs,Scenes}`. Main scenes include `HarborScene.unity`, `ExpeditionScene.unity`, `DebriefScene.unity`.
- Assembly defs: `DarkSeas.Core.asmdef`, `DarkSeas.Gameplay.asmdef` keep compile units fast.

## Build, Test, Dev Commands
- Open project: use Unity Hub (Unity 6000.2.0f1+). From CLI (Windows example):
  - `"C:/Program Files/Unity/Hub/Editor/6000.2.0f1/Editor/Unity.exe" -projectPath .` 
- Run EditMode tests (Windows example):
  - `Unity.exe -batchmode -projectPath . -runTests -testPlatform EditMode -logfile Logs/editmode.log`
- Run PlayMode tests:
  - `Unity.exe -batchmode -projectPath . -runTests -testPlatform PlayMode -logfile Logs/playmode.log`
- Build via Editor: File → Build Settings → Windows Standalone (or `Ctrl+B`).
  Note: Unity is not installed in WSL here—run builds/tests from the Windows Editor or Test Runner (Window → General → Test Runner).

## Coding Style & Naming
- C# 9, 4-space indentation; one class per file.
- Naming: PascalCase for types/methods; `_camelCase` for private fields; UPPER_SNAKE_CASE for constants.
- Unity: prefer `[SerializeField] private` over public fields; small, focused MonoBehaviours; event-driven via `*Signals` in `Core/`.
- Keep scripts inside the relevant module folder; match filename to type name.

## Testing Guidelines
- Framework: Unity Test Framework (EditMode/PlayMode split).
- Place tests under matching folders mirroring source (e.g., `Tests/EditMode/Gameplay/Boat/BoatFuelTests.cs`).
- Aim for coverage on core systems (boat movement, fuel, hazards, rescue, world gen). Fast logic → EditMode; integration/scene → PlayMode.

## Commit & Pull Requests
- Commits: imperative mood, concise subject (≤72 chars). Example: `Add BoatFuel consumption and tests`.
- Before PR: ensure compilation clean, run both test suites, sanity-play `Harbor`, `Expedition`, `Debrief` scenes.
- PRs should include: clear description, linked issues, test results, and gameplay GIF/screenshot for UX-visible changes.
 - Branching: target `main`. Keep PRs focused and small.

## Configuration Tips
- Use Git LFS for large assets; always commit `.meta` files.
- Never commit `Library/`, `Temp/`, or `obj/` (already ignored).
- Project targets URP on Windows; profile to maintain 60 FPS in Expedition scene.
- See `CLAUDE.md` and `.github/copilot-instructions.md` for deeper architecture and agent guidance.
