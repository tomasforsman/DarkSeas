# Dark Seas

A Unity-based survival game where players captain a boat through dangerous Arctic waters to rescue survivors. Navigate by headlight, manage fuel and hull damage, and deliver survivors back to harbor in this atmospheric top-down experience.

## Gameplay

**Core Loop:** Harbor → Night Expedition → Debrief

- **Harbor:** Upgrade your boat using Legacy Points earned from successful rescues
- **Expedition:** Navigate treacherous ice fields by headlight to find and rescue survivors
- **Debrief:** Review your performance and spend earned Legacy Points

### Key Mechanics

- **Boat Navigation:** Top-down Rigidbody-based movement with realistic acceleration and turning
- **Headlight System:** Limited visibility cone - navigate carefully to avoid hazards
- **Collision Damage:** Ice hazards damage your hull based on impact speed
- **Fuel Management:** Limited expedition time creates pressure to be efficient
- **Survivor Rescue:** Find survivors on ice floes and rafts, rescue with hold-to-interact
- **Drifting Ice:** Dynamic hazard field that moves with wind patterns

## Technical Details

- **Unity Version:** 6000.2.0f1 (Unity 6 LTS)
- **Rendering Pipeline:** Universal Render Pipeline (URP)
- **Target Platform:** Windows PC (1080p, 60 FPS)
- **Architecture:** Component-first, event-driven design
- **Input System:** Unity Input System with rebindable controls

## Project Structure

```
Assets/_DarkSeas/          # All project content
├── Art/                   # Visual assets
├── Audio/                 # Sound effects and music
├── Data/                  # ScriptableObject configurations
├── Prefabs/              # Game object prefabs
├── Scenes/               # Main game scenes
│   ├── HarborScene.unity
│   ├── ExpeditionScene.unity
│   └── DebriefScene.unity
├── Scripts/              # C# source code
│   ├── Core/             # Events and utilities
│   ├── Gameplay/         # Core game systems
│   ├── Meta/             # Progression systems
│   ├── UI/               # User interface
│   └── WorldGen/         # Procedural generation
└── Tests/                # Unit and integration tests
```

## Development Setup

### Requirements

- Unity 6000.2.0f1 or later
- Git with LFS support
- Visual Studio or JetBrains Rider

### Getting Started

1. **Clone Repository**
   ```bash
   git clone <repository-url>
   cd "Dark Seas"
   ```

2. **Open in Unity**
   - Open Unity Hub
   - Click "Add" and select the project folder
   - Open with Unity 6000.2.0f1+

3. **Test Setup**
   - Open `HarborScene.unity`
   - Press Play to verify basic functionality

### Building

- Use **File → Build Settings** to configure build
- Build via **File → Build And Run** or **Ctrl+B**
- Target: Windows Standalone

## Architecture Overview

### Code Philosophy

- **Component-First:** Small, focused MonoBehaviours
- **Event-Driven:** Decoupled systems using C# events
- **Data-Driven:** Configuration via ScriptableObjects
- **Performance-Focused:** Object pooling and optimized rendering

### Key Systems

- **Boat Systems:** `BoatController`, `BoatDamage`, `BoatFuel`
- **Lighting:** `HeadlightController` with URP spotlight
- **Hazards:** `IceHazard`, `HazardSpawner`, `DriftField`
- **Rescue:** `RescueTarget`, `RescueInteractor`
- **Progression:** `LegacyManager`, upgrade system
- **World Generation:** `PatchGenerator`, `SeedService`

### Event System

Uses static signal classes for loose coupling:

```csharp
public static class RunSignals {
    public static event Action<int> RunStart;
    public static event Action<string,int> RunEnd;
}
```

## Contributing

### Coding Standards

- **Naming:** PascalCase for types, `_camelCase` for private fields
- **Documentation:** XML summaries for public APIs
- **Testing:** Write tests for core gameplay systems
- **Performance:** Target 60 FPS, profile expensive operations

### Before Committing

1. Ensure all scripts compile without warnings
2. Test in all three main scenes
3. Verify performance in expedition scene
4. Run existing unit tests

See `CLAUDE.md` for comprehensive development guidelines and `/.github/copilot-instructions.md` for detailed architecture information.

## Game Design

### Target Experience

- **Session Length:** 5-10 minutes per expedition
- **Difficulty:** Meaningful resource management without frustration
- **Atmosphere:** Tense navigation with moments of relief upon rescue
- **Progression:** Clear upgrade paths that impact gameplay

### MVP Features

- ✅ Boat movement and physics
- ✅ Headlight visibility system  
- ✅ Ice hazard collision damage
- ✅ Fuel management with time pressure
- ✅ Survivor rescue mechanics
- ✅ Harbor progression system
- ✅ Procedural ice field generation

## License

[Add appropriate license information]

---

*Dark Seas - Navigate the darkness, save the survivors, return home.*