using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace Photon_NetWork
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Fields
        private Animator animator;
        private Vector3 dir;
        private CharacterController controller;
        private PlayerAniState aniState;
        #endregion

        #region SerializeField Private Fields
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float jumpSpeed = 15.0f;

        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
                return;
            Move();
        }
        // void FixedUpdate()
        // {

        private void Move()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if (controller.isGrounded)
            {
                SetAniState(PlayerAniState.None);
                dir = new Vector3(x, 0, y);
                if (dir != Vector3.zero)
                {
                    this.transform.forward = dir.normalized;
                    // Vector3 lookVec = Quaternion.LookRotation(dir).eulerAngles;
                    // transform.rotation = Quaternion.Euler(Vector3.up * (lookVec.y + Camera.main.transform.eulerAngles.y)).normalized;
                }

                float aniSpeed = Mathf.Clamp01(Mathf.Abs(dir.x) + Mathf.Abs(dir.z));
                animator.SetFloat("Move", aniSpeed);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetAniState(PlayerAniState.Jump);
                    dir.y = jumpSpeed;
                }
            }
            dir.y += Physics.gravity.y * Time.deltaTime;
            controller.Move(dir.normalized * moveSpeed * Time.deltaTime);
            // ProcessInputs();
        }

        #endregion
        public void SetAniState(PlayerAniState state)
        {
            aniState = state;
            animator.SetInteger("Anim", (int)aniState);
        }
    }
}