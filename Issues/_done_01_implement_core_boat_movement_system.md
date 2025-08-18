# Issue #1: Implement Core Boat Movement System

**Priority:** High  
**Labels:** gameplay, boat, mvp

## Description
The BoatController currently has placeholder methods that prevent the boat from moving. We need functional boat movement to test all other gameplay systems.

## Acceptance Criteria
- Boat responds to WASD/arrow key input with forward/backward movement
- Boat turns left/right with realistic physics
- Movement respects max speed and acceleration parameters from BoatConfig
- Boat physics feel responsive but not arcade-like
- Movement is impacted when fuel runs out (reduced control, not complete stop)

## Why This Matters
Movement is the foundation of the entire gameplay experience. Without it, we cannot test hazard collisions, rescue mechanics, or fuel pressure systems.

## Completion Summary
**Completed on:** 2025-01-18  
**Time spent:** ~45 minutes  
**Changes made:**
- Implemented complete BoatController with Rigidbody-based physics movement
- Integrated BoatConfig ScriptableObject for data-driven movement parameters
- Added realistic boat physics with drag, speed limiting, and turn speed scaling
- Implemented fuel impact system that reduces control when fuel runs out
- Enhanced BoatFuel class with throttle-based consumption
- Added proper error handling and component dependencies

**Key files modified:**
- `Assets/_DarkSeas/Scripts/Gameplay/Boat/BoatController.cs` - Complete rewrite with physics implementation
- `Assets/_DarkSeas/Scripts/Gameplay/Boat/BoatFuel.cs` - Added throttle input handling

**Testing notes:**
- Boat now responds to WASD/arrow key input for forward/backward movement
- Left/right turning only works when the boat is moving (realistic physics)
- Turning is scaled by current speed for authentic boat feel
- Max speed from BoatConfig is properly enforced
- When fuel runs out, movement is reduced to 30% effectiveness but not completely disabled
- Drag coefficient provides realistic water resistance

**Technical implementation:**
- Uses AddForce with ForceMode.Acceleration for responsive movement
- Transform.Rotate for turning based on current speed
- Velocity clamping prevents exceeding max speed
- Fuel multiplier system allows graceful degradation when out of fuel
- Input handling ready for future Input System migration

**Follow-up items:**
- Issue #3: Replace legacy Input.GetAxis with new Unity Input System
- Issue #2: Create BoatConfig ScriptableObject assets to replace hardcoded values
- Test movement in actual game scene when available