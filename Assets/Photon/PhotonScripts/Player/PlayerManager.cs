using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Collections;
using Cinemachine;
using System.Collections.Generic;

namespace Photon_NetWork
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static GameObject LocalPlayerInstance;
        #region  Private Fields
        [SerializeField] private Transform bulletPos;
        [SerializeField] private Bullet bullet;
        [SerializeField] public GameObject PlayerUiPrefab;
        [SerializeField] private CinemachineVirtualCamera cam;
        private PlayerAnimatorManager animatorManager;
        #endregion

        #region  Public Fields
        public bool IsFiring { get; set; }
        public float Health = 1;
        public int UserId { get; set; }
        #endregion

        #region  MonoBehaviour CallBacks
        void Awake()
        {
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
                cam.Priority = 100;
            }
            else
                cam.Priority = 10;

            animatorManager = GetComponent<PlayerAnimatorManager>();
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            if (PlayerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(PlayerUiPrefab);
                _uiGo.GetComponent<PlayerUI>().SetTarget(this);
                Debug.Log("Own ActorNumber : " + photonView.Owner.ActorNumber);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
            UserId = photonView.Owner.ActorNumber;
#if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }

#if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
#endif
        private bool leavingRoom;
        public void Update()
        {
            // we only process Inputs and check health if we are the local player
            if (photonView.IsMine)
            {
                if (Health <= 0f && !leavingRoom)
                {
                    leavingRoom = PhotonNetwork.LeaveRoom();
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    animatorManager.SetAniState(PlayerAniState.GunRight);
                    CreateBullet();
                    photonView.RPC("SendChat", RpcTarget.All, "Fire");
                    // photonView.RPC("SendChat", RpcTarget.OthersBuffered, "Fire");
                }
            }
        }
        // Next
        private void CreateBullet()
        {
            Bullet b = PhotonNetwork.Instantiate("Bullet", bulletPos.position, bulletPos.rotation).GetComponent<Bullet>();
            b.FireBullet();
            animatorManager.SetAniState(PlayerAniState.GunRight);
        }
        [PunRPC]
        void SendChat(string str, PhotonMessageInfo info)
        {
            Debug.Log("Chat : " + str);
            Debug.LogFormat("info : {0}\n{1}\n{2}", info.Sender.ActorNumber, info.photonView, info.SentServerTime);
        }
        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                animatorManager.enabled = false;
                this.transform.position = new Vector3(0, 0, 10);
                animatorManager.enabled = true;
            }
        }
        public override void OnLeftRoom()
        {
            this.leavingRoom = false;
        }
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Bullet : " + other);

            if (!photonView.IsMine)
                return;
            if (!other.gameObject.name.Contains("Bullet"))
                return;

            Health -= 0.1f;
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Bullet : " + other);

            if (!photonView.IsMine)
                return;
            if (!other.name.Contains("Bullet"))
                return;
            Health -= 0.1f;
        }
        private void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
                return;
            if (!other.name.Contains("Bullet"))
                return;
            Health -= 0.1f * Time.deltaTime;
        }
        #endregion
#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif
#if !UNITY_5_4_OR_NEWER
/// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
void OnLevelWasLoaded(int level)
{
    this.CalledOnLevelWasLoaded(level);
}
#endif

        void CalledOnLevelWasLoaded(int level)
        {
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 1f, 0f);
            }
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            // _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            _uiGo.GetComponent<PlayerUI>().SetTarget(this);
        }

        #region IPunObservable implementation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else
            {
                IsFiring = (bool)stream.ReceiveNext();
                Health = (float)stream.ReceiveNext();
            }
        }
        #endregion
    }
}