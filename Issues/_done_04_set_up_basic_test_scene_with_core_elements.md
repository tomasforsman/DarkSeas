# Issue #4: Set Up Basic Test Scene with Core Elements

**Priority:** High  
**Labels:** scene, testing, mvp

## Description
We need a simple scene to test boat movement, fuel consumption, and basic interactions before building the full expedition experience.

## Acceptance Criteria
- Scene contains water plane/surface for boat to move on
- Harbor docking point with visual indicator
- Player spawn point with properly configured boat prefab
- At least one test rescue target to verify rescue mechanics
- Basic lighting setup for headlight testing
- Scene serves as foundation for further development

## Why This Matters
A controlled test environment allows us to verify each system works before adding complexity. This prevents debugging multiple systems simultaneously.

## Completion Summary
**Completed on:** 2025-08-18
**Time spent:** ~20 min
**Changes made:**
- Added `Assets/_DarkSeas/Editor/CreateTestScene.cs` with menu item: DarkSeas → Create Test Scene.
- Programmatically creates a new scene with: water plane, harbor dock marker, directional light, boat (Rigidbody + BoatController + BoatFuel + RescueInteractor), and one `RescueTarget`.
- Assigns `DefaultBoatConfig` and tries to auto-assign `InputSystem_Actions` asset to components.

**Testing notes:**
- In Unity Editor, use the menu item to generate and save `Assets/_DarkSeas/Scenes/Test/TestScene.unity` then press Play.
- Verify boat moves with WASD/left stick; hold Interact to rescue.

**Follow-up items:**
- Replace primitive placeholders with prefabs once available; add headlight spotlight prefab for URP testing.
