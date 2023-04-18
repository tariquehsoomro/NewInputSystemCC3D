using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputSystemCharacterConroller : MonoBehaviour
{
    [Header("Serialized Fields")]
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _transform;

    [Header("Movement variables")]
    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _runSpeed = 3f;
    [SerializeField] private float _sprintSpeed = 8f;

    [Header("Gravity variables")]
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _gravityMultiplier = 3f;

    [Header("Velocity variables")]
    [SerializeField] private float _forceAppliedOverTimeWhenOnGround = -2f;

    private InputAsset _inputActions;
    private InputAction _movementAction;

    private Vector3 _velocity;
    private Vector3 _moveVec;

    private bool isGrounded = false;

    private float _inputX;
    private float _inputZ;
    private float _delta;

    private void Awake()
    {
        Initialization();
    }

    private void Start()
    {
        ValidationCheck();
    }

    private void OnEnable()
    {
        _movementAction = _inputActions.Player.Movement;
        _movementAction.Enable();
    }

    private void OnDisable()
    {
        _movementAction.Disable();
    }

    private void Update()
    {
        _delta = Time.deltaTime;
        isGrounded = IsTouchingGround();
        ApplyMovementAndVelocity();
    }

    private bool IsTouchingGround()
    {
        return _controller.isGrounded;
    }

    private void ApplyMovementAndVelocity()
    {
        if (isGrounded && _velocity.y < 0) _velocity.y = _forceAppliedOverTimeWhenOnGround;

        _inputX = _movementAction.ReadValue<Vector2>().x;
        _inputZ = _movementAction.ReadValue<Vector2>().y;

        _moveVec = _transform.right * _inputX + _transform.forward * _inputZ;

        _controller.Move(_moveVec.normalized * _runSpeed * _delta);

        _velocity.y += (_gravity * _gravityMultiplier) * _delta;

        _controller.Move(_velocity * _delta);
    }

    private void ValidationCheck()
    {
        if (_controller == null) _controller = GetComponent<CharacterController>() ? GetComponent<CharacterController>() : gameObject.AddComponent<CharacterController>();
    }

    private void Initialization()
    {
        _transform = transform;

        _inputActions = new InputAsset();
    }
}
