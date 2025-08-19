# Copilot Instructions - Dark Seas Unity Project

## Project Overview

Dark Seas is a Unity-based survival game implementing a harbor → night expedition → debrief gameplay loop. Players navigate a boat through dangerous waters to rescue survivors while managing fuel, avoiding ice hazards, and dealing with hull damage. The game features procedurally generated patches, legacy progression system, and atmospheric lighting mechanics.

**Key Stats:**
- Unity Version: 6000.2.0f1 (Unity 6 LTS)
- Target Platform: Windows PC
- Rendering: Universal Render Pipeline (URP)
- Architecture: Component-first, event-driven design
- Language: C# (.NET Standard 2.1)

## Build and Development Instructions

### Prerequisites
- Unity Editor 6000.2.0f1 or later with URP
- Git with LFS enabled
- Visual Studio or JetBrains Rider for C# development

### Build Process
**IMPORTANT:** Unity is NOT installed in WSL on this machine. All Unity operations must be performed through the Windows Unity Editor.

1. **Opening the Project:**
   ```
   Open Unity Hub → Add Project → Select "Dark Seas" folder
   Open project in Unity Editor 6000.2.0f1+
   ```

2. **Building:**
   ```
   File → Build Settings
   Select Windows target platform
   Build via File → Build And Run or Ctrl+B
   ```

3. **Testing:**
   - Use Unity's built-in Test Runner (Window → General → Test Runner)
   - EditMode tests for pure services/math located in `Assets/_DarkSeas/Tests/EditMode/`
   - PlayMode tests for interaction timings located in `Assets/_DarkSeas/Tests/PlayMode/`

4. **Git Workflow:**
   - Repository uses Git LFS for Unity assets
   - Main branch: `main`
   - Always commit Unity meta files alongside assets
   - Avoid committing Library/, Temp/, and obj/ folders (handled by .gitignore)

## Project Architecture and Layout

### Core Structure
All first-party content lives under `Assets/_DarkSeas/` to separate from third-party imports in `Assets/ThirdParty/`.

```
Assets/
  _DarkSeas/                    # All project-owned content
    Art/                        # Art assets
    Audio/                      # Sound effects and music
    Data/                       # ScriptableObjects (configs, upgrades)
    Prefabs/                    # Game object prefabs
      Boat/                     # Boat-related prefabs
      Hazards/                  # Ice and obstacle prefabs
      Props/                    # Environmental objects
      UI/                       # UI prefabs
    Scenes/                     # Main game scenes
      HarborScene.unity         # Harbor/meta progression scene
      ExpeditionScene.unity     # Main gameplay scene
      DebriefScene.unity        # Post-run results scene
    Scripts/                    # All C# scripts
      Core/                     # Events, services, utilities
      Gameplay/                 # Core gameplay systems
        Boat/                   # BoatController, BoatDamage, BoatFuel
        Hazards/                # IceHazard, HazardSpawner, DriftField
        Light/                  # HeadlightController, VisibilityPulse
        Interaction/            # RescueTarget, RescueInteractor
        Run/                    # RunStateMachine, RunConfig
      Meta/                     # Progression and harbor systems
        Legacy/                 # LegacyManager, UpgradeCatalog
        Harbor/                 # HarborManager, UI
      WorldGen/                 # PatchGenerator, SeedService
      UI/                       # HUDController, Alerts, Compass
      Data/                     # ScriptableObject definitions
    Tests/                      # Unit and integration tests
      EditMode/                 # Editor-time tests
      PlayMode/                 # Runtime tests
```

### Key Configuration Files
- `Packages/manifest.json` - Unity package dependencies
- `ProjectSettings/ProjectVersion.txt` - Unity version
- `Assets/_DarkSeas/Scripts/DarkSeas.Core.asmdef` - Assembly definition
- `CLAUDE.md` - Comprehensive project guidelines and coding standards

## Coding Standards and Philosophy

### Architecture Principles
- **Component-first:** Small, cohesive MonoBehaviours with tight responsibilities
- **Event-driven:** Use C# events for decoupling (avoid UnityEvent in core loop)
- **Data-driven:** ScriptableObjects for configuration, logic stays in code
- **State machine:** Harbor → Expedition → Debrief state flow
- **Pooling:** Pool transient objects (ice, VFX) for performance

### Code Style
- **Naming:** PascalCase for types/members, `_camelCase` for private fields
- **Documentation:** XML summaries for public APIs, practical comments only
- **Input:** Unity Input System with `InputMap` assets
- **Units:** Meters, seconds, degrees - no magic numbers
- **Error handling:** Guard early, prefer no-op patterns over null checks

### Event System
The project uses static signal classes for loose coupling:

```csharp
// Example from RunSignals.cs:4
public static class RunSignals {
    public static event Action<int> RunStart;
    public static event Action<string,int> RunEnd;
}
```

Key signal classes:
- `RunSignals` - Run lifecycle events
- `HazardSignals` - Collision and damage events  
- `RescueSignals` - Pickup and delivery events
- `FuelSignals` - Fuel depletion events

## Critical Implementation Notes

### Performance Requirements
- Target: 60 FPS @ 1080p on mid-range PC
- Pool small objects (ice chunks, VFX particles)
- Minimize per-frame allocations in Update loops
- Keep dynamic light count low (mainly headlight spotlight)

### Key Systems Integration
1. **Boat Physics:** Rigidbody-based movement with collision damage scaling
2. **Lighting:** URP spotlight with cookie for headlight, emissive materials
3. **World Generation:** Seed-based procedural patch generation
4. **Progression:** Legacy points from rescued survivors → permanent upgrades

### Assembly Definitions
- Main assembly: `DarkSeas.Core` (no external references)
- Namespace: `DarkSeas.Core` for all scripts
- Use assembly definitions per major folder to optimize compile times

## Validation and Quality Assurance

### Before Committing Code
1. **Compilation:** Ensure all scripts compile without errors/warnings
2. **Scene Validation:** Test in all three main scenes (Harbor/Expedition/Debrief)
3. **Performance:** Profile frame rate in expedition scene with hazards
4. **Input Testing:** Verify boat controls and UI interactions work

### Common Pitfalls to Avoid
- Don't add components to update core game state directly - use events
- Don't hardcode values - use ScriptableObject configs
- Don't use Update() for expensive operations - cache and batch
- Don't create circular dependencies between assemblies
- Don't commit without testing basic boat movement and collision
- Ignore the files in Issues/ and Codereviews/ unless specifically asked to read them

### File Management
- **Always create:** Both .cs and .cs.meta files for scripts
- **Never commit:** Library/, Temp/, obj/ folders
- **Always version:** .unity, .asset, .prefab, and .asmdef files
- **Use Git LFS:** For art assets, audio files, and large binaries

## Dependencies and Packages

**Core Unity Packages:**
- `com.unity.render-pipelines.universal@17.2.0` - URP rendering
- `com.unity.inputsystem@1.14.1` - Modern input handling
- `com.unity.ai.navigation@2.0.8` - AI pathfinding (future use)
- `com.unity.timeline@1.8.7` - Cinematic sequences
- `com.unity.test-framework@1.5.1` - Unit testing

**Development Tools:**
- Visual Studio/Rider integration via IDE packages
- Git LFS for asset management
- URP shader support for custom materials

## Trust These Instructions

The information above has been validated against the current project state. Only search for additional information if:
1. These instructions appear incomplete for your specific task
2. You encounter errors that contradict the guidance above
3. You need details about specific implementation patterns not covered

For any gameplay logic, refer to existing implementations in `Assets/_DarkSeas/Scripts/Gameplay/` as authoritative examples of the project's patterns and conventions.