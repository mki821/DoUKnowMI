using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _blockLayer;
    [SerializeField] private LayerMask _enemyLayer;

    [SerializeField] private SlimeMove _slimeMove;
    [SerializeField] private DrawLine _drawLine;

    private bool firstMove = false;

    private Camera _cam;

    private BatchCharacter _batchCharacter;

    private void Awake() {
        _cam = Camera.main;
        _batchCharacter = GetComponent<BatchCharacter>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0, _blockLayer);

            if (hit.collider != null) {
                Block block = hit.transform.GetComponent<Block>();

                Vector2 dir = block.worldPos - _slimeMove.movePos[_slimeMove.movePos.Count - 1];
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                if (Mathf.Abs(angle) % 45 == 0) {
                    if (CheckEnemy(_slimeMove.movePos[_slimeMove.movePos.Count - 1], dir.normalized, dir.magnitude)) {
                        _drawLine.SetLinePos(block.worldPos);
                        _slimeMove.SetMovePos(block.worldPos);
                    }
                }
            }
        }
    }

    private bool CheckEnemy(Vector2 pos, Vector2 dir, float distance) {
        RaycastHit2D[] d = Physics2D.RaycastAll(pos, dir, distance, _enemyLayer);

        this.pos = pos;
        this.dir = dir;
        this.distance = distance;

        if(!firstMove) {
            if(d.Length == 1) {
                firstMove = true;
                return true;
            }
            return false;
        }

        if(d.Length == 1 && (Vector2)d[0].transform.position == pos) {

        }
        return d.Length == 1 ? true : false;
    }

    Vector2 pos, dir = Vector2.zero;
    float distance = 0;
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pos, dir * distance);
    }
}
