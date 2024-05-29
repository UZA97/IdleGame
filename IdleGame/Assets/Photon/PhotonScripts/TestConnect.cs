using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("Connecting . . .");
        PhotonNetwork.NickName = MasterManager.Instance.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.Instance.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server!!!!!!");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("DisConnected to server : " + cause.ToString());
    }
}
