using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerMoveControl : MonoBehaviourPun
{
    public float playerSpeed = 3f;
    public float jumpForce = 5f;
    public float gravityValue = 9.81f;
    private CharacterController controller;

    private Vector3 inputVector;
    public Vector3 movePostion;
    private Vector3 lookPosition;
    private Vector3 jumpPosition;
    private float inputValue = 0;
    private bool isJump = false;

    private Camera mainCam;
    private PlayerAniControl playerAniControl;
    private CinemachineFreeLook cinemachineFreeLook;

    private void Awake()
    {
        controller = this.GetComponent<CharacterController>();
        playerAniControl = this.GetComponent<PlayerAniControl>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
            cinemachineFreeLook.m_LookAt = cinemachineFreeLook.m_Follow = this.transform;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetButtonDown("Jump"))
            Jump();

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");
        inputValue = Mathf.Clamp01(Mathf.Abs(inputVector.x) + Mathf.Abs(inputVector.z));

        movePostion = this.transform.forward * playerSpeed * inputValue * Time.deltaTime;

        if (inputValue > 0)
        {
            lookPosition = Quaternion.LookRotation(inputVector).eulerAngles;
            this.transform.rotation = Quaternion.Euler(Vector3.up * (lookPosition.y + mainCam.transform.eulerAngles.y)).normalized;
        }

        CollisionFlags flags = controller.Move(movePostion + jumpPosition);
        playerAniControl.AnimationChange(inputValue);

        if ((flags & CollisionFlags.Below) != 0)
        {
            if (isJump)
            {
                jumpPosition.y = 0f;
                isJump = false;
                playerAniControl.AnimationChange(PlayerAnimationType.Idle);
            }
        }
        else
        {
            jumpPosition.y -= gravityValue * Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (isJump == false)
        {
            isJump = true;
            jumpPosition.y = jumpForce;
            playerAniControl.AnimationChange(PlayerAnimationType.Jump);
        }
    }
}
