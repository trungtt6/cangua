Auto-match Feature: Test Plan

Purpose:
Verify auto-matching, 4-player support, 30s timeout, and auto-fill with bots.

Prerequisites:
- Unity project opened in Unity Editor (same version project was created with).
- PlayFab and Photon settings configured in `PlayFabManager` and `PhotonServerSettings`.
- Scenes: MenuScene with PlayFabManager and UI elements referenced.

Manual Test Steps:

1. Verify AutoMatch UI
- Open the Settings UI scene or the Menu where `SettingsWindowController` exists.
- Confirm a new toggle `AutoMatchToggle` and an input `AutoMatchTimeoutInput` are present and wired.
- Toggle auto-match on/off and set timeout (e.g., 30). Close settings.

2. Start Auto-Matching (single device)
- From main menu, press "Start Random Game" to trigger `FacebookManager.startRandomGame()`.
- Console: expect "AutoMatch: started".
- If no existing room, a new room should be created by `PlayFabManager.OnPhotonRandomJoinFailed` with `MaxPlayers` set to required players.

3. Wait for auto-match timeout
- If fewer players join within `AutoMatchTimeout` seconds (default 30), master client should log "AutoMatch: timeout reached, checking to fill with bots..." followed by "master adding bots...".
- Verify bots are added by checking game starts and opponent avatars/names include generated bot entries.

4. Multi-device test (recommended)
- Run 4 devices/instances (Editor + 3 mobile builds or multiple Editor instances using Build and Run with different players). Ensure devices connect to Photon cloud region configured.
- Start random match on each device; they should join the same room and start the game when all players connected.

5. Disable AutoMatch while running
- While auto-matching loop is running, open Settings UI and disable `AutoMatchToggle`.
- Console: expect "AutoMatch: stopped".
- Ensure the loop stops attempting further matches.

6. Edge cases
- If the master client disconnects while waiting, ensure `OnMasterClientSwitched` logic in `PlayFabManager` handles the new master and bots do not get duplicated.
- Verify `matchTimeout` respects minimum (5s enforced in UI handler).

Notes for QA/Automation:
- Running multiple Unity Editor clients on one machine requires separate builds or using the Unity Multiprocess play modes or Photon local server.
- For automated unit tests, consider mocking Photon callbacks or using Photon Realtime SDK test tools.

Files changed:
- `Assets/Ludo Masters/Scripts/AutoMatchManager.cs` (new)
- `Assets/Ludo Masters/Scripts/PlayFabManager.cs` (modified)
- `Assets/Ludo Masters/Scripts/FacebookManager.cs` (modified)
- `Assets/Ludo Masters/Scripts/Game/StaticStrings.cs` (modified)
- `Assets/Ludo Masters/Scripts/SettingsWindowController.cs` (modified)

If you want, I can now:
- Add UI prefabs and wire the new toggle+input directly into the existing settings GameObject (requires scene editing), or
- Create an Editor test harness to launch multiple headless player builds for automated integration tests.

Which would you like next?

Simulator guide:

- Open Unity Editor.
- Window -> Tools -> AutoMatch Simulator.
- Set `Target players` to 4 and `Add player interval` (seconds) to control when simulated other players join.
- Click `Start Simulation` — the simulator will inject a mock Photon provider and run the `AutoMatchManager` loop.
- Observe the Console for messages: `AutoMatch: started`, player joins, timeout and bot-fill logs.

Note: This simulator runs in-editor and mocks Photon behavior. It doesn't use the real Photon Cloud — it's suitable for validating local auto-match logic and bot-fill behavior.