Headless Multi-build Integration Runner

This harness builds the Unity project and launches multiple standalone clients to run automated integration tests using real Photon.

Prerequisites:
- Unity Editor installed on the build machine and accessible by path.
- Photon SDK configured in project and Photon cloud AppID set in `StaticStrings.PhotonAppID`.
- PlayFab configuration (if required) set up for real accounts, or the project must permit anonymous/guest login.

Files added:
- `Assets/Editor/IntegrationBuild.cs` — Editor script to perform a standalone Windows build via Unity CLI.
- `Assets/Ludo Masters/Scripts/AutoIntegrationRunner.cs` — runtime component that detects `-auto_integration` command-line flag and runs the join/test flow.
- `tools/build_and_run_integration.ps1` — PowerShell script to build and launch N clients, collect logs, and enforce a timeout.

Usage (Windows CI agent):
1. Ensure build agent has Unity installed and available at `unityPath` in the PowerShell script. Edit `build_and_run_integration.ps1` parameters at top or supply them when invoking the script.

2. From PowerShell (in admin or user shell):

```powershell
# Example invocation (adjust Unity path and project path)
.
\tools\build_and_run_integration.ps1 -unityPath "C:\Program Files\Unity\Hub\Editor\2021.3.0f1\Editor\Unity.exe" -projectPath "C:\deps\cangua\Ludo Online Chupamobile" -clients 4 -testTimeout 240
```

3. The script will:
- Build the project via `IntegrationBuild.PerformIntegrationBuild` (outputs `LudoIntegration.exe` to your Desktop folder by default).
- Launch the specified number of clients, each with `-auto_integration -clientId <id> -playerName <name>` args.
- Wait for processes to finish or until the timeout elapses. It then kills any remaining processes and prints the tail of each log.

Notes and caveats:
- The runtime `AutoIntegrationRunner` expects `PlayFabManager` to be present in the initial scene and will call `JoinRoomAndStartGame()`; real Photon network connectivity and PlayFab login must succeed for the test to proceed.
- For CI, ensure firewall/networking allows outbound to Photon Cloud and PlayFab.
- You can modify the build output path in `IntegrationBuild.PerformIntegrationBuild` or pass environment-specific parameters in a CI job.
- For Linux headless server builds, adjust `BuildTarget` and build options in `IntegrationBuild.cs` and adapt `build_and_run_integration.ps1` to launch the Linux executable.

If you want, I can:
- Add Windows/Linux build targets to `IntegrationBuild` and accept arguments for output path.
- Extend the PowerShell script to upload logs/artifacts to your CI storage (Azure DevOps, GitHub Actions, etc.).
- Add a small report parser that detects test success/failure from client logs and returns a non-zero exit code on failures.

Which enhancements should I add next?