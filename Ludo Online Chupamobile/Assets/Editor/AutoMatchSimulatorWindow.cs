#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class AutoMatchSimulatorWindow : EditorWindow
{
    private MockPhotonProvider mock;
    private AutoMatchManager amm;
    private float simulationTime = 0f;
    private int simulatedPlayers = 1;
    private int targetPlayers = 4;
    private bool running = false;
    private float addPlayerInterval = 5f;

    [MenuItem("Tools/AutoMatch Simulator")]
    public static void ShowWindow()
    {
        GetWindow<AutoMatchSimulatorWindow>("AutoMatch Simulator");
    }

    void OnGUI()
    {
        GUILayout.Label("AutoMatch Simulator", EditorStyles.boldLabel);
        targetPlayers = EditorGUILayout.IntField("Target players:", targetPlayers);
        addPlayerInterval = EditorGUILayout.FloatField("Add player interval (s):", addPlayerInterval);

        if (!running)
        {
            if (GUILayout.Button("Start Simulation"))
            {
                StartSimulation();
            }
        }
        else
        {
            if (GUILayout.Button("Stop Simulation"))
            {
                StopSimulation();
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("Simulation state:");
        GUILayout.Label("Running: " + running);
        GUILayout.Label("Simulated players: " + (mock != null ? mock.PlayerCount.ToString() : "-"));
        GUILayout.Label("Simulation time: " + simulationTime.ToString("0.0"));
    }

    void StartSimulation()
    {
        running = true;
        // create mock
        mock = new MockPhotonProvider();
        mock.InRoom = false;
        mock.PlayerCount = 1; // local player
        mock.IsMasterClient = true;
        mock.RoomName = "SimRoom";

        // inject mock
        PhotonProvider.Instance = mock;

        // create a GameObject with AutoMatchManager
        GameObject go = new GameObject("AutoMatchSimulator");
        amm = go.AddComponent<AutoMatchManager>();
        amm.PhotonProvider = mock;
        amm.matchTimeout = StaticStrings.DefaultAutoMatchTimeout;
        amm.retryInterval = 1.0f;

        // start auto-match
        amm.StartAutoMatch();

        // start editor update
        EditorApplication.update += EditorUpdate;
        simulationTime = 0f;
        simulatedPlayers = 1;
    }

    void StopSimulation()
    {
        running = false;
        if (amm != null)
        {
            amm.StopAutoMatch();
            if (Application.isPlaying == false)
            {
                DestroyImmediate(amm.gameObject);
            }
            else
            {
                Destroy(amm.gameObject);
            }
            amm = null;
        }
        if (mock != null)
        {
            PhotonProvider.Instance = PhotonProvider.Default;
            mock = null;
        }
        EditorApplication.update -= EditorUpdate;
    }

    void EditorUpdate()
    {
        if (!running) return;
        simulationTime += (float)EditorApplication.timeSinceStartup - simulationTime;
        // crude timing: add players every addPlayerInterval seconds
        if (simulationTime > 0 && (int)(simulationTime / addPlayerInterval) + 1 > simulatedPlayers && mock != null)
        {
            simulatedPlayers++;
            mock.PlayerCount = Mathf.Min(simulatedPlayers, targetPlayers);
            mock.InRoom = true;
            Debug.Log("[Simulator] Player joined. Count=" + mock.PlayerCount);
            // if joined reaches target, simulate OnJoinedRoom
            if (mock.PlayerCount >= targetPlayers)
            {
                // nothing extra: AutoMatchManager has logic to stop
            }
        }

        // After match timeout + some, if bots were requested, StopSimulation will be triggered by AutoMatchManager stopping auto-match
        if (amm != null && !amm.AutoMatching)
        {
            Debug.Log("[Simulator] AutoMatch finished; stopping simulation.");
            StopSimulation();
        }
    }
}
#endif
