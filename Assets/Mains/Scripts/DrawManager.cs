using Unity.VisualScripting;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public const float RESOLUTION = 0.1f;

    [SerializeField] private Line _linePrefab;

    private Line _currentLine;

    private void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            _currentLine = Instantiate(_linePrefab, new Vector3(mousePosition.x, mousePosition.y, 0), Quaternion.identity);
        }

        if (Input.GetMouseButton(0))
        {
            _currentLine.SetPosition(mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _currentLine.OnEndDraw();
        }
    }
}
