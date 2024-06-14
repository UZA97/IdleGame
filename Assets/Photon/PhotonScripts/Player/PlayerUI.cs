using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using TMPro;
using Photon.Pun;

namespace Photon_NetWork
{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private TextMeshProUGUI playerNameText;

        [SerializeField] private Slider playerHealthSlider;
        [SerializeField] private GameObject chatBox;
        [SerializeField] private TextMeshProUGUI chatText;
        [SerializeField] private Vector3 screenOffset = new Vector3(0f, 30f, 0f);
        private PlayerManager target;
        private float characterControllerHeight = 0f;
        private Transform targetTransform;
        private Renderer targetRenderer;
        private CanvasGroup _canvasGroup;
        private Vector3 targetPosition;
        public bool isMine;
        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            _canvasGroup = this.GetComponent<CanvasGroup>();
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }
        void Update()
        {
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.Health;
            }
        }
        private void LateUpdate()
        {
            if (targetRenderer != null)
            {
                this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

            // #Critical
            // Follow the Target GameObject on screen.
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }
        #endregion

        #region Public Methods
        public void SetTarget(PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            target = _target;
            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();
            CharacterController characterController = _target.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterControllerHeight = characterController.height;
            }
            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
            isMine = PlayerManager.LocalPlayerInstance;
        }
        private int ownID;
        public void SetID(int id)
        {
            ownID = id;
        }
        public void SetChatText(string text)
        {
            Debug.Log("Chat : " + text);
            chatBox.SetActive(true);
            chatText.text = text;
        }
        #endregion
    }
}