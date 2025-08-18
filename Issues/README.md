# Issues Folder Instructions

This folder contains individual issue files for the Dark Seas project. Each issue represents a specific task or problem that needs to be addressed.

## File Naming Convention

Issues should be named using the following format:
```
{issuenumber}_{issue_name_in_lowercase_with_underscores}.md
```

Examples:
- `01_implement_core_boat_movement_system.md`
- `02_create_essential_scriptableobject_data_assets.md`

## Issue File Structure

Each issue file should contain:
- **Title** - Clear, descriptive heading
- **Priority** - High/Medium/Low
- **Labels** - Relevant tags for categorization
- **Description** - Brief overview of the problem/task
- **Acceptance Criteria** - Specific, testable requirements
- **Why This Matters** - Context and rationale

## Working on Issues

### Before Starting
1. Choose an issue based on priority and dependencies
2. Read through all acceptance criteria carefully
3. Understand the "Why This Matters" context
4. Check for any dependencies on other issues

### During Development
1. Work through acceptance criteria systematically
2. Test each requirement as you implement it
3. Follow the coding standards in `CLAUDE.md`
4. Update the issue file with progress notes if needed

### After Completion
1. **Rename the file** to include `_done_` prefix:
   ```
   _done_01_implement_core_boat_movement_system.md
   ```

2. **Add completion summary** at the end of the file:
   ```markdown
   ## Completion Summary
   **Completed on:** [Date]
   **Time spent:** [Estimate]
   **Changes made:**
   - Brief description of what was implemented
   - Key files modified or created
   - Any deviations from original plan
   
   **Testing notes:**
   - How the feature was tested
   - Any edge cases discovered
   - Performance considerations
   
   **Follow-up items:**
   - Any new issues discovered during implementation
   - Technical debt or improvements for later
   ```

## Priority Guidelines

- **High Priority** - Blocks other work or is critical for MVP
- **Medium Priority** - Important for complete feature set
- **Low Priority** - Nice to have or technical debt cleanup

## Labels for Organization

Common labels include:
- `mvp` - Required for minimum viable product
- `gameplay` - Core game mechanics
- `ui` - User interface and HUD
- `technical-debt` - Code quality improvements
- `testing` - Test setup or validation
- `data` - Configuration and assets
- `performance` - Optimization work

## Dependencies

Some issues depend on others being completed first. Always check:
1. Can I test this feature without other incomplete systems?
2. Do I need specific assets or data that don't exist yet?
3. Are there code dependencies in other assemblies?

When in doubt, start with High priority issues as they're typically foundational.

## Creating New Issues

When you discover new work that needs to be done:
1. Create a new issue file following the naming convention
2. Assign appropriate priority based on impact and urgency
3. Add relevant labels for easy filtering
4. Include clear acceptance criteria
5. Explain why the work matters for the overall project

Remember: Good issues are specific, testable, and provide clear value to the project.