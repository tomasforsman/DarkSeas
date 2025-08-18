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