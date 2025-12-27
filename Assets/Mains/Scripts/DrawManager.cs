using UnityEngine;
using UnityEngine.UI;

public class DrawManager : MonoBehaviour
{
    public const float RESOLUTION = 0.1f;

    [SerializeField] private Line _linePrefab;
    [SerializeField] private float _pointDistance = 0.1f;

    [Header("Limit")]
    public float MaxLength = 3f;

    [Header("UI")]
    [SerializeField] private Image _barFill;
    [SerializeField] private Image Star1;
    [SerializeField] private Image Star2;
    [SerializeField] private Image Star3;

    private Line _currentLine;
    private Vector2 _previousMousePos;
    private bool _isDrawing;
    private float _currentLength;

    private bool _waitingLeaveGround;
    private bool _touchingGroundWhileDrawing;

    private const string GROUND_LAYER_NAME = "Ground";
    private int _groundLayer;

    private void Awake()
    {
        _groundLayer = LayerMask.NameToLayer(GROUND_LAYER_NAME);
        if (_groundLayer == -1)
            Debug.LogError("Layer 'Ground' chưa được tạo!");
    }

    private void Update()
    {
        Vector3 mouse3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mouse3D.x, mouse3D.y);

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

        if (Input.GetMouseButton(0) && _isDrawing)
        {
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

            if (IsOnGround(mousePos))
            {
                _touchingGroundWhileDrawing = true;
                _previousMousePos = mousePos;
                return;
            }

            _touchingGroundWhileDrawing = false;

            float distance = Vector2.Distance(mousePos, _previousMousePos);

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

                UpdateStars();
                return;
            }

            if (distance > _pointDistance)
            {
                _currentLine.SetPosition(mousePos);
                _currentLength += distance;
                _previousMousePos = mousePos;

                if (_barFill != null)
                    _barFill.fillAmount = (MaxLength - _currentLength) / MaxLength;

                UpdateStars();
            }
        }

        if (Input.GetMouseButtonUp(0) && _isDrawing)
        {
            EndDrawAndCallBeehive();
            _waitingLeaveGround = false;
            _touchingGroundWhileDrawing = false;
        }
    }

    private void StartNewLine(Vector2 pos)
    {
        _currentLine = Instantiate(_linePrefab, pos, Quaternion.identity);
        _previousMousePos = pos;
        _currentLength = 0f;

        if (_barFill != null)
            _barFill.fillAmount = 1f;

        if (Star1 != null) Star1.enabled = true;
        if (Star2 != null) Star2.enabled = true;
        if (Star3 != null) Star3.enabled = true;
    }

    private void UpdateStars()
    {
        float oneStar = MaxLength / 3f;

        if (Star3 != null)
            Star3.enabled = _currentLength < oneStar;

        if (Star2 != null)
            Star2.enabled = _currentLength < oneStar * 2f;
    }

    private bool IsOnGround(Vector2 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos);
        if (hit == null) return false;

        return hit.gameObject.layer == _groundLayer;
    }

    private void EndDrawAndCallBeehive()
    {
        if (_currentLine != null)
            _currentLine.OnEndDraw();

        _currentLine = null;
        _isDrawing = false;

        LevelManager.Instance.StartCountTime();

        Beehive[] beehives = FindObjectsOfType<Beehive>();
        foreach (var hive in beehives)
        {
            hive.Begin();
        }
    }

    public void ResetDrawUI()
    {
        _currentLine = null;
        _isDrawing = false;
        _currentLength = 0f;
        _waitingLeaveGround = false;
        _touchingGroundWhileDrawing = false;

        if (_barFill != null)
            _barFill.fillAmount = 1f;

        if (Star1 != null) Star1.enabled = true;
        if (Star2 != null) Star2.enabled = true;
        if (Star3 != null) Star3.enabled = true;
    }
}
