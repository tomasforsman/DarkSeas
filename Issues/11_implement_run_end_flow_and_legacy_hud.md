# Issue #11: Implement Run End Flow and Legacy HUD

**Priority:** Medium  
**Labels:** run, ui, progression, mvp

## Description
Define when a run ends (dock delivery or sinking) and present a clear debrief UI. Surface Legacy Points during play and show a delivery toast on turn-ins.

## Acceptance Criteria
- End triggers: docking with any passengers, or boat sinking → show Debrief panel.
- Debrief shows rescued count, points earned, total Legacy, and a Continue button (returns to Harbor/start state).
- Delivery toast appears on dock turn-in: “Delivered N → +P Legacy”.
- Always-visible HUD text shows total Legacy (updates on change).
- Emits `RunSignals.RunEnd(result, rescuedCount)` and integrates with `LegacyManager`.

## Why This Matters
Run closure and clear rewards create a satisfying loop and guide players back to the Harbor/meta layer.

