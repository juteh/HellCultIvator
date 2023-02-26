using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float groundDrag;

    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;
    [SerializeField] MovementState state;

    [Header("Crouching")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchYScale;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] Transform orientation;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    private bool _playerOnGround;
    private float _horizontalInput;
    private float _verticalInput;
    private bool _readyToJump;
    private float _moveSpeed;
    // scaling of players hight for croaching
    private float _startYScale;

    private Vector3 _moveDirection;

    private Rigidbody _playerRigidbody;

    public enum MovementState {
        walking,
        sprinting,
        air,
        crouching
    }

    private void Start() {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
        _readyToJump = true;
        _startYScale = transform.localScale.y;
    }

    private void Update() {

        // ground check with raycast to the bottom. Raycast goes from middle of player to 0.2 in the ground
        _playerOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        PlayerInput();
        SpeedControl();
        MovementStateHandler();

        if (_playerOnGround) {
            _playerRigidbody.drag = groundDrag;
        } else {
            _playerRigidbody.drag = 0;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void PlayerInput() {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && _readyToJump && _playerOnGround) {
            _readyToJump = false;
            Jump();

            // allows to continously to jump if the jumpkey is permanent pressed
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            // reducing the scale of player will shrink the body in the center
            // add force to push the body in the ground
            _playerRigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
        }

    }

    private void MovePlayer() {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (_playerOnGround) {
            _playerRigidbody.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        } else {
            _playerRigidbody.AddForce(_moveDirection.normalized * _moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

    }

    private void SpeedControl() {
        Vector3 flatVelocity = new Vector3(_playerRigidbody.velocity.x, 0f, _playerRigidbody.velocity.z);

        if (flatVelocity.magnitude > _moveSpeed) {
            Vector3 limitedVelocity = flatVelocity.normalized * _moveSpeed;
            _playerRigidbody.velocity = new Vector3(limitedVelocity.x, _playerRigidbody.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump() {
        // reset y velocity for always jumping with the same height
        _playerRigidbody.velocity = new Vector3(_playerRigidbody.velocity.x, 0f, _playerRigidbody.velocity.z);

        // applying the force only once with ForceMode.Impulse
        _playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        _readyToJump = true;
    }

    private void MovementStateHandler() {
        if (Input.GetKey(crouchKey)) {
            state = MovementState.crouching;
            _moveSpeed = crouchSpeed;
        }

        if (_playerOnGround && Input.GetKey(sprintKey)) {
            state = MovementState.sprinting;
            _moveSpeed = sprintSpeed;
        } else if (_playerOnGround) {
            state = MovementState.walking;
            _moveSpeed = walkSpeed;
        } else {
            state = MovementState.air;
        }

    }
}
