using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerUIControl : MonoBehaviourPun
{
    [SerializeField] GameObject playerUI;
    [SerializeField] TextMeshPro nameText;

    private Camera mainCam;
    private bool isName = false;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        SetName(photonView.Owner.NickName);
    }

    private void Update()
    {
        if (isName)
            playerUI.transform.forward = -mainCam.transform.forward;
    }

    private void SetName(string name)
    {
        nameText.text = name;
        // namePanel.size = nameText.GetPreferredValues() + (Vector2.one * 0.15f);
        isName = true;
    }



}
