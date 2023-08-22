using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using domi.DB;

public class SlimeAttack : MonoBehaviour
{
    private SlimeMove _slimeMove;
    private DrawLine _drawLine;

    private LayerMask layer;

    private DBstruct _db;

    private void Awake() {
        _slimeMove = GetComponent<SlimeMove>();
        _drawLine = GameObject.Find("LineRenderer").GetComponent<DrawLine>();
        layer = LayerMask.GetMask("Enemy");

        _db = DBmanager.GetData();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("EnemyAttack")) {
            if (CheckEnemy(_slimeMove.direction, other.transform.parent)) BlockManager.instance.ShowPanel(false);
        }
        else if(other.CompareTag("Enemy")) {
            BatchCharacter.enemyColList.Remove((CircleCollider2D)other);
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Key")) {
            Debug.Log("Key");
            _db.takenKeyStage.Add(CreateBlock.stageInfo);
            DBmanager.Save();
            Destroy(other.gameObject);
        }
    }
    
    private bool CheckEnemy(Vector2 dir, Transform parent) {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, BlockManager.instance.tileSize * 1.5f, layer);
        this.dir = dir;

        if(ray.transform is not null)
            if(_slimeMove.curPos != _drawLine._posList.Count - 1 && _slimeMove.enemyPos[_slimeMove.curPos] != (Vector2)ray.transform.position) return false;

        return ray.transform == parent;
    }

    Vector2 dir = Vector2.zero;
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, dir);
    }
}
