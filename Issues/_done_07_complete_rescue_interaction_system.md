# Issue #7: Complete Rescue Interaction System

**Priority:** Medium  
**Labels:** rescue, interaction, mvp

## Description
The rescue system has basic structure but needs refinement to create satisfying hold-to-rescue mechanics and proper passenger management.

## Acceptance Criteria
- Hold E near rescue target initiates rescue progress
- Visual/audio feedback shows rescue progress during hold interaction
- Rescue succeeds after holding for configured duration
- Rescued survivors are added to passenger count
- Boat cannot rescue more survivors than seat capacity
- Rescue is interrupted if boat moves away during interaction

## Why This Matters
Rescue is the primary objective and reward mechanism. A polished rescue interaction makes the core gameplay loop satisfying and clear.

## Completion Summary
**Completed on:** 2025-08-18
**Time spent:** ~25 min
**Changes made:**
- `RescueInteractor`: pulls duration from RunConfig; emits Started/Progress/Canceled signals; maintains hold-to-rescue and capacity checks.
- `RescueSignals`: added `Started`, `Progress`, `Canceled` events (kept `PickedUp`).
- `RescueProgressUI`: optional HUD helper that displays a progress bar and plays audio clips based on signals.

**Testing notes:**
- In the test scene, add a Canvas with a Slider and a CanvasGroup; attach `RescueProgressUI` and wire fields.
- Hold Interact near `RescueTarget` to see progress; move away to cancel; on success, bar hides and complete sound plays.

**Follow-up items:**
- Add world-space progress ring on target; integrate with SFX/VFX library.
