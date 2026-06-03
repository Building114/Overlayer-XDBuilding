# Overlayer XDB Patch Changelog

## 3.42.2

Base:
- Based on Overlayer 3.42.0.

Fixes:
- Fixed a possible freeze when opening or saving JSON files through the file dialog on some systems.
- Added timeout handling for the PowerShell-based file dialog process.

Compatibility:
- Added compatibility helpers for ADOFAI r141+ field and method changes.
- Added soft compatibility tags for XPerfect.
- Updated true auto judgement patching path for r141+.

Changes:
- Disabled AutoUpdater in this unofficial patch to avoid replacing the patch build with official builds.
- Refactored text parser for safer parsing of tags, arguments, nested tags, quotes, and escape characters.

Notes:
- This is an unofficial temporary community patch.
- If an official fixed version is released, users should prefer the official release unless they specifically need this patch.