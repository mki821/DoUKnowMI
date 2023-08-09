using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlock : MonoBehaviour
{
    public BatchSO batchSO;

    public static Block[,] blocks = new Block[16, 16];

    [SerializeField] private Block block;


    [SerializeField] private BatchCharacter _batchCharacter;
    [SerializeField] private SlimeMove _slimeMove;

    private BlockManager blockManager;

    private void Awake() {
        blockManager = BlockManager.instance;
    }

    public Block[,] Create() {
        int _blockCount = batchSO.blockCount;
        float _size = blockManager.size;

        block.transform.localScale = Vector3.one * (_size / _blockCount);
        float width = BlockWidth() * 1.1f;
        BlockManager.instance.tileSize = width;
        float half = width * (_blockCount - 1) / 2;

        Vector2 offset = new Vector2(-half, -half);

        //Modify the creation method
        // for(int x = 0; x < _blockCount; x++) {
        //     for (int y = 0; y < _blockCount; y++) {
        //         Block tile = Instantiate(block, transform);
        //         tile.transform.position = new Vector2(width * x, width * y) + offset;
        //         tile.worldPos = tile.transform.position;
        //         tile.pos = new Vector2Int(x, y);
        //         blocks[y, x] = tile;
        //     }
        // }
        StartCoroutine(Diagonal(_blockCount, width, offset));
        
        return blocks;
    }

    private IEnumerator Diagonal(int blockCount, float width, Vector2 offset) {
        int currentX = 0, currentY = 0;

        for (int y = 0; y < blockCount; y++) {
            currentY = y;
            for (int x = 0; x <= y; x++) {
                Block tile = Instantiate(block, transform);
                tile.transform.position = new Vector2(width * x, width * currentY) + offset;
                tile.worldPos = tile.transform.position;
                tile.pos = new Vector2Int(x, currentY);
                blocks[currentY, x] = tile;
                currentY--;
                yield return new WaitForSeconds(0.05f);
            }
        }

        for (int x = 1; x < blockCount; x++) {
            currentX = x;
            for (int y = blockCount - 1; y > x - 1; y--) {
                Block tile = Instantiate(block, transform);
                tile.transform.position = new Vector2(width * currentX, width * y) + offset;
                tile.worldPos = tile.transform.position;
                tile.pos = new Vector2Int(currentX, y);
                blocks[y, currentX] = tile;
                currentX++;
                yield return new WaitForSeconds(0.05f);
            }
        }

        EndCreate();
    }

    private void EndCreate() {
        _batchCharacter.batchSO = batchSO;
        _batchCharacter.BatchAll();
        _slimeMove = BlockManager.instance.slimeMove;
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

    public void Move() {
        _slimeMove.Move();
    }
}
