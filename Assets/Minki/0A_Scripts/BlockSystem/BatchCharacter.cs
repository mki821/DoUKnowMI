using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType {
    Knight = 0,
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
    }

    public void BatchAll() {
        foreach(var item in _batchSO.batchObject) {
            Vector2 pos = Vector2.zero;
            foreach(var b in CreateBlock.blocks) {
                if(b == null) continue;
                if(item.pos == b.pos) {
                    pos = b.worldPos;
                    break;
                }
            }
            
            Batch(pos, item.dir, (int)item.type);
        }
    }
}
