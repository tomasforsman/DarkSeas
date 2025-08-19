# Code Review – Full Codebase Initial Assessment

**Date/Time:** 2025-08-19 11:57  
**Agent:** Gemini

## 1. Specific Requirements

This review provides a full, static analysis of the entire codebase in its current state. It serves as an initial architectural and quality baseline.

## 2. Changes Since Last Review

Not applicable. This is the first comprehensive review of the project.

## 3. Current Code State

The codebase is well-structured, clean, and demonstrates a strong adherence to the project's established architectural principles.

### Game Systems
- **Core Loop:** The gameplay loop is managed by `RunStateMachine.cs`, which correctly handles the `Harbor`, `Expedition`, and `Debrief` states. State transitions are driven by events from the `RunSignals` class, which is a clean and effective approach.
- **Boat Mechanics:** The boat's functionality is logically split across `BoatController`, `BoatDamage`, and `BoatFuel`. The controller handles movement physics, while the other two components manage their respective states. The use of a `BoatConfig` ScriptableObject makes tuning easy.
- **Hazard System:** `IceHazard.cs` defines the hazard's properties, and its `OnCollisionEnter` correctly computes damage and notifies the `BoatDamage` component. `HazardSpawner.cs` handles procedural placement based on a seed, which aligns with the design goals.
- **Rescue System:** The `RescueTarget` and `RescueInteractor` components provide a complete hold-to-interact mechanic. The system is properly decoupled, using `RescueSignals` to broadcast progress to UI elements like `RescueProgressUI.cs`.

### Architecture
- **Module Separation:** The project structure under `Assets/_DarkSeas/Scripts/` is excellent, with clear separation between `Core` (signals), `Gameplay`, `Meta`, `UI`, and `WorldGen`. This is reinforced by assembly definitions, which improve compile times and enforce dependencies.
- **Event-Driven Design:** The "Signals" pattern is the architectural cornerstone and is used consistently across the codebase. This decouples systems effectively (e.g., `BoatDamage` raises a `RunSignals.RunEnd` event without needing a direct reference to the state machine).
- **Data-Driven Configuration:** The use of ScriptableObjects (`BoatConfig`, `RunConfig`, `UpgradeDef`) to externalize design parameters is implemented correctly. This allows designers to tune the game without modifying code.
- **Singleton/Manager Pattern:** The project uses singletons for global managers like `GameManager` and `LegacyManager`. This provides a centralized access point for shared configuration and state.

### Code Quality
- **Naming and Style:** The code consistently follows C# and Unity conventions, including `PascalCase` for methods/properties and `_camelCase` for private fields. The use of `[SerializeField] private` instead of public fields is correctly applied.
- **Readability:** The code is generally self-documenting due to clear naming and small, focused components.
- **Documentation:** Public classes and methods have good XML documentation, explaining their purpose and parameters.
- **Error Handling:** Components include checks for required components (`RequireComponent`) and log warnings or errors when dependencies are missing (e.g., `BoatController` checking for a `Rigidbody`).

### Resource Management
- **Asset Loading:** A common pattern is to use direct `[SerializeField]` references, with a fallback to `Resources.Load()` if the reference is missing (e.g., `GameManager`, `HazardSpawner`). This provides both flexibility and robustness.
- **Object Lifecycle:** `HazardSpawner` includes logic to `DestroyImmediate` existing hazards before spawning new ones. This is appropriate for editor-time generation.

## 4. Areas Requiring Attention

Despite the high quality, several areas could be improved for robustness and maintainability.

### High - Singleton Implementation in LegacyManager
**Issue:** The singleton pattern in `LegacyManager.cs` is not robust. It creates a new `GameObject` if an instance doesn't exist, which can lead to multiple instances being created accidentally across scene loads or if another script creates one.
**Location:** `Assets/_DarkSeas/Scripts/Meta/Legacy/LegacyManager.cs:13-24`
**Current Code:**
```csharp
public static LegacyManager Instance
{
    get
    {
        if (_instance == null)
        {
            var go = new GameObject("LegacyManager");
            _instance = go.AddComponent<LegacyManager>();
            DontDestroyOnLoad(go);
        }
        return _instance;
    }
}
```
**Recommendation:** Adopt the simpler and more common singleton pattern used in `GameManager.cs`, which relies on an instance being present in the scene and assigning itself in `Awake()`. This ensures a single, scene-managed instance.

