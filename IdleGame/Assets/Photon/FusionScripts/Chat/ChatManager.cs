using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;


public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;

    public Button chatButton;
    public Button closeButton;
    public Button sendButton;
    public GameObject chatGroup;
    public GameObject noticeImage;
    public Chat chatScroll;
    public TMP_InputField myInputField;
    public NetworkObject networkObject;
    // public NetworkObject networkObject { get; private set; }
    bool isSend = false;
    public void SetNetWorkObject(NetworkObject _obj)
    {
        networkObject = _obj;
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
        chatButton.onClick.AddListener(OnClick_ChatButton);
        closeButton.onClick.AddListener(OnClick_CloseButton);
        sendButton.onClick.AddListener(SendButton_Click);
    }
    // public override void Spawned()
    // {
    //     base.Spawned();
    //     if (HasStateAuthority)
    // }
    // public override void FixedUpdateNetwork()
    // {
    //     if (Input.GetKeyDown(KeyCode.R))
    //         if (HasStateAuthority && Input.GetKeyDown(KeyCode.R))
    //         {
    //             SendButton_ClickRpc(myInputField.text);
    //         }
    // }
    public void OnClick_ChatButton()
    {
        chatGroup.SetActive(true);
        myInputField.ActivateInputField();

        chatScroll.Init();
        chatButton.gameObject.SetActive(false);
    }
    public void OnClick_CloseButton()
    {
        chatGroup.SetActive(false);
        chatButton.gameObject.SetActive(true);
    }
    public void SendButton_Click()
    {
        if (!string.IsNullOrEmpty(myInputField.text))
            networkObject.GetComponent<PlayerChat>().SendButton_ClickRpc(myInputField.text);
    }
    public void SendOtherChat_UI(bool isMine, string str)
    {
        if (isMine)
        {
            chatScroll.AddNewRow(Data.CellType.MyText, myInputField.text);
            myInputField.text = "";
            myInputField.ActivateInputField();
        }
        else
        {
            if (chatScroll.gameObject.activeSelf == false)
                noticeImage.SetActive(true);
            else
            {
                chatScroll.AddNewRow(Data.CellType.OtherText, str);
                noticeImage.SetActive(false);
            }
        }
    }
}
