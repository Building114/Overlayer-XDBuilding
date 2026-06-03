# Third Party Notices

This project is based on Overlayer and keeps the upstream third-party notices.

## RapidGUI

RapidGUI is included in source form.

License:
- MIT License

## UnityCodeEditor

UnityCodeEditor-related code is included in source form.

Upstream notice:
- Unlicensed project using MIT-licensed code.

## Jint

Jint is used as a modified compiled DLL in the upstream Overlayer distribution.

License:
- BSD-2-Clause License

Notice:
- This repository does not claim ownership of Jint.
- If a binary release includes a modified Jint DLL, the release should clearly state where the corresponding source or upstream fork can be found.

## JipperResourcePack reference

The XDB patch notes mention JipperResourcePack as a public reference for changed ADOFAI field names.

No JipperResourcePack source code is bundled in this repository unless explicitly stated elsewhere.
If code is copied from that project in the future, its license must be checked and included here.

## NuGet dependencies

This project uses NuGet dependencies listed in `Overlayer/packages.config`.

Known dependencies include:

- ncalc 1.3.8
- System.Buffers
- System.IO.Compression.ZipFile
- System.Memory
- System.Numerics.Vectors
- System.Runtime.CompilerServices.Unsafe
- System.Threading.Tasks.Extensions

These packages are not committed into this repository.
They should be restored through NuGet when building the project.

For binary releases, the included dependency DLLs should keep their original license notices where required.