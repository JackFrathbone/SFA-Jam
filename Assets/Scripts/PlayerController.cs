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

    private Vector2 screenPos;
    private Vector2 screenExtents;
    private float newX;
    private float newY;
    private readonly Vector3 halfUnit = Vector3.one / 2;

    private PlayerInput _playerInput;
    private Vector2 _moveInputValue;
    private float _horizontalInput;
    private float _verticalInput;

    private TrailRenderer _trailRenderer;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerInput = FindObjectOfType<PlayerInput>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();

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

    private void Update()
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

    private void WrapScreen()
    {
        screenPos = _mainCamera.WorldToViewportPoint(_spriteRenderer.bounds.center);

        screenExtents = (_mainCamera.WorldToViewportPoint(_spriteRenderer.bounds.extents) - halfUnit);

        if (screenPos.x > 1.0f + screenExtents.x)
        {
            newX = -screenExtents.x;
            newY = screenPos.y;
            SetNewPosition();
        }
        if (screenPos.x < 0 - screenExtents.x)
        {
            newX = 1.0f + screenExtents.x;
            newY = screenPos.y;
            SetNewPosition();
        }
        if (screenPos.y > 1.0f + screenExtents.y)
        {
            newY = -screenExtents.y;
            newX = screenPos.x;
            SetNewPosition();
        }
        if (screenPos.y < 0 - screenExtents.y)
        {
            newY = 1.0f + screenExtents.y;
            newX = screenPos.x;
            SetNewPosition();
        }
    }

    private void SetNewPosition()
    {
        _trailRenderer.Clear();
        //_rigidbody.MovePosition(_mainCamera.ViewportToWorldPoint(new Vector3(newX, newY, 10)));
        transform.position = _mainCamera.ViewportToWorldPoint(new Vector3(newX, newY, 10));
        _trailRenderer.Clear();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(gameObject);
        }
    }
}
