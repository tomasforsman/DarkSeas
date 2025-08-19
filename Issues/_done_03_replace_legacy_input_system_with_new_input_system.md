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

## Completion Summary
**Completed on:** 2025-08-18
**Time spent:** ~25 min
**Changes made:**
- `BoatController`: reads `Player/Move` from `InputActionAsset`; removed legacy `Input.GetAxis` under ENABLE_INPUT_SYSTEM.
- `RescueInteractor`: wires `Player/Interact` action; uses started/canceled callbacks; removed legacy `Input.GetKey*` under ENABLE_INPUT_SYSTEM.
- Added serialized `InputActionAsset` fields for assignment to `InputSystem_Actions` asset.

**Testing notes:**
- Requires assigning the `InputSystem_Actions` asset to both components in the Editor or providing a `PlayerInput` setup.
- Hold interaction tested via action with Hold interaction configured in asset.

**Follow-up items:**
- Optionally add a small bootstrap that finds a shared `InputActionAsset` (e.g., via `PlayerInput` on a central object) and injects it into controllers at runtime.
