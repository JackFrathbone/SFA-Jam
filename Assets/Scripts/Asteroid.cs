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

    private Vector2 screenPos;
    private Vector2 screenExtents;
    private float newX;
    private float newY;
    private readonly Vector3 halfUnit = Vector3.one / 2;

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

    void WrapScreen()
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
        transform.position = _mainCamera.ViewportToWorldPoint(new Vector3(newX, newY, 10));
    }
}