### Medium - Duplicate Delivery Logic
**Issue:** Both `HarborDock.cs` and `HarborManager.cs` appear to handle passenger delivery. `HarborDock` uses a trigger collider, while `HarborManager` polls distance. This creates redundant logic and potential for conflicting behaviors.
**Location:**
- `Assets/_DarkSeas/Scripts/Meta/Harbor/HarborDock.cs`
- `Assets/_DarkSeas/Scripts/Meta/Harbor/HarborManager.cs`
**Recommendation:** Consolidate all delivery logic into a single component. `HarborDock`'s trigger-based approach is more efficient and Unity-idiomatic than polling distances in `Update`. The `HarborManager` should be removed or repurposed.

### Medium - Hardcoded Resource Paths
**Issue:** Several components use hardcoded strings to load assets from the `Resources` folder. This is brittle and can lead to runtime errors if paths change.
**Location:**
- `GameManager.cs:41`: `Resources.Load<RunConfig>("DefaultRunConfig");`
- `HazardSpawner.cs:38`: `Resources.Load<GameObject>("Hazards/Ice_Small");`
**Recommendation:** Move these strings into a static configuration class or, preferably, create a central `GameSettings` ScriptableObject that holds direct references to these default prefabs and configurations.

### Low - Incomplete Input System Transition
**Issue:** `BoatController.cs` and `RescueInteractor.cs` contain both the new Input System logic (wrapped in `#if ENABLE_INPUT_SYSTEM`) and fallback code for the legacy `Input.GetAxis`. This indicates the transition is incomplete and adds unnecessary complexity.
**Location:**
- `Assets/_DarkSeas/Scripts/Gameplay/Boat/BoatController.cs:61-67`
- `Assets/_DarkSeas/Scripts/Gameplay/Interaction/RescueInteractor.cs:53-56`
**Recommendation:** Once the project fully commits to the new Input System, remove the legacy input code and the `#if` directives to clean up the scripts.

### Low - Polling in UI Update
**Issue:** `LegacyHUD.cs` updates its text based on a `DeliverySignals` event, but also includes a fallback in `Update()` that runs every 30 frames. This polling is inefficient and redundant in an event-driven architecture.
**Location:** `Assets/_DarkSeas/Scripts/UI/LegacyHUD.cs:33-37`
**Current Code:**
```csharp
private void Update()
{
    // Fallback: keep updated even without signals
    if (Time.frameCount % 30 == 0) UpdateText();
}
```
**Recommendation:** Remove the `Update` method entirely. The `OnDelivered` event handler is sufficient to keep the HUD updated.

## 5. Development Priorities

### High Impact
1.  **Consolidate Delivery Logic:** Refactor `HarborDock` and `HarborManager` into a single, reliable system for ending a run at the harbor. This will prevent potential bugs related to passenger delivery.
2.  **Implement Object Pooling:** The documentation mentions pooling, but it is not yet implemented for hazards. For performance, create a pooling system for `IceHazard` instances instead of using `Instantiate`/`Destroy` at runtime.

### Quality
1.  **Refactor LegacyManager Singleton:** Update the `LegacyManager` to use the same robust, scene-based singleton pattern as `GameManager`.
2.  **Centralize Resource Paths:** Remove hardcoded `Resources.Load` strings and manage default assets via a central ScriptableObject.
3.  **Finalize Input System:** Remove the legacy input fallback code from all scripts to simplify and commit fully to the new Input System.
4.  **Make UI Event-Driven:** Remove the polling `Update` method from `LegacyHUD.cs`.

### Enhancement
1.  **Flesh out GameManager:** The `InitializeGame` method in `GameManager.cs` is currently empty. This could be used to initialize key services or managers at startup.
2.  **Complete Compass UI:** The `HUDController.cs` has a `// TODO` to add a directional arrow to the compass. This would be a valuable UI improvement.
