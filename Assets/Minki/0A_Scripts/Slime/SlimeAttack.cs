using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    private SlimeMove _slimeMove;

    private LayerMask layer;

    private void Awake() {
        _slimeMove = GetComponent<SlimeMove>();
        layer = LayerMask.GetMask("Enemy");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("EnemyAttack")) {
            if (CheckEnemy(_slimeMove.direction, other.transform.parent)) Destroy(gameObject);
        }
        else if(other.CompareTag("Enemy")) {
            BatchCharacter.enemyColList.Remove((CircleCollider2D)other);
            Destroy(other.gameObject);
        }
    }
    
    private bool CheckEnemy(Vector2 dir, Transform parent) {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, BlockManager.instance.tileSize * 1.5f, layer);
        this.dir = dir;

        Debug.Log($"{_slimeMove.curPos} != {_slimeMove.movePos.Count}");
        if(ray.transform is not null)
            if(_slimeMove.curPos != _slimeMove.movePos.Count - 1 && _slimeMove.enemyPos[_slimeMove.curPos] != (Vector2)ray.transform.position) return false;

        return ray.transform == parent;
    }

    Vector2 dir = Vector2.zero;
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, dir);
    }
}
