using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Test_Pun2
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private const string GAME_VERSION = "1";
        private const string PLAYER_NAME_PREF_KEY = "PlayerName";
        private const string GAME_SCENE = "MainScene";

        [SerializeField] byte maxPlayerPerRoom = 4;
        [SerializeField] GameObject[] uiPanels;
        [SerializeField] TMP_InputField playerNameField;

        private bool isConneting = false;

        private void Awake()
        {
            Application.targetFrameRate = 30;
            // 마스터 클라이언트에서 LoadLevel을 사용하게하며 같은 룸에 있는 클라이언트가 자동으로 수준 동기화
            // PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.AutomaticallySyncScene = false;
        }

        private void Start()
        {
            OnChangePanel(0);
            InitNameField();
        }

        #region Network_Connect
        public void Connect()
        {
            isConneting = true;
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();             // 네트워크 접속
                Debug.Log($"<color=red>NetworkJoin</color>");
            }
            else
            {
                PhotonNetwork.GameVersion = GAME_VERSION;    // 네트워크 버전
                Debug.Log($"<color=green>NetworkConnect</color>");
            }

            OnChangePanel(1);
            SetNickName(playerNameField.text);
            PhotonNetwork.ConnectUsingSettings();       // 네트워크 연결

        }

        public override void OnConnectedToMaster()
        {
            if (isConneting)
            {
                PhotonNetwork.JoinRandomRoom();             // 네트워크 접속
                Debug.Log($"<color=green>NetworkJoin</color>");
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            OnChangePanel(0);
            Debug.Log($"<color=red>OnDisconnected</color>");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom("TEST",
            new RoomOptions
            {
                MaxPlayers = maxPlayerPerRoom
            });
            Debug.Log($"<color=red>OnJoinRandomFailed</color>");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"<color=green>OnJoinedRoom</color>");
            // if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            // {
            PhotonNetwork.LoadLevel(GAME_SCENE);
            // PhotonNetwork.LoadLevel("Room for 1");
            // }
        }

        private void OnChangePanel(int idx)
        {
            bool isActive = false;

            for (int i = 0; i < uiPanels.Length; i++)
            {
                isActive = i == idx ? true : false;
                uiPanels[i].SetActive(isActive);
            }
        }
        #endregion

        #region NickName
        private void InitNameField()
        {
            string myName = string.Empty;
            if (PlayerPrefs.HasKey(PLAYER_NAME_PREF_KEY))
            {
                myName = PlayerPrefs.GetString(PLAYER_NAME_PREF_KEY);
                playerNameField.text = myName;
            }

            // PhotonNetwork.NickName = myName;
        }

        private void SetNickName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.Log($"<color=red>Null Player Name</color>");
                return;
            }

            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(PLAYER_NAME_PREF_KEY, value);

            Debug.Log($"<color=green>SetNickName [{value}]</color>");
        }
        #endregion
    }
}

