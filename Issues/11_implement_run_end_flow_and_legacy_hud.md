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
 
## Completion Summary
**Completed on:** 2025-08-18
**Time spent:** ~35 min
**Changes made:**
- Signals: added `DeliverySignals.Delivered(count, points)`; fire from `HarborDock`.
- Run end: `HarborDock` now invokes `RunSignals.RunEnd("Delivered", count)`; `BoatDamage` invokes `RunEnd("Sank", 0)`.
- `RunStateMachine`: minimal state holder (Harbor/Expedition/Debrief) and RunStart/End helpers.
- UI: `LegacyHUD` (always visible total), `DeliveryToast` (transient message), `DebriefPanel` (summary + Continue).
- Test scene generator extended to add Canvas with HUD, toast, and Debrief panel.

**Testing notes:**
- Generate the test scene; rescue a target and dock → toast appears and Debrief shows; Continue hides it. Sink to see failure summary.

**Follow-up items:**
- Load Harbor/Debrief dedicated scenes later; add save of Legacy via PlayerPrefs/JSON.

## Why This Matters
Run closure and clear rewards create a satisfying loop and guide players back to the Harbor/meta layer.
