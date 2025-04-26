using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _scaleMin = 0.5f;
    [SerializeField] float _scaleMax = 1.5f;
    [SerializeField] float _speedMin = 1f;
    [SerializeField] float _speedMax = 5f;

    private Camera _mainCamera;
    private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rigidbody;

    private bool _isWrappingX = false;
    private bool _isWrappingY = false;

    private void Start()
    {
        _mainCamera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _rigidbody.AddTorque(Random.Range(-3f, 3f));

        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        _rigidbody.AddForce(randomDirection * (Random.Range(_speedMin, _speedMax)), ForceMode2D.Force);

        float randomScale = Random.Range(_scaleMin, _scaleMax);

        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    private void Update()
    {
        WrapScreen();
    }

    private void WrapScreen()
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
}
