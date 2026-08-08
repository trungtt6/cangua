using UnityEngine;
using System.Collections;

public class AutoMatchManager : Photon.PunBehaviour
{
    public float retryInterval = 2.0f;
    // time to wait for other players to join before filling with bots
    public float matchTimeout = 30.0f;
    public bool AutoMatching { get; private set; }

    private Coroutine loopCoroutine;
    private Coroutine fillCoroutine;

    // Photon provider abstraction to allow injection for testing
    public IPhotonProvider PhotonProvider;

    void Awake()
    {
        // Defer assigning the default provider to Awake to avoid referencing instance members in field initializers.
        if (this.PhotonProvider == null)
        {
            this.PhotonProvider = global::PhotonProvider.Default;
        }
    }

    public void StartAutoMatch()
    {
        if (AutoMatching) return;
        AutoMatching = true;
        if (loopCoroutine != null) StopCoroutine(loopCoroutine);
        loopCoroutine = StartCoroutine(AutoMatchLoop());
        Debug.Log("AutoMatch: started");
    }

    public void StopAutoMatch()
    {
        if (!AutoMatching) return;
        AutoMatching = false;
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }
        Debug.Log("AutoMatch: stopped");
    }

    private IEnumerator AutoMatchLoop()
    {
        while (AutoMatching)
        {
            if (!PhotonProvider.InRoom)
            {
                var pf = GameObject.FindObjectOfType<PlayFabManager>();
                if (pf != null)
                {
                    pf.JoinRoomAndStartGame();
                }
            }
            yield return new WaitForSeconds(retryInterval);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("AutoMatch: joined room: " + PhotonProvider.RoomName + " players:" + PhotonProvider.PlayerCount);
        // start waiting for players to join; if not enough after `matchTimeout`, fill with bots (master client only)
        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);
        fillCoroutine = StartCoroutine(WaitAndFillWithBots());
        // If room already full, stop auto-matching for this client
        if (PhotonProvider.PlayerCount >= GameManager.Instance.requiredPlayers)
        {
            StopAutoMatch();
        }
    }

    private IEnumerator WaitAndFillWithBots()
    {
        float timer = 0f;
        while (timer < matchTimeout)
        {
            if (!PhotonProvider.InRoom)
                yield break; // left room
            if (PhotonProvider.PlayerCount >= GameManager.Instance.requiredPlayers)
                yield break; // enough players joined
            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("AutoMatch: timeout reached, checking to fill with bots. Players:" + (PhotonNetwork.room != null ? PhotonNetwork.room.PlayerCount.ToString() : "-"));

        if (PhotonProvider.InRoom && PhotonProvider.IsMasterClient)
        {
            // Request PlayFabManager to add bots / start game with bots
            if (GameManager.Instance != null && GameManager.Instance.playfabManager != null)
            {
                Debug.Log("AutoMatch: master adding bots to fill up to " + GameManager.Instance.requiredPlayers);
                GameManager.Instance.playfabManager.StartGameWithBots();
            }
            else
            {
                Debug.LogWarning("AutoMatch: PlayFabManager not available to add bots");
            }
        }
        // stop auto-matching for this client after filling
        StopAutoMatch();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("AutoMatch: left room");
        // cancel pending fill coroutine
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }
        // If auto matching still enabled, the loop will try again
    }
}
