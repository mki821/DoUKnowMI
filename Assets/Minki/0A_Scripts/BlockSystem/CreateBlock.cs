using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlock : MonoBehaviour
{
    public Block[,] blocks = new Block[16, 16];

    [Range(4, 16)] [SerializeField] private int _blockCount = 8;

    [SerializeField] private Block block;

    public void Create() {
        float size = Mathf.Lerp(0.5f, 2, 8 / _blockCount);
        block.transform.localScale = Vector3.one * size;
        float width = BlockWidth() * 1.1f;
        float half = width * (_blockCount - 1) / 2;

        Vector2 offset = new Vector2(-half, -half);

        //Modify the creation method
        for(int x = 0; x < _blockCount; x++) {
            for (int y = 0; y < _blockCount; y++) {
                Block tile = Instantiate(block, transform);
                tile.transform.position = new Vector2(width * x, width * y) + offset;
                tile.worldPos = transform.position;
                tile.pos = new Vector2(x, y);
                blocks[y, x] = tile;
            }
        }
    }

    private float BlockWidth() {
        Vector2[] vertices = block.GetComponent<SpriteRenderer>().sprite.uv;

        return (block.transform.TransformPoint(vertices[0]) - block.transform.TransformPoint(vertices[3])).magnitude;
    }

    public void Clear() {
        foreach(Block item in blocks) {
            if(item != null) Destroy(item.gameObject);
        }

        blocks = new Block[16, 16];
    }
}
