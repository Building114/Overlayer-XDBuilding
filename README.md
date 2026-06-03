# Overlayer XDB Patch

Unofficial temporary community patch for Overlayer 3.42.0.

This is not an official release from modlist.org.

## Source

Original project: modlist-org/Overlayer  
Base version: 3.42.0  
Patch version: 3.42.2  
License: GPL-3.0

## Notes

This patch keeps Overlayer usable before an official fixed version or successor version is available.

See `CHANGELOG-XDB.md` for patch details.

## Build

This repository contains the Overlayer patch source.

Before building:

1. Open `Overlayer/Overlayer.sln`.
2. Restore NuGet packages.
3. Make sure A Dance of Fire and Ice is installed.
4. Update `GameDir` in `Overlayer/Overlayer.csproj` if your game path is different.
5. Build the project in Release mode.

The project currently references game DLLs from the local ADOFAI installation through `GameDir`.


This repository focuses on the patched Overlayer mod source.
It may not include every helper project or script from the original archived upstream repository.