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
    [SerializeField] private Image _barFill;

    private Line _currentLine;
    private Vector2 _previousMousePos;
    private bool _isDrawing;
    private float _currentLength;

    // ⭐ trạng thái
    private bool _waitingLeaveGround;
    private bool _touchingGroundWhileDrawing;

    private const string GROUND_LAYER_NAME = "Ground";
    private int _groundLayer;

    private Beehive[] _beehives;

    private void Awake()
    {
        _beehives = FindObjectsOfType<Beehive>();

        _groundLayer = LayerMask.NameToLayer(GROUND_LAYER_NAME);
        if (_groundLayer == -1)
            Debug.LogError("Layer 'Ground' chưa được tạo!");
    }

    private void Update()
    {
        Vector3 mouse3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mouse3D.x, mouse3D.y);

        // ================= MOUSE DOWN =================
        if (Input.GetMouseButtonDown(0))
        {
            _isDrawing = true;

            if (IsOnGround(mousePos))
            {
                _waitingLeaveGround = true;
                _previousMousePos = mousePos;
                return;
            }

            StartNewLine(mousePos);
        }

        // ================= MOUSE HOLD =================
        if (Input.GetMouseButton(0) && _isDrawing)
        {
            // ⏳ Chờ rời Ground
            if (_waitingLeaveGround)
            {
                if (!IsOnGround(mousePos))
                {
                    StartNewLine(mousePos);
                    _waitingLeaveGround = false;
                }
                else
                {
                    _previousMousePos = mousePos;
                    return;
                }
            }

            if (_currentLine == null)
                return;

            // 🚫 Đang vẽ mà chạm Ground → KHÔNG vẽ
            if (IsOnGround(mousePos))
            {
                _touchingGroundWhileDrawing = true;
                _previousMousePos = mousePos;
                return;
            }

            _touchingGroundWhileDrawing = false;

            float distance = Vector2.Distance(mousePos, _previousMousePos);

            // 🚫 Giới hạn độ dài
            if (_currentLength + distance >= MaxLength)
            {
                float remain = MaxLength - _currentLength;
                if (remain > 0f)
                {
                    Vector2 dir = (mousePos - _previousMousePos).normalized;
                    Vector2 finalPos = _previousMousePos + dir * remain;
                    _currentLine.SetPosition(finalPos);
                }

                _currentLength = MaxLength;

                if (_barFill != null)
                    _barFill.fillAmount = 0f;

                return;
            }

            // ✏️ Vẽ bình thường
            if (distance > _pointDistance)
            {
                _currentLine.SetPosition(mousePos);
                _currentLength += distance;
                _previousMousePos = mousePos;

                if (_barFill != null)
                    _barFill.fillAmount = (MaxLength - _currentLength) / MaxLength;
            }
        }

        // ================= MOUSE UP =================
        if (Input.GetMouseButtonUp(0) && _isDrawing)
        {
            EndDrawAndCallBeehive();
            _waitingLeaveGround = false;
            _touchingGroundWhileDrawing = false;
        }
    }

    // ================= CREATE LINE =================
    private void StartNewLine(Vector2 pos)
    {
        _currentLine = Instantiate(_linePrefab, pos, Quaternion.identity);
        _previousMousePos = pos;
        _currentLength = 0f;

        if (_barFill != null)
            _barFill.fillAmount = 1f;

        if (_player != null)
            _player.gravityScale = 0;
    }

    // ================= CHECK GROUND =================
    private bool IsOnGround(Vector2 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos);
        if (hit == null) return false;

        return hit.gameObject.layer == _groundLayer;
    }

    // ================= END DRAW =================
    private void EndDrawAndCallBeehive()
    {
        if (_currentLine != null)
            _currentLine.OnEndDraw();

        _currentLine = null;
        _isDrawing = false;

        foreach (var hive in _beehives)
        {
            if (hive != null)
                hive.Begin();
        }

        if (_player != null)
            _player.gravityScale = 1;
    }
}
