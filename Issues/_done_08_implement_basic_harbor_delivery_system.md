# Issue #8: Implement Basic Harbor Delivery System

**Priority:** Medium  
**Labels:** harbor, delivery, mvp

## Description
Players can rescue survivors but have no way to complete the objective by delivering them to safety. This breaks the core gameplay loop.

## Acceptance Criteria
- Approaching harbor docking area with passengers triggers delivery
- Passengers are converted to Legacy Points through LegacyManager
- Visual/audio feedback confirms successful delivery
- Passenger count resets to zero after delivery
- System works as foundation for run completion

## Why This Matters
Without delivery, rescue becomes meaningless. The harbor delivery completes the core risk/reward loop and provides progression currency.

## Completion Summary
**Completed on:** 2025-08-18
**Time spent:** ~15 min
**Changes made:**
- Added `LegacyManager` singleton to track Legacy Points (`_pointsPerPassenger` default 1).
- Added `HarborDock` trigger that detects boats with `RescueInteractor`, converts passengers to points, and clears passenger list.
- Dock has cooldown to avoid double-delivery, and supports delivery on enter or stay.

**Testing notes:**
- In the test scene, select the `HarborDock` cube and add the `HarborDock` script; ensure its collider is trigger.
- Rescue passengers, drive into dock, observe log and passenger reset.

**Follow-up items:**
- Add UI for Legacy point totals and a proper RunEnd flow; integrate audio/FX on delivery.
