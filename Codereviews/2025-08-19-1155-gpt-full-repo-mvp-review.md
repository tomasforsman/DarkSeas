# Code Review – Dark Seas Unity MVP

**Date/Time:** 2025-08-19 11:55  
**Agent:** gpt

## 1. Specific Requirements
Provide a full static code review of the current codebase (Unity project), independent of recent changes. Focus on architecture, quality, Unity best practices, and actionable recommendations.

## 2. Changes Since Last Review
First review in Codereviews; no prior file to compare.

## 3. Current Code State

### Game Systems
- Core loop foundations exist: boat control, fuel, collision damage via `IceHazard`, rescue hold-to-interact, harbor delivery awarding Legacy points, and a minimal run state machine with Debrief UI.
- Signals provide loose coupling (`RunSignals`, `RescueSignals`, `FuelSignals`, `DeliverySignals`), aligning with the decoupled architecture goal.
- Editor tools accelerate iteration: test scene generator and ice prefab generator.

### Architecture  
- Clear module separation under `Assets/_DarkSeas/Scripts/{Core,Gameplay,Meta,UI,WorldGen}`, with asmdefs per module and Input System references configured.
- Data-driven patterns are in place: `BoatConfig`, `RunConfig`, `UpgradeDef` with default assets in `Resources/`.
- Input System is integrated behind `ENABLE_INPUT_SYSTEM`; injection uses serialized `InputActionAsset`.

### Code Quality
- Consistent naming: PascalCase for types, `_camelCase` for private fields.
- Cohesive MonoBehaviours; events used for decoupling; helpful Debug logs and warnings.
- Documentation is light but pragmatic; public APIs could benefit from XML summaries over time.

### Resource Management
- Uses `Resources.Load` for configs and hazard prefabs; fine for MVP, consider Addressables later.
- No pooling yet (hazards, VFX), acceptable at MVP but flagged for performance later.
- Editor-only creation uses `DestroyImmediate` in `HazardSpawner` (see attention).

## 4. Areas Requiring Attention

**Issue:** Physics API usage in movement  
**Location:** `Assets/_DarkSeas/Scripts/Gameplay/Boat/BoatController.cs:120–140`

**Current Code:**
```csharp
float turnAmount = _inputVector.x * _boatConfig.turnRateDegPerSec * Time.fixedDeltaTime * fuelMultiplier;
transform.Rotate(0, turnAmount, 0);
_rigidbody.velocity *= _dragCoefficient;
_rigidbody.angularVelocity *= _dragCoefficient;
```

**Recommendation:** (High) Use physics-friendly APIs: `Rigidbody.MoveRotation` (or torque) for turning, and `rb.drag`/`rb.angularDrag` instead of manual velocity scaling to avoid interfering with the solver. Clamp speeds via forces, not direct velocity writes.

---

**Issue:** Editor-only destruction in runtime spawner  
**Location:** `Assets/_DarkSeas/Scripts/Gameplay/Hazards/HazardSpawner.cs:101–107`

**Current Code:**
```csharp
DestroyImmediate(hazard);
```

**Recommendation:** (Critical) Replace with `Destroy(hazard)` for runtime. Reserve `DestroyImmediate` for editor tools guarded by `#if UNITY_EDITOR`.

---

**Issue:** Target acquisition cost for rescue  
**Location:** `Assets/_DarkSeas/Scripts/Gameplay/Interaction/RescueInteractor.cs:147–157`

**Current Code:**
```csharp
RescueTarget[] targets = FindObjectsOfType<RescueTarget>();
foreach (var target in targets) { if (target.InRange(transform)) return target; }
```

**Recommendation:** (High) Use trigger colliders on `RescueTarget` + `OnTriggerEnter/Exit` to track nearby targets, or `Physics.OverlapSphere` per attempt; avoid `FindObjectsOfType` scans.

---

**Issue:** Runtime singleton creation  
**Location:** `Assets/_DarkSeas/Scripts/Meta/Legacy/LegacyManager.cs:15–21`

**Current Code:**
```csharp
if (_instance == null) { var go = new GameObject("LegacyManager"); _instance = go.AddComponent<LegacyManager>(); DontDestroyOnLoad(go); }
```

**Recommendation:** (Medium) Prefer a scene-placed bootstrap or a dedicated `Services` prefab to avoid runtime object creation and ordering concerns; add persistence (PlayerPrefs/JSON).

---

**Issue:** Direct transform rotation in physics step  
**Location:** `Assets/_DarkSeas/Scripts/Gameplay/Boat/BoatController.cs:129`

**Recommendation:** (High) Replace with `MoveRotation` or torque; ensure all kinematic updates happen via Rigidbody methods.

---

**Issue:** `HazardSpawner` fallback creates primitives without mass/collider tuning  
**Location:** `Assets/_DarkSeas/Scripts/Gameplay/Hazards/HazardSpawner.cs:62–69`

**Recommendation:** (Medium) Ensure created hazards match prefabs’ colliders/masses; otherwise collisions will feel inconsistent.

---

**Issue:** HUD uses legacy `UnityEngine.UI.Text`  
**Location:** `Assets/_DarkSeas/Scripts/UI/*`

**Recommendation:** (Low) Plan migration to TextMeshPro for clarity and performance; add null guards and serialized references in scenes over runtime discovery where possible.

---

**Issue:** Input System asset injection  
**Location:** `BoatController.cs:70–81`, `RescueInteractor.cs:46–60`

**Recommendation:** (Medium) Standardize via a `PlayerInput` component or a central `InputProvider` ScriptableObject; avoid enabling individual actions repeatedly; prefer enabling the map once in `OnEnable`.

---

**Issue:** Resource paths are stringly-typed  
**Location:** Multiple (e.g., `Resources.Load<BoatConfig>("DefaultBoatConfig")`)

**Recommendation:** (Low) Centralize resource path constants or switch to Addressables; ensure missing-asset paths are unit-tested.

## 5. Development Priorities

### Critical
- Replace `DestroyImmediate` with `Destroy` in `HazardSpawner` and guard editor-only logic.
- Rework boat rotation and damping to proper Rigidbody APIs (MoveRotation/torque, drag/angularDrag).

### High Impact  
- Optimize rescue target detection (trigger-based or overlap queries).
- Standardize Input System wiring via `PlayerInput` or a shared provider; enable/disable action maps in lifecycle methods.

### Quality
- Add XML doc comments to public APIs in `Data/` and core systems.
- Remove legacy input fallbacks once scenes use the new Input System everywhere.
- Introduce simple error surfaces in UI (e.g., missing config warnings).

### Enhancement
- Add basic persistence for Legacy points (PlayerPrefs/JSON).
- Pool hazards and small VFX when worldgen/VFX arrive.
- Migrate UI text to TMP; add compass arrow and low fuel/hull warnings.
- Add EditMode tests (BoatFuel consumption, IceHazard damage curve) and PlayMode tests (rescue coroutine timing, harbor delivery).
