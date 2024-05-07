using DG.Tweening;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// public class PlayerChat : NetworkBehaviour
// {
//     public TextMeshPro chatStr;
//     public SpriteRenderer chatBg;
//     public TMP_InputField myInputField;
//     public Button sendButton;
//     public float width = 1.5f;
//     [Networked, OnChangedRender(nameof(ChatChanged))]
//     public string NetworkedChat { get; set; }
//     private void Start()
//     {
//         myInputField = GameObject.Find("Canvas").transform
//         .Find("My Text Window").Find("My InputField").GetComponent<TMP_InputField>();
//         sendButton = GameObject.Find("Canvas").transform
//         .Find("My Text Window").Find("My Send Button").GetComponent<Button>();
//         sendButton?.onClick.AddListener(ChatChanged);
//     }
//     private void Update()
//     {
//         chatStr.transform.forward = Camera.main.transform.forward;
//         chatBg.size = new Vector2(chatStr.preferredWidth * width, chatBg.size.y);
//         if (HasStateAuthority)
//             NetworkedChat = myInputField.text;

//     }
//     void ChatChanged()
//     {
//         chatStr.text = NetworkedChat;
//     }
// }
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
        chatBg.transform.forward = -cam.transform.forward;
        chatBg.size = new Vector2(chatStr.preferredWidth * width, chatBg.size.y);

    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void SendButton_ClickRpc(string str)
    {
        ChatString = str;
    }
    Sequence sequence;
    void SendOtherChat()
    {
        ChatManager.instance.SendOtherChat_UI(HasStateAuthority, ChatString);
        if (HasStateAuthority)
        {
            // chatBg.transform.DOScale(1,1).;  
            chatStr.text = ChatString;
        }
    }
}
