using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private List<Vector3> _posList = new List<Vector3>();

    private void Awake() {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetLinePos(Vector2 pos) {
        _posList.Add(pos);

        
        _lineRenderer.positionCount = _posList.Count;
        _lineRenderer.SetPosition(_posList.Count - 1, pos);
    }
}
