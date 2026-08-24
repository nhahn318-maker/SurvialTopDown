using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
    [Header("Scene References")]
    [SerializeField] private FixedJoystick moveJoystick;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerStats playerStats;

    [Header("Technical Movement Settings")]
    [SerializeField] private float gravity;
    [SerializeField] private float groundedVerticalSpeed;

    public Vector2 MovementInput { get; private set; }
    public Vector3 CurrentForward => transform.forward;

    private CharacterController characterController;
    private float verticalSpeed;
    private bool isInputEnabled = true;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (moveJoystick == null || cameraTransform == null || playerStats == null)
            return;

        ReadMovementInput();

        Vector3 moveDirection = GetCameraRelativeMoveDirection();

        RotateTowards(moveDirection);
        UpdateVerticalSpeed();
        Move(moveDirection);
    }

    private void ReadMovementInput()
    {
        MovementInput = isInputEnabled
            ? Vector2.ClampMagnitude(
                new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical),
                1f)
            : Vector2.zero;
    }

    private Vector3 GetCameraRelativeMoveDirection()
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(
            cameraTransform.forward,
            Vector3.up).normalized;

        Vector3 cameraRight = Vector3.ProjectOnPlane(
            cameraTransform.right,
            Vector3.up).normalized;

        return
            cameraRight * MovementInput.x +
            cameraForward * MovementInput.y;
    }

    private void RotateTowards(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude > Mathf.Epsilon)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                playerStats.TurnSpeedDegreesPerSecond * Time.deltaTime);
        }
    }

    private void UpdateVerticalSpeed()
    {
        if (characterController.isGrounded && verticalSpeed < 0f)
            verticalSpeed = groundedVerticalSpeed;

        verticalSpeed += gravity * Time.deltaTime;
    }

    private void Move(Vector3 moveDirection)
    {
        Vector3 velocity =
            moveDirection * playerStats.MoveSpeed +
            Vector3.up * verticalSpeed;

        characterController.Move(velocity * Time.deltaTime);
    }

    public void SetInputEnabled(bool value)
    {
        isInputEnabled = value;

        if (!isInputEnabled)
            MovementInput = Vector2.zero;
    }
}
