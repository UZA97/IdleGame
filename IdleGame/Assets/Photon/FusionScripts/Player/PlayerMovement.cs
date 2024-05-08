using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private Vector3 _velocity;
    public Transform cmaPos;
    private bool _jumpPressed;

    private CharacterController _controller;

    public float PlayerSpeed = 2f;

    public float JumpForce = 5f;
    public float GravityValue = -9.81f;
    public Camera Camera;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
    }

    public override void FixedUpdateNetwork()
    {
        // Only move own player and not every other player. Each player controls its own player object.
        if (HasStateAuthority == false)
        {
            return;
        }

        if (_controller.isGrounded)
        {
            _velocity = new Vector3(0, -1, 0);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            NetworkTransform networkTransform = this.GetComponent<NetworkTransform>();
            networkTransform.Teleport(Vector3.zero);
            // this.transform.position = Vector3.zero;
        }
        Quaternion cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        Vector3 move = /*cameraRotationY **/ new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;
        // Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;

        _velocity.y += GravityValue * Runner.DeltaTime;
        if (_jumpPressed && _controller.isGrounded)
        {
            _velocity.y += JumpForce;
        }
        _controller.Move(move + _velocity * Runner.DeltaTime);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        _jumpPressed = false;
    }
    // Network 준비가 되었을 때 Call
    public override void Spawned()
    {
        Debug.Log("Spawned");
        if (HasStateAuthority)
        {
            Camera = Camera.main;
            Camera.GetComponent<FirstPersonCamera>().Target = cmaPos;
        }
    }
}