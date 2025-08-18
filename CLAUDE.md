# CLAUDE.md — Dark Seas (Unity)

Guidance for Claude Code when building **Dark Seas** in Unity. Keep it professional, modular, and pragmatic. Prioritize **code philosophy and style** over rigid content/layout rules. Use Unity-native patterns; avoid overengineering.

---

## 0) High-level goals

* Deliver the **MVP loop**: harbor → night expedition → debrief.
* Core systems: boat control, collisions/damage, headlight/visibility, drifting ice, single-survivor pickup & delivery, fuel/time pressure, small procedural patch, HUD + core SFX, simple legacy upgrades.
* Short runs (5–10 min), clear feedback, stable 60 FPS at 1080p.

---

## 1) Unity version & packages

* **Unity:** Latest LTS, **URP**.
* **Packages:** Input System, Cinemachine, TextMeshPro, Test Framework. Addressables optional.
* **Target:** Windows PC for MVP.
* **Rendering:** URP Forward, one main spotlight headlight (cookie), mild bloom; keep light count low.

---

## 2) Project structure (suggested, not mandatory)

Put **all first-party content** in a single root folder to keep third-party imports separate.

```
Assets/
  _DarkSeas/               # all project-owned content lives here
    Art/
    Audio/
    Data/                  # ScriptableObjects
    Prefabs/
      Boat/
      Hazards/
      Props/
      UI/
    Scenes/
      Harbor.unity
      Expedition.unity
      Debrief.unity
    Scripts/
      Core/                # Events, lightweight services (save, telemetry), utilities
      Gameplay/
        Boat/              # BoatController, BoatDamage, BoatFuel
        Hazards/           # IceHazard, HazardSpawner, DriftField
        Light/             # HeadlightController, VisibilityPulse
        Interaction/       # RescueTarget, RescueInteractor
        Run/               # RunStateMachine, RunConfig, RunSignals
      Meta/
        Legacy/            # LegacyManager, UpgradeCatalog
        Harbor/            # HarborView, UpgradeUI
      WorldGen/            # PatchGenerator, SeedService
      UI/                  # HUDController, Alerts, Compass
    Tests/
      PlayMode/
      EditMode/

  ThirdParty/              # imported assets (plugins, art packs, tools) live OUTSIDE _DarkSeas
```

Notes:

* Structure above is **guidance**, not law—rearrange when it improves clarity.
* Prefer assembly definitions per major folder in `_DarkSeas/Scripts` to keep compile times predictable.

---

## 3) Code philosophy

**Component-first, data-driven, decoupled.**

* Small, cohesive **MonoBehaviours** for gameplay. Keep responsibilities tight.
* **Interfaces + plain C# services** for logic that doesn’t need to be a component (save, telemetry, seeding, math helpers).
* **Events over references**: use C# events to decouple systems (`Signals.*` hubs); avoid UnityEvent in core loop.
* **ScriptableObjects** for tunable data (boat, upgrades, hazard sets, run config). Business logic stays in code, not SOs.
* **State machine**: `Harbor → Expedition → Debrief` (single scene with additive sub-scenes or lightweight sub-states).
* **Pool** transient objects (small ice, small VFX).
* Favor **clarity** over cleverness; short methods; descriptive names; minimal mutable shared state.

---

## 4) Coding standards

* **Naming:** PascalCase for types/members; `_camelCase` for private fields; explicit, descriptive identifiers.
* **Docs:** XML summaries for public APIs; keep comments practical (no tutorial prose).
* **Input:** Unity Input System; define an `InputMap` asset with rebinding later.
* **Time/Units:** Meters, seconds, degrees; no magic numbers—expose in config/SOs.
* **Error handling:** Guard early; prefer no-op/null patterns over `null` checks everywhere.
* **Testing:** EditMode tests for pure services/math; PlayMode for interaction timings and collisions.

---

## 5) Events & state hubs (lightweight)

Create static signal classes (one file per domain). Keep the payloads small and serializable.

```csharp
public static class RunSignals {
  public static event Action<int> RunStart;           // seed
  public static event Action<string,int> RunEnd;      // result, rescuedCount
}

public static class HazardSignals {
  public static event Action<int,float> CollideIce;   // size, relativeSpeed
}

public static class RescueSignals {
  public static event Action<string,float> PickedUp;  // id, time
}

public static class FuelSignals {
  public static event Action<float> Empty;            // time
}
```

These signals mirror telemetry fields.

---

## 6) Core systems (MVP scope & intent)

### Boat

* `BoatController` — top-down motion using Rigidbody. Cap max speed, apply acceleration, simple yaw.
* `BoatDamage` — collision impulse → damage; sink sequence ≤ 5s at 0 HP.
* `BoatFuel` — time-based drain + throttle modifier; at 0: engine off, dim control, light remains.

### Light & visibility

* `HeadlightController` — URP spotlight with cookie; tunable range/angle/intensity.
* Optional visibility pulse/beep later; keep materials emissive for readability.

### Hazards

* `IceHazard` (sizes S/M/L) — collider + damage table.
* `DriftField` — Perlin vector field with global wind scalar; applied to drifting ice only.
* `HazardSpawner` — patch placement from seed; min spacing; ensures target counts.

### Rescue

* `RescueTarget` — island/raft marker with trigger radius.
* `RescueInteractor` — hold-to-rescue (≈1.5s), boat seats/capacity from `BoatConfig`.
* Delivery: harbor trigger counts rescued and ends the run if player docks.

### Run/meta

* `RunStateMachine` — handles transitions and scene (or sub-scene) loads.
* `LegacyManager` — accumulates rescued → Legacy Points; persists between runs (JSON or PlayerPrefs for MVP).
* `UpgradeCatalog` (SO) + basic `HarborView` to buy 3–5 permanent upgrades (Light, Hull, Fuel, Seat +1).

