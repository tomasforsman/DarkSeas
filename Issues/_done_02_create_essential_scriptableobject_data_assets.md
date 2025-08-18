# Issue #2: Create Essential ScriptableObject Data Assets

**Priority:** High  
**Labels:** data, configuration, mvp

## Description
We have ScriptableObject classes defined but no actual data assets created. This blocks testing of data-driven systems and makes it impossible to tune gameplay balance.

## Acceptance Criteria
- Default BoatConfig asset with balanced movement/fuel/hull values
- RunConfig asset with reasonable expedition parameters (180s fuel, 1.5s rescue time)
- Basic UpgradeDef assets for Light, Hull, Fuel, and Seats upgrades
- All assets properly configured and assigned to relevant prefabs/managers

## Why This Matters
Data-driven design allows rapid iteration on game balance without code changes. Missing data assets prevent testing of core systems like fuel consumption and upgrades.
## Completion Summary
**Completed on:** 2025-08-18
**Time spent:** ~30 min
**Changes made:**
- Added default data assets under `Assets/_DarkSeas/Data/Resources/`:
  - `DefaultBoatConfig.asset`, `DefaultRunConfig.asset`
  - Upgrades: `Upgrade_Light.asset`, `Upgrade_Hull.asset`, `Upgrade_Fuel.asset`, `Upgrade_Seats.asset`
- Auto-load defaults when unassigned:
  - `BoatController` now loads `DefaultBoatConfig` from Resources if none assigned
  - `GameManager` now loads `DefaultRunConfig` from Resources at startup

**Testing notes:**
- Editor will auto-detect assets in `Resources/`; at runtime, Boat uses default config if prefab field is empty. GameManager exposes `RunConfig`.
- Values match MVP targets (fuel 180s, rescue 1.5s).

**Follow-up items:**
- When Harbor/Upgrades systems are added, consider an `UpgradeCatalog` ScriptableObject to group upgrade defs.
- Assign configs explicitly on prefabs to avoid Resources dependency in production.
