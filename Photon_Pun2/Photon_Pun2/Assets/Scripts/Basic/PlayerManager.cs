using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject localPlayerInstance;
    private bool IsFiring = false;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.localPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // #if UNITY_5_4_OR_NEWER
        //         // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        //         UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
        //                 {
        //                     this.CalledOnLevelWasLoaded(scene.buildIndex);
        //                 };
        // #endif
    }

#if !UNITY_5_4_OR_NEWER
/// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
void OnLevelWasLoaded(int level)
{
    this.CalledOnLevelWasLoaded(level);
}
#endif

    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(IsFiring);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
        }
    }
}
