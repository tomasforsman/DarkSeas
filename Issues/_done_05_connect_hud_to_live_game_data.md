# Issue #5: Connect HUD to Live Game Data

**Priority:** Medium  
**Labels:** ui, feedback, mvp

## Description
The HUDController exists but displays static or placeholder data. Players need real-time feedback on fuel, hull damage, passenger count, and harbor direction to make meaningful decisions.

## Acceptance Criteria
- Fuel bar accurately reflects current fuel levels and updates in real-time
- Hull bar shows damage when boat collides with hazards
- Passenger counter updates when survivors are rescued
- Compass shows direction and distance to harbor
- All UI elements are visually clear and update smoothly

## Why This Matters
Player feedback is crucial for creating tension and enabling strategic decisions. Without accurate HUD data, players cannot understand the consequences of their actions.

## Completion Summary
**Completed on:** 2025-08-18
**Time spent:** ~15 min
**Changes made:**
- `HUDController`: added `_playerTransform` and smoothing; compass now measures from player to harbor; fuel/hull bars LERP for smoother updates.
- Test scene generator (Issue #4) provides `HarborDock`; assign its transform to HUD in scene.

**Testing notes:**
- In the test scene, add a Canvas with HUDController and wire references: fuel slider, hull slider, passenger text, compass text; set harbor to `HarborDock` and leave player blank to auto-bind.

**Follow-up items:**
- Add a simple compass arrow and warnings (low fuel, low hull) to improve feedback.
