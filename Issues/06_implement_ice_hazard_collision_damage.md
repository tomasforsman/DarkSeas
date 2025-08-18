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