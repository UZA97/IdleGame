using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI roomName;
    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        if (roomName.text.IsNullOrEmpty())
            return;
        RoomOptions option = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(roomName.text, option, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Success CreateRoom");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed CreateRoom");
    }
}
