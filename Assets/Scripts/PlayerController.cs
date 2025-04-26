using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerNumber
{
    one,
    two,
    three,
    four
}
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlayerNumber _playerNumber;

    [SerializeField] private float _thrustForce = 5f;
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private float _maxVelocity = 10f;

    [Header("References")]
    [SerializeField] private GameObject _frontFire;
    [SerializeField] private GameObject _backFire;

    private Rigidbody2D _rigidbody;

    private Camera _mainCamera;
    private SpriteRenderer _spriteRenderer;

    private bool _isWrappingX = false;
    private bool _isWrappingY = false;

    private PlayerInput _playerInput;
    private Vector2 _moveInputValue;
    private float _horizontalInput;
    private float _verticalInput;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerInput = FindObjectOfType<PlayerInput>();

        _mainCamera = Camera.main;

        switch (_playerNumber)
        {
            case PlayerNumber.one:
                _spriteRenderer.color = Color.green;
                break;
            case PlayerNumber.two:
                _spriteRenderer.color = Color.red;
                break;
            case PlayerNumber.three:
                _spriteRenderer.color = Color.blue;
                break;
            case PlayerNumber.four:
                _spriteRenderer.color = Color.yellow;
                break;
        }
    }

    void Update()
    {
        GetMovementInput();

        float rotationInput = -_horizontalInput;
        _rigidbody.angularVelocity = rotationInput * _rotationSpeed;

        WrapScreen();
    }

    void FixedUpdate()
    {
        // Handle thrust input
        if (_verticalInput > 0)
        {
            _rigidbody.AddForce(transform.up * _thrustForce, ForceMode2D.Force);
            _backFire.SetActive(true);
        }
        else if (_verticalInput < 0)
        {
            _rigidbody.AddForce(transform.up * -_thrustForce, ForceMode2D.Force);
            _frontFire.SetActive(true);
        }
        else
        {
            _frontFire.SetActive(false);
            _backFire.SetActive(false);
        }

        // Limit the maximum velocity
        if (_rigidbody.velocity.magnitude > _maxVelocity)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxVelocity;
        }
    }

    void WrapScreen()
    {
        bool isVisible = _spriteRenderer.isVisible;

        if (isVisible)
        {
            _isWrappingX = false;
            _isWrappingY = false;
            return;
        }
        if (_isWrappingX && _isWrappingY)
        {
            return;
        }

        Vector2 viewportPosition = _mainCamera.WorldToViewportPoint(transform.position);
        Vector2 newPosition = transform.position;
        if (!_isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;
            _isWrappingX = true;
        }
        if (!_isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;
            _isWrappingY = true;
        }
        transform.position = newPosition;
    }

    private void GetMovementInput()
    {
        switch (_playerNumber)
        {
            case PlayerNumber.one:
                _moveInputValue = _playerInput.actions.FindAction("MovePlayer1").ReadValue<Vector2>();
                _verticalInput = _playerInput.actions.FindAction("AttackPlayer1").ReadValue<float>();
                break;
            case PlayerNumber.two:
                _moveInputValue = _playerInput.actions.FindAction("MovePlayer2").ReadValue<Vector2>();
                _verticalInput = _playerInput.actions.FindAction("AttackPlayer2").ReadValue<float>();
                break;
            case PlayerNumber.three:
                _moveInputValue = _playerInput.actions.FindAction("MovePlayer3").ReadValue<Vector2>();
                _verticalInput = _playerInput.actions.FindAction("AttackPlayer3").ReadValue<float>();
                break;
            case PlayerNumber.four:
                break;
        }

        _horizontalInput = _moveInputValue.x;

        if (_verticalInput == 0)
        {
            _verticalInput = -_playerInput.actions.FindAction("Crouch").ReadValue<float>();
        }
    }
}
