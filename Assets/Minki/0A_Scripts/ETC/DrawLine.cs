using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    public Stack<Vector3> _posList = new Stack<Vector3>();

    private void Awake() {
        _lineRenderer = (LineRenderer)GetComponent("LineRenderer");
    }

    public void SetLinePos(Vector2 pos) {
        _posList.Push(pos);
        
        _lineRenderer.positionCount = _posList.Count;
        _lineRenderer.SetPositions(_posList.ToArray());
        
    }

    public void RevertLinePos() {
        if(_posList.Count > 2) {
            _posList.Pop();

            _lineRenderer.positionCount = _posList.Count;
            _lineRenderer.SetPositions(_posList.ToArray());
        }
    }
}
