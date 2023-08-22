using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    public List<Vector3> _posList = new List<Vector3>();

    private void Awake() {
        _lineRenderer = (LineRenderer)GetComponent("LineRenderer");
    }

    public void SetLinePos(Vector2 pos) {
        _posList.Add(pos);
        Debug.Log(_posList.Count);
        
        _lineRenderer.positionCount = _posList.Count;
        _lineRenderer.SetPositions(_posList.ToArray());
        
    }

    public void RevertLinePos() {
        if(_posList.Count > 1) {
            _posList.Remove(_posList[_posList.Count - 1]);

            _lineRenderer.positionCount = _posList.Count;
        }
    }
}
