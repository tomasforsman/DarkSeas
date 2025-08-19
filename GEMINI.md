# GEMINI.md — Dark Seas (Unity)

This document provides consolidated guidance for Gemini for developing the **Dark Seas** Unity project. It is synthesized from `CLAUDE.md`, `AGENTS.md`, `README.md`, and `.github/copilot-instructions.md`.

---

## 1. High-Level Goals & Project Overview

Dark Seas is a Unity-based survival game with a **harbor → night expedition → debrief** gameplay loop. Players captain a boat through dangerous, procedurally generated Arctic waters to rescue survivors, while managing fuel, avoiding ice hazards, and repairing hull damage.

- **Core Experience:** 5-10 minute expeditions, atmospheric tension, and meaningful progression.
- **MVP Features:** Boat control, collision damage, headlight/visibility, drifting ice, survivor pickup & delivery, fuel pressure, and a simple legacy upgrade system.
- **Target:** Stable 60 FPS @ 1080p on Windows PC.

---

## 2. Unity & Project Configuration

- **Unity Version:** **6000.2.0f1 (Unity 6 LTS)**
- **Rendering:** Universal Render Pipeline (URP)
- **Language:** C# (.NET Standard 2.1)
- **Core Packages:**
  - `com.unity.render-pipelines.universal@17.2.0`
  - `com.unity.inputsystem@1.14.1`
  - `com.unity.test-framework@1.5.1`
  - `com.unity.ai.navigation@2.0.8`
  - `com.unity.timeline@1.8.7`

---

## 3. Build, Test, & Development Commands

**IMPORTANT:** Unity is **NOT** installed in the WSL environment. All Unity operations must be performed through the Windows Unity Editor or command line on the host.

- **Open Project (from Windows CMD/PowerShell):**
  ```sh
  "C:/Program Files/Unity/Hub/Editor/6000.2.0f1/Editor/Unity.exe" -projectPath .
  ```
- **Run EditMode Tests (from Windows CMD/PowerShell):**
  ```sh
  "C:/Program Files/Unity/Hub/Editor/6000.2.0f1/Editor/Unity.exe" -batchmode -projectPath . -runTests -testPlatform EditMode -logfile Logs/editmode-test-run.log
  ```
- **Run PlayMode Tests (from Windows CMD/PowerShell):**
  ```sh
  "C:/Program Files/Unity/Hub/Editor/6000.2.0f1/Editor/Unity.exe" -batchmode -projectPath . -runTests -testPlatform PlayMode -logfile Logs/playmode-test-run.log
  ```
- **Build Project (from Windows CMD/PowerShell):**
  - Use the Unity Editor: **File → Build Settings → Build** (or `Ctrl+B`).

---

## 4. Project Structure

All first-party content is located under `Assets/_DarkSeas/` to keep it separate from third-party assets.

```
Assets/
  _DarkSeas/                    # All project-owned content
    Art/                        # Art assets
    Audio/                      # Sound effects and music
    Data/                       # ScriptableObjects (configs, upgrades)
    Prefabs/                    # Game object prefabs (Boat, Hazards, UI, etc.)
    Scenes/                     # Main game scenes (Harbor, Expedition, Debrief)
    Scripts/                    # All C# scripts
      Core/                     # Events, services, utilities
      Gameplay/                 # Core gameplay systems (Boat, Hazards, Light, etc.)
      Meta/                     # Progression and harbor systems
      WorldGen/                 # Procedural generation
      UI/                       # HUD controllers, UI elements
      Data/                     # ScriptableObject class definitions
    Tests/                      # Unit and integration tests
      EditMode/                 # Editor-time tests (logic, services)
      PlayMode/                 # Runtime tests (interactions, timing)
  ThirdParty/                   # Imported assets
```

- **Assembly Definitions:** The project uses `.asmdef` files (`DarkSeas.Core.asmdef`, `DarkSeas.Gameplay.asmdef`) to reduce compile times. Ensure new scripts are added to the correct assembly.

---

## 5. Architecture & Code Philosophy

- **Component-First:** Use small, cohesive `MonoBehaviour`s with single responsibilities.
- **Event-Driven:** Decouple systems using static C# events (`*Signals` classes). Avoid direct references and `UnityEvent` for core gameplay loops.
- **Data-Driven:** Use `ScriptableObject`s for configuration and tuning (e.g., `BoatConfig`, `RunConfig`). Logic resides in C# classes, not assets.
- **State Machine:** The game follows a simple `Harbor → Expedition → Debrief` flow.
- **Pooling:** Pool transient objects like ice hazards and VFX to minimize garbage collection.
- **Clarity over Cleverness:** Write readable, maintainable code with descriptive names.

---

## 6. Coding Standards

- **Naming:** `PascalCase` for types and public/protected members. `_camelCase` for private fields.
- **Fields:** Prefer `[SerializeField] private` over `public` fields for component data.
- **Comments:** Use XML summaries for public APIs. Add comments to explain the *why*, not the *what*.
- **Units:** Use standard units (meters, seconds, degrees). Avoid magic numbers; define them in `ScriptableObject` configs.
- **Input:** Use the **Unity Input System** via the `InputSystem_Actions.inputactions` asset.

---

## 7. Event System ("Signals")

Communication between systems is handled by static "signal" classes.

- **`RunSignals`**: For run lifecycle events (start, end).
- **`HazardSignals`**: For collision and damage events.
- **`RescueSignals`**: For survivor pickup and delivery events.
- **`FuelSignals`**: For fuel state changes (e.g., empty).

**Example (`RunSignals.cs`):**
```csharp
public static class RunSignals {
    public static event Action<int> RunStart;           // Payload: seed
    public static event Action<string,int> RunEnd;      // Payload: result, rescuedCount
}
```

---

## 8. Data Model (ScriptableObjects)

Configuration is managed via `ScriptableObject` assets. Key definitions include:

- **`BoatConfig`**: Defines boat stats (speed, HP, fuel, seats, headlight).
- **`RunConfig`**: Defines run parameters (base fuel, collision damage curves).
- **`UpgradeDef`**: Defines a single purchasable upgrade (type, cost, value).

Logic should remain in `MonoBehaviour`s and services, while these assets should only store data.

---

## 9. Git Workflow & Committing

- **LFS:** The repository uses Git LFS for large assets. Ensure LFS is active.
- **Meta Files:** **Always** commit `.meta` files alongside their corresponding assets.
- **Ignore:** Never commit `Library/`, `Temp/`, or `obj/` folders (handled by `.gitignore`).
- **Commits:**
  - Use imperative mood (e.g., `Add fuel consumption feature` not `Added...`).
  - Write a concise subject line (≤72 characters).
  - Reference issues in the commit body where applicable.
- **Pre-Commit Checklist:**
  1. Ensure the project compiles without errors or warnings.
  2. Run EditMode and PlayMode tests.
  3. Confirm the main scenes (`HarborScene`, `ExpeditionScene`) are functional.

---

## 10. Key Files & Locations

- **Main Scenes:** `Assets/_DarkSeas/Scenes/`
- **Core Scripts:** `Assets/_DarkSeas/Scripts/`
- **ScriptableObject Data:** `Assets/_DarkSeas/Data/Resources/`
- **Input Actions:** `Assets/InputSystem_Actions.inputactions`
- **Package Dependencies:** `Packages/manifest.json`
- **Unity Version:** `ProjectSettings/ProjectVersion.txt`
