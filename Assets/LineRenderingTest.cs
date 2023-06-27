using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderingTest : MonoBehaviour
{
    private int count = 0;
    private List<Vector3> pos = new List<Vector3>();
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.Raycast(mousePos, Vector2.zero).collider;

            if (hit != null && (pos.Count == 0 ||
               (hit.transform.position.x == pos[count - 1].x ||
               hit.transform.position.y == pos[count - 1].y || AngleCheck(hit))))
            {
                if (hit.gameObject.CompareTag("Tile"))
                {
                    if (_lineRenderer.positionCount == count)
                    {
                        _lineRenderer.positionCount++;
                    }

                    _lineRenderer.SetPosition(count, hit.transform.position);
                    pos.Add(hit.transform.position);
                    count++;
                }
            }
        }
    }

    private bool AngleCheck(Collider2D hit)
    {
        if (pos.Count != 0)
        {
            Vector2 vec = new Vector2(hit.transform.position.x, hit.transform.position.y) - (Vector2)pos[count - 1];

            float x = Mathf.Abs(vec.x);
            float y = Mathf.Abs(vec.y);

            Vector2 v2 = new Vector2(x, y).normalized - Vector2.zero;

            return Mathf.Round(Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg) == 45;
        }
        return false;
    }
}
