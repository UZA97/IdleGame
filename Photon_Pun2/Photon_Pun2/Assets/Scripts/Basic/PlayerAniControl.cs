using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum PlayerAnimationType
{
    Idle = 0,
    Walk,
    Jump,
    Attack,
    Damage
}

public class PlayerAniControl : MonoBehaviourPun
{
    private Animator animator;
    private PlayerAnimationType playerAnimationType;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AnimationChange(float speed)
    {
        // 내가 아니고 연결이 되어 있을 때 
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;

        animator.SetFloat("Move", speed);
    }

    public void AnimationChange(PlayerAnimationType type)
    {
        // 내가 아니고 연결이 되어 있을 때 
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;

        if (playerAnimationType == type) return;
        playerAnimationType = type;

        switch (type)
        {
            case PlayerAnimationType.Idle:
                animator.SetBool("Jump", false);
                animator.Play("Move");
                break;
            case PlayerAnimationType.Jump:
                animator.SetBool("Jump", true);
                break;
            case PlayerAnimationType.Attack:
                animator.SetTrigger("Attack");
                break;
            case PlayerAnimationType.Damage:
                animator.SetTrigger("Damage");
                break;
            default:
                // animator.Play(type.ToString());
                break;
        }
    }
}
