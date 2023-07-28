using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchCharacter : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    public void Batch(Vector2 pos, Vector2 dir) {
        GameObject obj = new GameObject();
        obj.transform.position = pos;

        SpriteRenderer objSpr = obj.AddComponent<SpriteRenderer>();
        objSpr.sortingLayerName = "Character";
        objSpr.sortingOrder = 5;
    }
}
