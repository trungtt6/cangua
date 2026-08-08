using System;
using UnityEngine;

public interface IPhotonProvider
{
    bool InRoom { get; }
    int PlayerCount { get; }
    string RoomName { get; }
    bool IsMasterClient { get; }
}

public class PhotonProvider : IPhotonProvider
{
    public static IPhotonProvider Default = new PhotonProvider();

    // Allow replacing for tests
    public static IPhotonProvider Instance
    {
        get { return Default; }
        set { Default = value; }
    }

    // For convenience in other scripts
    public static IPhotonProvider DefaultProvider => Default;

    public virtual bool InRoom { get { return PhotonNetwork.inRoom; } }
    public virtual int PlayerCount { get { return PhotonNetwork.room != null ? PhotonNetwork.room.PlayerCount : 0; } }
    public virtual string RoomName { get { return PhotonNetwork.room != null ? PhotonNetwork.room.Name : ""; } }
    public virtual bool IsMasterClient { get { return PhotonNetwork.isMasterClient; } }
}

// Simple mock provider for Editor simulation
public class MockPhotonProvider : IPhotonProvider
{
    public bool InRoom { get; set; }
    public int PlayerCount { get; set; }
    public string RoomName { get; set; }
    public bool IsMasterClient { get; set; }

    public MockPhotonProvider()
    {
        InRoom = false;
        PlayerCount = 0;
        RoomName = "MockRoom";
        IsMasterClient = true;
    }
}
