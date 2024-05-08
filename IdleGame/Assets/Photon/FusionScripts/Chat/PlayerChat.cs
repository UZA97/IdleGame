using DG.Tweening;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChat : NetworkBehaviour
{
    public TextMeshPro chatStr;
    public SpriteRenderer chatBg;
    public float width = 1.25f;
    [Networked, OnChangedRender(nameof(SendOtherChat))]
    public string ChatString { get; set; }
    Camera cam;
    void Awake()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        chatBg.transform.forward = cam.transform.forward;
        chatBg.size = new Vector2(chatStr.preferredWidth * width, chatStr.preferredHeight * width);

    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void SendButton_ClickRpc(string str)
    {
        if (HasStateAuthority)
            ChatString = str;
    }
    Sequence sequence;
    void SendOtherChat()
    {
        ChatManager.instance.SendOtherChat_UI(HasStateAuthority, ChatString);
        //DoTween Sequence
        chatStr.text = ChatString;
        Sequence seq = DOTween.Sequence();
        seq.Append(chatBg.transform.DOScale(1, 1));  // animation 바로 실행  
        seq.AppendInterval(5.0f); // 1초를 기다림
        seq.Append(chatBg.transform.DOScale(0, 0.5f));  // animation 바로 실행  
        seq.Play(); // Sequence 실행 
    }
}
