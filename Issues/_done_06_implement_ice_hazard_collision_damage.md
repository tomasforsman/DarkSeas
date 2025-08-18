# Issue #6: Implement Ice Hazard Collision Damage

**Priority:** Medium  
**Labels:** hazards, collision, mvp

## Description
Ice hazards exist but don't actually damage the boat when collided with. This removes the primary risk element from the risk/reward gameplay loop.

## Acceptance Criteria
- Boat takes damage when colliding with ice based on collision speed
- Different ice sizes cause different amounts of damage
- Damage calculation uses the curve defined in RunConfig
- Hull HP decreases and is reflected in the HUD
- Boat sinks when hull HP reaches zero

## Why This Matters
Risk is essential to create tension and make navigation decisions meaningful. Without collision damage, there's no penalty for careless navigation.

## Completion Summary
**Completed on:** 2025-08-18
**Time spent:** ~20 min
**Changes made:**
- `IceHazard`: damage now uses `RunConfig.collisionDamageBySpeed` curve scaled by ice size; renamed base field to `_baseDamage`.
- `BoatDamage`: implemented sinking sequence over `_sinkDuration` and disabled `BoatController` on 0 HP.

**Testing notes:**
- In test scene, add a few `IceHazard` cubes with sizes 1–3 and collide at varying speeds; observe HUD hull bar drop and sinking at 0 HP.

**Follow-up items:**
- Add VFX/SFX on impact and during sinking; tune curve values in `DefaultRunConfig`.

