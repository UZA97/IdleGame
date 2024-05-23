using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Test_Pun2
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] GameObject[] playerPrefabs;

        private void Start()
        {
            CreateRandomCharacter();
        }

        private void CreateRandomCharacter()
        {
            if (PlayerManager.localPlayerInstance != null && photonView.IsMine == false) return;

            int ran = Random.Range(0, playerPrefabs.Length);
            PlayerManager.localPlayerInstance = PhotonNetwork.Instantiate(playerPrefabs[ran].name, Vector3.zero, Quaternion.identity);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Launcher");
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"<color=red>Is Master Client XX</color>");
                return;
            }

            Debug.Log($"<color=green>Load Level {PhotonNetwork.CurrentRoom.PlayerCount}");
            // PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"<color=green>PlayerEnterRoom={newPlayer.NickName}</color>");

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"<color=green>PlayerEnterRoom IsMasterClient={PhotonNetwork.IsMasterClient}</color>");
                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"<color=green>PlayerLeftRoom={otherPlayer.NickName}</color>");

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"<color=green>PlayerLeftRoom IsMasterClient={PhotonNetwork.IsMasterClient}</color>");
                LoadArena();
            }
        }

    }
}