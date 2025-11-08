using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private EdgeCollider2D _collider;

    private List<Vector2> _points = new();

    private void Start()
    {
        //_collider.transform.position -= transform.position;
    }

    public void SetPosition(Vector2 position)
    {
        if (CamAppend(position) == false) return;

        Vector2 localPosition = transform.InverseTransformPoint(position);

        _points.Add(localPosition);
    }

    public void OnEndDraw()
    {
        this.FinalizeLine();
        this.AddComponent<Rigidbody2D>();
    }

    private bool CamAppend(Vector2 position)
    {
        if (_renderer.positionCount == 0) return true;

        return Vector2.Distance(_renderer.GetPosition(_renderer.positionCount - 1), position) > DrawManager.RESOLUTION;
    }

    private void FinalizeLine()
    {
        if (_points.Count < 2) return;

        // 1️⃣ Convert all local points to world space
        List<Vector2> worldPoints = new List<Vector2>(_points.Count);
        foreach (var p in _points)
            worldPoints.Add(transform.TransformPoint(p));

        // 2️⃣ Compute the center in world space
        Vector2 center = Vector2.zero;
        foreach (var p in worldPoints)
            center += p;
        center /= worldPoints.Count;

        // 3️⃣ Move transform to center
        transform.position = center;

        // 4️⃣ Rebuild local-space points (relative to new position)
        for (int i = 0; i < worldPoints.Count; i++)
            _points[i] = transform.InverseTransformPoint(worldPoints[i]);

        // 5️⃣ Update renderer & collider
        _renderer.positionCount = _points.Count;
        for (int i = 0; i < _points.Count; i++)
            _renderer.SetPosition(i, _points[i]);
        _collider.points = _points.ToArray();

        // 6️⃣ Add Rigidbody
        var rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 1f;

        _points.Clear();
    }
}