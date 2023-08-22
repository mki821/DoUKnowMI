using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _blockLayer;
    [SerializeField] private LayerMask _enemyLayer;

    [SerializeField] private SlimeMove _slimeMove;
    [SerializeField] private DrawLine _drawLine;

    private List<Collider2D> enemyColList = new List<Collider2D>();

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

                Vector2 dir = block.worldPos - (Vector2)_drawLine._posList[_drawLine._posList.Count - 1];
                int angle = (int)Mathf.Abs(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

                if (angle % 45 == 0) {
                    if (CheckEnemy(_drawLine._posList[_drawLine._posList.Count - 1], dir.normalized, dir.magnitude, block.transform.position)) {
                        _drawLine.SetLinePos(block.worldPos);
                    }
                }
            }
        }
    }

    public void Back() {
        if (enemyColList.Count > 0) {
            _drawLine.RevertLinePos();
            enemyColList[enemyColList.Count - 1].enabled = true;
            enemyColList.Remove(enemyColList[enemyColList.Count - 1]);
        }
    }

    private bool CheckEnemy(Vector2 pos, Vector2 dir, float distance, Vector2 blockPos) {
        RaycastHit2D[] d = Physics2D.RaycastAll(pos, dir, distance, _enemyLayer);

        this.pos = pos;
        this.dir = dir;
        this.distance = distance;

        if(d.Length == 1 && (Vector2)d[0].transform.position - _batchCharacter.characterOffset == blockPos) {
            return false;
        }

        if(d.Length == 1) {
            _slimeMove.AddEnemyPos(d[0].transform.position);
            d[0].collider.enabled = false;
            enemyColList.Add(d[0].collider);
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
