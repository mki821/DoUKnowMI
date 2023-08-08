using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCheck : MonoBehaviour
{
    public List<Vector2> enemyPos = new List<Vector2>();

    [SerializeField] private LayerMask _blockLayer;
    [SerializeField] private LayerMask _enemyLayer;

    [SerializeField] private SlimeMove _slimeMove;
    [SerializeField] private DrawLine _drawLine;

    private Camera _cam;

    private BatchCharacter _batchCharacter;

    private void Awake() {
        _cam = Camera.main;
        _batchCharacter = GetComponent<BatchCharacter>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if(_slimeMove is null) _slimeMove = BlockManager.instance.slimeMove; 

            Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0, _blockLayer);

            if (hit.collider != null) {
                Block block = hit.transform.GetComponent<Block>();

                Vector2 dir = block.worldPos - _slimeMove.movePos[_slimeMove.movePos.Count - 1];
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                if (Mathf.Abs(angle) % 45 == 0) {
                    if (CheckEnemy(_slimeMove.movePos[_slimeMove.movePos.Count - 1], dir.normalized, dir.magnitude, block.transform.position)) {
                        _drawLine.SetLinePos(block.worldPos);
                        _slimeMove.SetMovePos(block.worldPos);
                    }
                }
            }
        }
    }

    private bool CheckEnemy(Vector2 pos, Vector2 dir, float distance, Vector2 blockPos) {
        RaycastHit2D[] d = Physics2D.RaycastAll(pos, dir, distance, _enemyLayer);

        this.pos = pos;
        this.dir = dir;
        this.distance = distance;

        if(d.Length == 1 && (Vector2)d[0].transform.position == blockPos) {
            return false;
        }

        if(d.Length == 1) {
            _slimeMove.AddEnemyPos(d[0].transform.position);
            d[0].collider.enabled = false;
            return true;
        }
        else {
            return false;
        }
    }

    Vector2 pos, dir = Vector2.zero;
    float distance = 0;
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pos, dir * distance);
    }
}
