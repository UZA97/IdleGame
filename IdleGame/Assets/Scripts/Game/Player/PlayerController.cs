using UnityEngine;

public class PlayerController: MonoBehaviour
{
    [SerializeField] FloatingJoystick variableJoystick;
    [SerializeField] ForceMode mode;
    [SerializeField] Animator anim;
    [SerializeField] PlayerWeponSword wepon;
    private Movement3D movement;
    private void Awake()
    {
        movement = GetComponent<Movement3D>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Attack();
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        direction = new Vector3(x, 0, z);

        movement.MoveTo(direction.normalized);
        if (direction != Vector3.zero)
        {
            movement.LookRotation();
            anim.SetBool("Move", true);
        }
        else
            anim.SetBool("Move", false);
    }
    public void Attack()
    {
        anim.SetBool("Attack", true);
        wepon.Attack();
    }
}