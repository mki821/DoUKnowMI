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
    [SerializeField] private BatchSO _batchSO;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private RuntimeAnimatorController[] _animators;

    public void Batch(Vector2 pos, EnemyDir dir, int type) {
        GameObject obj = new GameObject();
        obj.transform.position = pos;

        SpriteRenderer objSpr = obj.AddComponent<SpriteRenderer>();
        objSpr.sprite = _sprites[type];
        objSpr.sortingLayerName = "Character";
        objSpr.sortingOrder = 5;

        Animator objAnim = obj.AddComponent<Animator>();
        objAnim.runtimeAnimatorController = _animators[type];

        if(type == 0) {
            obj.AddComponent<SlimeMove>();
        }
    }

    public void BatchAll() {
        foreach(var item in _batchSO.batchObject) {
            Vector2 pos = CreateBlock.blocks[item.pos.y, item.pos.x].worldPos;
            
            Batch(pos, item.dir, (int)item.type);
        }
    }
}
