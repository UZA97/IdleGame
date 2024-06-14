using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;
/*
[로비 접속시도]
PhotonNetwork.ConnectUsingSettings() -> (OnConnectedToMaster	/	OnDisconnected)

[방 접속]
PhotonNetwork.JoinRandomRoom(); -> OnJoinedRoom -> OnJoinRandomFailed -> CreateRoom -> OnDisconnected
*/
namespace Photon_NetWork
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable  Fields
        [SerializeField] private byte maxPlayersPerRoom = 4;
        [SerializeField] private Button startButton;
        [SerializeField] GameObject controlPanel;
        [SerializeField] GameObject progressLabel;
        #endregion

        #region  Private Fields
        private string gameVersion = "1";
        private bool isConnecting;
        #endregion

        #region  MonoBehaviour CalBacks
        private void Awake()
        {
            // # 같은 방에 있는 모든 클라이언트가 자동으로 레벨을 동기화
            PhotonNetwork.AutomaticallySyncScene = true;
            SetButton();
        }

        private void Start()
        {
            ControlProgressObject(isProgress: true);
        }
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("Called by PUN with reason {0}", cause);
            ControlProgressObject(true);
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Called by PUN. No random room available, so we create one");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("Join Room");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Load the Room for 1");
                PhotonNetwork.LoadLevel("Room for 1");
            }
        }

        #endregion

        #region Public Methods
        public void Connect()
        {
            ControlProgressObject(false);
            if (PhotonNetwork.IsConnected)
            {
                // #방이 없으면 OnJoinRandomFailed() Callback 후 하나 새로 만듦.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Photon 서버에 접속.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        #endregion

        #region  Private Methods
        private void ControlProgressObject(bool isProgress)
        {
            progressLabel.SetActive(!isProgress);
            controlPanel.SetActive(isProgress);
        }
        public void SetButton()
        {
            startButton.onClick.AddListener(Connect);
        }
        #endregion
    }
}