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