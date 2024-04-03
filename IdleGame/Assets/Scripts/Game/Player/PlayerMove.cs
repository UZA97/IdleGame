using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] FloatingJoystick variableJoystick;
    [SerializeField] CharacterController character;
    [SerializeField] Rigidbody rb;
    [SerializeField] ForceMode mode;
    [SerializeField] Animator anim;
    Vector3 lookDeirection;
    private float gravity = -9.81f;
    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        //Debug.Log(direction);
        character.SimpleMove(direction * speed * Time.deltaTime);
        //rb.AddForce(direction * speed * Time.deltaTime, mode);
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
            anim.SetBool("Move", true);
        }
        else
            anim.SetBool("Move", false);
    }
}