### World generation

* `PatchGenerator` — seeds a small area (≈1–2 km²) with open water, ice fields, rocks, ≥1 survivor POI.
* `SeedService` — daily seed option for repeatability.

### UI & audio

* HUD: fuel bar, hull bar, seat occupancy, compass/heading, harbor marker with distance.
* SFX: engine RPM layers, ice scrape/impact, pickup chime, low-fuel cue; simple ambient loop.

---

## 7) Data model (ScriptableObjects)

```csharp
[CreateAssetMenu(menuName="DarkSeas/BoatConfig")]
public sealed class BoatConfig : ScriptableObject {
  public string id;
  public float maxSpeed;
  public float acceleration;
  public float turnRateDegPerSec;
  public int hullHP;
  public float fuelCapacity;
  public int seats;
  public float headlightRange;
  public float headlightAngle;
}

public enum UpgradeType { Light, Hull, Fuel, Seats }

[CreateAssetMenu(menuName="DarkSeas/UpgradeDef")]
public sealed class UpgradeDef : ScriptableObject {
  public string id;
  public UpgradeType type;
  public float value;       // % or flat; interpret by type
  public int legacyCost;
  public bool stackable;
}

[CreateAssetMenu(menuName="DarkSeas/RunConfig")]
public sealed class RunConfig : ScriptableObject {
  public float baseFuelSeconds = 180f;
  public AnimationCurve collisionDamageBySpeed;
  public float rescueHoldSeconds = 1.5f;
  public int minIceCount = 20;
}
```

Keep logic in components/services; SOs only store data.

---

## 8) Telemetry & save (pragmatic)

* **Telemetry:** newline-delimited JSON under `Application.persistentDataPath` with fields that match signals: `run_start`, `pickup`, `collide_ice`, `fuel_empty`, `run_end`, `upgrade_buy`.
* **Save:** legacy totals + purchases (PlayerPrefs or a single JSON file). Keep format stable; version field for migration.

---

## 9) Acceptance targets (light touch)

* Boat drives, collides, and can sink; fuel creates meaningful pressure (\~180s cruise).
* Visibility cone makes hazards readable; HUD communicates low fuel and damage clearly.
* At least one survivor can be rescued and delivered to end a successful run.
* Hazard patches spawn reliably with a seed; 10 seeds in a row produce no softlocks.

---

## 10) Performance budget (practical)

* 60 FPS @ 1080p on a mid-range PC.
* Pool small hazards and VFX; avoid per-frame allocations in hot loops.
* Keep dynamic lights minimal (headlight + occasional emissives).
* Prefer simple shaders; URP batching on.

---

## 11) Task backlog (suggested order)

1. BoatController + BoatDamage + BoatFuel
2. HeadlightController + HUD basics
3. IceHazard + HazardSpawner + DriftField
4. RescueTarget + RescueInteractor + harbor delivery
5. LegacyManager + UpgradeCatalog + HarborView
6. PatchGenerator + SeedService
7. Telemetry & Debrief screen

---

## 12) Example snippets

**Collision → damage (speed-scaled):**

```csharp
void OnCollisionEnter(Collision c) {
  if (!c.collider.TryGetComponent<IceHazard>(out var ice)) return;
  var relSpeed = c.relativeVelocity.magnitude;
  var dmg = ice.ComputeDamage(relSpeed); // table/curve on hazard or service
  _boatDamage.Apply(dmg);
  HazardSignals.CollideIce?.Invoke(ice.Size, relSpeed);
}
```

**Hold-to-rescue:**

```csharp
IEnumerator RescueCoroutine(RescueTarget target) {
  float t = 0f;
  while (t < _config.rescueHoldSeconds) {
    if (!target.InRange(transform) || Interrupted()) yield break;
    t += Time.deltaTime;
    yield return null;
  }
  _passengers.Add(target.Claim());
  RescueSignals.PickedUp?.Invoke(target.Id, Time.time);
}
```

---

## 13) Non-goals for MVP

Skip: day cycle and weather, advanced enemy behaviors, anchoring/island mechanics, multi-boat fleets, narrative events. Park as post-MVP options.

---

## Development Commands

### Building the Project
- Open project in Unity Editor 6000.2.0f1 or later
- Use **File > Build Settings** to configure build targets
- Build via **File > Build And Run** or Ctrl+B

### Unity Editor
- Open scenes from `Assets/_DarkSeas/Scenes/`
- Main scenes: HarborScene, ExpeditionScene, DebriefScene
- Scripts are located in `Assets/_DarkSeas/Scripts/`

### C# Development
- Uses .NET Standard 2.1
- Main assemblies: Assembly-CSharp.csproj, Assembly-CSharp-Editor.csproj
- Solution file: `Dark Seas.sln`

## Key Unity Packages

- **Universal Render Pipeline**: 17.2.0 (3D rendering)
- **Input System**: 1.14.1 (player input handling)
- **AI Navigation**: 2.0.8 (pathfinding)
- **Visual Scripting**: 1.9.7 (node-based scripting)
- **Timeline**: 1.8.7 (cinematic sequences)


## Git Workflow

- Repository uses Git LFS for Unity assets
- Main branch: `main`
- Recent changes include adding scenes and core game scripts

### Final notes

* Keep everything we own under `Assets/_DarkSeas/`. Import any external assets to `Assets/ThirdParty/` to avoid mixing.
* Treat this document as **direction, not constraint**. If a different layout or pattern makes a system clearer or safer, use it and keep the intent: component-first, data-driven, decoupled, readable.


