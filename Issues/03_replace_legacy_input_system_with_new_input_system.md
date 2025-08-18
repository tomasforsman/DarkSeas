# Issue #3: Replace Legacy Input System with New Input System

**Priority:** High  
**Labels:** input, technical-debt, mvp

## Description
The project uses the new Unity Input System but BoatController and RescueInteractor still use legacy Input.GetAxis() calls. This creates inconsistency and may cause input to not work on some platforms.

## Acceptance Criteria
- All Input.GetAxis() and Input.GetKeyDown() calls replaced with Input System
- Boat movement uses the existing InputSystem_Actions.inputactions asset
- Rescue interaction (E key) properly configured through Input System
- Input continues to work as expected for keyboard and gamepad

## Why This Matters
Consistent input handling is essential for a polished experience. The legacy input system is deprecated and may not work reliably across all target platforms.