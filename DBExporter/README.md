## Build Instruction
```sh
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:SelfContained=true /p:IncludeAllContentForSelfExtract=true /p:DebugType=None
```

The `dotnet publish` command allows several `/p:` (MSBuild properties) options to customize the build and deployment process. Here are some commonly used ones:

### General Publish Options
- `/p:PublishSingleFile=true` – Produces a single-file executable.
- `/p:SelfContained=true` – Publishes the app with the .NET runtime included.
- `/p:RuntimeIdentifier=win-x64` – Specifies the target runtime (e.g., `win-x64`, `linux-x64`, `osx-arm64`).
- `/p:Configuration=Release` – Sets the build configuration (e.g., `Release`, `Debug`).

### Optimization & Debugging
- `/p:IncludeAllContentForSelfExtract=true` – Embeds all dependencies inside the executable.
- `/p:DebugType=None` – Removes debugging symbols to reduce file size.
- `/p:Optimize=true` – Enables optimizations for performance.
- `/p:PublishTrimmed=true` – Trims unused parts of the .NET runtime to reduce executable size.
- `/p:PublishReadyToRun=true` – Precompiles code to improve startup performance.

### Native & Compatibility
- `/p:IncludeNativeLibrariesForSelfExtract=true` – Ensures native libraries are extracted at runtime.
- `/p:EnableCompressionInSingleFile=true` – Compresses bundled dependencies inside the executable.
- `/p:AppHost=true` – Generates a platform-specific executable.
- `/p:UseAppHost=false` – Disables the platform-specific executable (useful for trimming scenarios).

### Output Customization
- `/p:PublishDir=<PATH>` – Defines the output directory for published files.
- `/p:AssemblyName=MyApp` – Sets a custom name for the output assembly.
- `/p:Version=1.0.0` – Specifies the application version.
