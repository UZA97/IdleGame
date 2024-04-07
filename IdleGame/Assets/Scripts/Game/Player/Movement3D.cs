using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField]
    GameObject target;
    [SerializeField]
    private Vector3 moveDirection;
    private float moveSpeed = 5.0f;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
    public void LookRotation()
    {
        if (target != null)
            target.gameObject.transform.rotation = Quaternion.LookRotation(moveDirection);
    }
}
