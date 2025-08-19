# Issue #10: Fix Assembly Definition Dependencies

**Priority:** Low  
**Labels:** technical-debt, build

## Description
Several assembly definition files have incomplete references, which may cause compilation issues as the project grows and could prevent proper code completion in IDEs.

## Acceptance Criteria
- DarkSeas.UI.asmdef includes Unity.InputSystem reference
- DarkSeas.Gameplay.asmdef has correct Unity package references
- All assembly dependencies are properly configured
- Project compiles without warnings about missing references
- Code completion works correctly across assemblies

## Why This Matters
Proper assembly organization improves compile times and prevents hard-to-debug dependency issues as the codebase grows.

## Completion Summary
**Completed on:** 2025-08-18
**Time spent:** ~10 min
**Changes made:**
- `DarkSeas.UI.asmdef`: added `Unity.InputSystem` reference.
- `DarkSeas.Gameplay.asmdef`: ensured `Unity.InputSystem` reference is present alongside existing GUID reference.

**Testing notes:**
- Editor should resolve Input System types across assemblies; rebuild regenerates without missing reference warnings.

**Follow-up items:**
- Audit any additional packages as they’re introduced (Cinemachine, TMP) and add references where used.
