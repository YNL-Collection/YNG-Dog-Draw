using UnityEngine;
using UnityEngine.UI;

public class DrawManager : MonoBehaviour
{
    public const float RESOLUTION = 0.1f;

    [SerializeField] private Line _linePrefab;
    [SerializeField] private float _pointDistance = 0.1f;

    [Header("Limit")]
    public float MaxLength = 3f;

    [SerializeField] private Rigidbody2D _player;
    [SerializeField] private Beehive _beehive;

    private Line _currentLine;
    private Vector2 _previousMousePos;
    private bool _isDrawing;
    private float _currentLength;

    [SerializeField] private Image _barFill;

    private void Update()
    {
        Vector3 mousePosition3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition = new Vector2(mousePosition3D.x, mousePosition3D.y);

        if (Input.GetMouseButtonDown(0))
        {
            _currentLine = Instantiate(_linePrefab, mousePosition, Quaternion.identity);
            _previousMousePos = mousePosition;
            _currentLength = 0f;     // reset
            _isDrawing = true;
        }

        if (Input.GetMouseButton(0) && _isDrawing && _currentLine != null)
        {
            float distance = Vector2.Distance(mousePosition, _previousMousePos);

            // --- LIMIT CHECK ---
            if (_currentLength + distance > MaxLength)
            {
                // clamp last segment to max boundary
                float remain = MaxLength - _currentLength;
                if (remain > 0f)
                {
                    Vector2 direction = (mousePosition - _previousMousePos).normalized;
                    Vector2 finalPos = _previousMousePos + direction * remain;
                    _currentLine.SetPosition(finalPos);
                }

                EndDrawInternal();
                return;
            }

            // normal append
            if (distance > _pointDistance)
            {
                _currentLine.SetPosition(mousePosition);
                _currentLength += distance;
                _previousMousePos = mousePosition;

                _barFill.fillAmount = (MaxLength - _currentLength) / MaxLength;
            }
        }

        if (Input.GetMouseButtonUp(0) && _isDrawing)
        {
            EndDrawInternal();
        }
    }

    private void EndDrawInternal()
    {
        _currentLine.OnEndDraw();
        _currentLine = null;
        _isDrawing = false;

        _beehive.Begin();
        _player.gravityScale = 1;

        LevelManager.OnDrawEnd?.Invoke();
    }
}