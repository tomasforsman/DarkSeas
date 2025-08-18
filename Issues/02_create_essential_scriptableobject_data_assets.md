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