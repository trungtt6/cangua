using UnityEngine;
using System.Collections;

public class AutoIntegrationRunner : MonoBehaviour
{
    private bool isIntegration = false;
    private string clientId = "";
    private string playerName = "";

    IEnumerator Start()
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-auto_integration" || args[i] == "-autoIntegration")
            {
                isIntegration = true;
            }
            if (args[i] == "-clientId" && i + 1 < args.Length)
            {
                clientId = args[i + 1];
            }
            if (args[i] == "-playerName" && i + 1 < args.Length)
            {
                playerName = args[i + 1];
            }
        }

        if (!isIntegration)
            yield break;

        Debug.Log("AutoIntegrationRunner: running integration flow. clientId=" + clientId + " playerName=" + playerName);

        // apply player name to GameManager early
        if (!string.IsNullOrEmpty(playerName))
        {
            GameManager.Instance.nameMy = playerName;
        }

        // Ensure PlayFabManager exists in scene; wait up to 20s
        PlayFabManager pf = null;
        float wait = 0f;
        while (pf == null && wait < 20f)
        {
            var go = GameObject.Find("PlayFabManager");
            if (go != null)
                pf = go.GetComponent<PlayFabManager>();
            if (pf == null)
            {
                yield return new WaitForSeconds(0.5f);
                wait += 0.5f;
            }
        }

        if (pf == null)
        {
            Debug.LogError("AutoIntegrationRunner: PlayFabManager not found in scene.");
            yield break;
        }

        // Optionally set Photon player nick
        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.player.NickName = playerName;
        }

        // Start the join flow (this will wait internally until connected/joined lobby)
        pf.JoinRoomAndStartGame();

        // Wait for game scene to start or timeout
        float timeout = 120f;
        float t = 0f;
        while (!GameManager.Instance.gameSceneStarted && t < timeout)
        {
            yield return new WaitForSeconds(0.5f);
            t += 0.5f;
        }

        if (GameManager.Instance.gameSceneStarted)
        {
            Debug.Log("AutoIntegrationRunner: game scene started; test succeeded for client " + clientId);
            // Emit explicit token for log parser
            Debug.Log("[TEST_TOKEN] gameSceneStarted");
        }
        else
        {
            Debug.LogError("AutoIntegrationRunner: timeout waiting for game scene to start for client " + clientId);
            Debug.Log("[TEST_TOKEN] gameSceneTimeout");
        }

        // Exit application
#if UNITY_EDITOR
        Debug.Log("AutoIntegrationRunner: stopping playmode (editor)");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Debug.Log("AutoIntegrationRunner: quitting application");
        Application.Quit();
#endif
    }
}
