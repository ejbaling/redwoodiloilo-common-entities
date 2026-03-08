NuGet workflow — RedwoodIloilo.Common.Entities

Purpose

- Quick reference for updating consumer projects to a new `RedwoodIloilo.Common.Entities` NuGet version and verifying the assembly metadata.

Quick steps

1. Add or update the package reference

- From the repository root, to explicitly add or set a package version:

```powershell
dotnet add WebhookApi\WebhookApi.csproj package RedwoodIloilo.Common.Entities --version <version>
```

2. Clear caches and restore (force fresh resolution)

```powershell
# from repository root (where WebhookApi.sln lives)
dotnet nuget locals all --clear
dotnet build WebhookApi.sln -c Debug
```

3. Verify the installed package and DLL version (PowerShell)

```powershell
$d = Get-ChildItem "$env:USERPROFILE\.nuget\packages\redwoodiloilo.common.entities\<version>" -Filter 'RedwoodIloilo.Common.Entities.dll' -Recurse | Select-Object -First 1

(Get-Item $d.FullName).VersionInfo | Select-Object FileVersion, ProductVersion, FileName
```

Note: NuGet package version != AssemblyVersion. `FileVersion`/`ProductVersion` above reflect what OmniSharp shows in metadata.

4. If the editor/OmniSharp still shows the old assembly metadata

- In VS Code: open Command Palette (Ctrl+Shift+P) → `OmniSharp: Restart OmniSharp`.
- If needed: `Developer: Reload Window` or touch the `.csproj` file and restart the C# language server.
- As a last resort: delete `bin`/`obj` directories, re-run restore/build, then restart OmniSharp.

Commands to clean build artifacts:

```powershell
Get-ChildItem -Path . -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force
dotnet build WebhookApi.sln -c Debug
```

5. If `dotnet add package` or `dotnet restore` returns `NU1102` (package not found)

- Check your configured sources:

```powershell
dotnet nuget list source
```

- Confirm the version is published on the feed (nuget.org flat container index):

```powershell
(Invoke-RestMethod 'https://api.nuget.org/v3-flatcontainer/redwoodiloilo.common.entities/index.json').versions
```

- If propagation is delayed, either wait or test with a local feed:

```powershell
# create a local folder feed
mkdir C:\local-nuget
# copy/paste the .nupkg into C:\local-nuget
dotnet nuget add source C:\local-nuget -n LocalFeed
# restore from LocalFeed
dotnet restore --source LocalFeed
```

6. If you maintain the library and need to publish

- Update library `Version`, `AssemblyVersion` and `FileVersion` where appropriate in the library project.

- Recommended workflow (run these commands from the library project directory: `d:\src\Entities\RedwoodIloilo.Common.Entities`)

```powershell
# change to the project directory
Set-Location d:\src\Entities\RedwoodIloilo.Common.Entities
dotnet clean
dotnet build -c Release
Get-Item .\bin\Release\net8.0\RedwoodIloilo.Common.Entities.dll | Select-Object @{n='FileVersion';e={$_.VersionInfo.FileVersion}}, @{n='ProductVersion';e={$_.VersionInfo.ProductVersion}}

# pack into a local folder for easy discovery
dotnet pack -c Release -o .\nupkgs

# Push to nuget.org using an API key stored in an environment variable
# (safer than pasting the key on the command line)
$env:NUGET_API_KEY = '<YOUR_API_KEY>'
dotnet nuget push .\nupkgs\*.nupkg -s https://api.nuget.org/v3/index.json -k $env:NUGET_API_KEY
```

Notes

- Do NOT commit API keys to source control. Use environment variables or CI secrets.
- If the package Id+Version already exist on nuget.org, increment the `<Version>` in the `.csproj` and repack.
- To publish from CI (GitHub Actions), store the API key as a repository secret and use `dotnet nuget push` in the workflow.

7. Quick troubleshooting checklist

- Confirm `%USERPROFILE%\.nuget\packages\redwoodiloilo.common.entities\<version>` exists and contains the DLL.
- If assembly metadata still shows an older `AssemblyVersion`, the library may not have bumped `AssemblyVersion` — republish with updated assembly attributes.
- If OmniSharp shows stale metadata after a successful restore, restart OmniSharp / reload VS Code.

Need me to commit `WebhookApi.csproj` version changes or add instructions to the repo README instead?
