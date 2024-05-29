using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerChatManager : MonoBehaviourPun
{
    [PunRPC]
    public void SendChat(string text)
    {

    }
}
