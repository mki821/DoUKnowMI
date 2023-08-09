using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType {
    Slime = 0,
    Knight,
}

public enum EnemyDir {
    Up = 0,
    Down,
    Left,
    Right
}

public class BatchCharacter : MonoBehaviour
{
    [HideInInspector] public BatchSO batchSO;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private RuntimeAnimatorController[] _animators;

    private List<CircleCollider2D> enemyColList = new List<CircleCollider2D>();
    private DrawLine _drawLine;

    private void Awake() {
        _drawLine = GameObject.Find("LineRenderer").GetComponent<DrawLine>();
    }

    public void Batch(Vector2 pos, EnemyDir dir, int type) {
        GameObject obj = new GameObject();
        obj.transform.position = pos;

        SpriteRenderer objSpr = obj.AddComponent<SpriteRenderer>();
        objSpr.sprite = _sprites[type];
        objSpr.sortingLayerName = "Character";
        objSpr.sortingOrder = 5;

        CircleCollider2D objCol = obj.AddComponent<CircleCollider2D>();
        objCol.offset = new Vector2(0, 0);
        objCol.radius = BlockManager.instance.tileSize / 5f;

        Animator objAnim = obj.AddComponent<Animator>();
        objAnim.runtimeAnimatorController = _animators[type];

        if(type == 0) {
            BlockManager.instance.slimeMove = obj.AddComponent<SlimeMove>();
            obj.AddComponent<SlimeAttack>();
            _drawLine.SetLinePos(obj.transform.position);

            Rigidbody2D objRig = obj.AddComponent<Rigidbody2D>();
            objRig.gravityScale = 0;
        }
        else if(type > 0) {
            enemyColList.Add(objCol);
            obj.tag = "Enemy";
            obj.layer = 7;
            objCol.isTrigger = true;
            switch(type) {
                case 2:
                    switch((int)dir) {
                        case 0:
                            CreateEnemy(obj.transform, new Vector3(0, -BlockManager.instance.tileSize));
                            break;
                        case 1:
                            CreateEnemy(obj.transform, new Vector3(BlockManager.instance.tileSize, 0));
                            break;
                        case 2:
                            CreateEnemy(obj.transform, new Vector3(0, BlockManager.instance.tileSize));
                            break;
                        case 3:
                            CreateEnemy(obj.transform, new Vector3(-BlockManager.instance.tileSize, 0));
                            break;
                    }
                    break;
            }
        }
    }

    public void BatchAll() {
        foreach(var item in batchSO.batchObject) {
            Vector2 pos = CreateBlock.blocks[item.pos.y, item.pos.x].worldPos;
            
            Batch(pos, item.dir, (int)item.type);
        }
    }

    private void CreateEnemy(Transform parent, Vector3 pos) {
        GameObject enemyAttack = new GameObject();
        enemyAttack.transform.position = parent.position + pos;
        enemyAttack.transform.parent = parent;
        enemyAttack.tag = "EnemyAttack";

        CircleCollider2D eAtkCol = enemyAttack.AddComponent<CircleCollider2D>();
        eAtkCol.isTrigger = true;
        eAtkCol.radius = 0.03f;
    }

    public void EndCheck() {
        foreach(CircleCollider2D item in enemyColList) {
            item.enabled = true;
        }
    }
}
