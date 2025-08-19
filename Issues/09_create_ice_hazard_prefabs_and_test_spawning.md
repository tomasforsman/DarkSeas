# Issue #9: Create Ice Hazard Prefabs and Test Spawning

**Priority:** Medium  
**Labels:** hazards, prefabs, worldgen

## Description
HazardSpawner references ice prefabs that don't exist, preventing testing of procedural hazard placement and collision systems.

## Acceptance Criteria
- Small, Medium, and Large ice hazard prefabs with appropriate colliders
- Each size has different visual scale and damage values
- Prefabs are assigned to HazardSpawner component
- Spawning respects minimum spacing requirements
- Generated ice fields create interesting navigation challenges

## Why This Matters
Procedural hazard generation is key to replayability. Without proper ice prefabs, we cannot test or tune the difficulty curve of generated expeditions.

## Completion Summary
**Completed on:** 2025-08-18
**Time spent:** ~20 min
**Changes made:**
- Added Editor tool `DarkSeas → Create Ice Prefabs` to generate `Ice_Small`, `Ice_Medium`, `Ice_Large` under `Assets/_DarkSeas/Prefabs/Resources/Hazards` with `IceHazard` component and appropriate scale/damage/size.
- `HazardSpawner`: now auto-loads prefabs from `Resources/Hazards/*` if unassigned; pulls counts/spacing/area from `RunConfig`.
- Fallback path spawns primitive cubes with `IceHazard` if prefabs are still missing.

**Testing notes:**
- Run the menu to generate prefabs, add a `HazardSpawner` to the test scene and call `SpawnHazards(seed)` from a temporary script or via the Inspector.
- Verify spacing and collision behavior.

**Follow-up items:**
- Replace cubes with real models/materials; add drift behavior via `DriftField`.
