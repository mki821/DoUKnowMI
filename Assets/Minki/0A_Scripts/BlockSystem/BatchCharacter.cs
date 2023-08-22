using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using domi.DB;

public enum ObjectType {
    Slime = 0,
    key,
    sinchamgisa,
    archer,
    shield,
    spear
}

public enum EnemyDir {
    Down = 0,
    Right,
    Up,
    Left
}

public class BatchCharacter : MonoBehaviour
{
    public static List<CircleCollider2D> enemyColList = new List<CircleCollider2D>();
    [HideInInspector] public BatchSO batchSO;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private RuntimeAnimatorController[] _animators;

    private DrawLine _drawLine;

    private DBstruct _db;

    private void Awake() {
        _drawLine = GameObject.Find("LineRenderer").GetComponent<DrawLine>();

        _db = DBmanager.GetData();
    }

    public void Batch(Vector2 pos, EnemyDir dir, int type) {
        GameObject obj = new GameObject();
        obj.transform.position = pos;
        obj.transform.localScale = Vector3.one * BlockManager.instance.tileSize * 1.5f;

        SpriteRenderer objSpr = obj.AddComponent<SpriteRenderer>();
        objSpr.sprite = _sprites[type];
        objSpr.sortingLayerName = "Character";
        objSpr.sortingOrder = 5;

        CircleCollider2D objCol = obj.AddComponent<CircleCollider2D>();
        objCol.offset = new Vector2(0, 0);
        objCol.radius = BlockManager.instance.tileSize / 5f;

        Animator objAnim = obj.AddComponent<Animator>();
        objAnim.runtimeAnimatorController = _animators[type == 0 ? type : type - 1];

        if(type == 0) {
            BlockManager.instance.slimeMove = obj.AddComponent<SlimeMove>();
            obj.AddComponent<SlimeAttack>();
            _drawLine.SetLinePos(obj.transform.position);

            Rigidbody2D objRig = obj.AddComponent<Rigidbody2D>();
            objRig.gravityScale = 0;
        }
        else if(type == 1) {
            if (_db.takenKeyStage.Contains(CreateBlock.stageInfo)) Destroy(obj);
            else {
                obj.tag = "Key";
                Destroy(objAnim);
            }
        }
        else if(type > 1) {
            enemyColList.Add(objCol);
            obj.tag = "Enemy";
            obj.layer = 7;
            obj.transform.position += Vector3.up * BlockManager.instance.tileSize * 0.13f;
            objCol.isTrigger = true;
            objAnim.SetFloat("Idle", (int)dir);
            if ((int)dir == 1)
                obj.GetComponent<SpriteRenderer>().flipX = true;
            float tSize = BlockManager.instance.tileSize;
            switch(type) {
                case 3:
                    switch((int)dir) {
                        case 0:
                            CreateEnemy(obj.transform, new Vector3(0, -tSize));
                            break;
                        case 1:
                            CreateEnemy(obj.transform, new Vector3(tSize, 0));
                            break;
                        case 2:
                            CreateEnemy(obj.transform, new Vector3(0, tSize));
                            break;
                        case 3:
                            CreateEnemy(obj.transform, new Vector3(-tSize, 0));
                            break;
                    }
                    break;
                case 4:
                    switch((int)dir) {
                        case 0:
                            CreateEnemy(obj.transform, new Vector3(-tSize, -tSize));
                            CreateEnemy(obj.transform, new Vector3(0, -tSize));
                            CreateEnemy(obj.transform, new Vector3(tSize, -tSize));
                            break;
                        case 1:
                            CreateEnemy(obj.transform, new Vector3(tSize, tSize));
                            CreateEnemy(obj.transform, new Vector3(tSize, 0));
                            CreateEnemy(obj.transform, new Vector3(tSize, -tSize));
                            break;
                        case 2:
                            CreateEnemy(obj.transform, new Vector3(tSize, tSize));
                            CreateEnemy(obj.transform, new Vector3(0, tSize));
                            CreateEnemy(obj.transform, new Vector3(-tSize, tSize));
                            break;
                        case 3:
                            CreateEnemy(obj.transform, new Vector3(-tSize, tSize));
                            CreateEnemy(obj.transform, new Vector3(-tSize, 0));
                            CreateEnemy(obj.transform, new Vector3(-tSize, -tSize));
                            break;
                    }
                    break;
                case 5:
                    switch((int)dir) {
                        case 0:
                            CreateEnemy(obj.transform, new Vector3(0, tSize));
                            CreateEnemy(obj.transform, new Vector3(0, -tSize));
                            break;
                        case 1:
                            CreateEnemy(obj.transform, new Vector3(tSize, 0));
                            CreateEnemy(obj.transform, new Vector3(-tSize, 0));
                            break;
                        case 2:
                            CreateEnemy(obj.transform, new Vector3(0, tSize));
                            CreateEnemy(obj.transform, new Vector3(0, -tSize));
                            break;
                        case 3:
                            CreateEnemy(obj.transform, new Vector3(tSize, 0));
                            CreateEnemy(obj.transform, new Vector3(-tSize, 0));
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
        enemyAttack.transform.localScale = Vector3.one * BlockManager.instance.tileSize;
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
