using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CreateBlock : MonoBehaviour
{
    [SerializeField] int stageInfo;
    public BatchSO batchSO;

    public static Block[,] blocks = new Block[16, 16];

    [SerializeField] private Block block;
    [SerializeField] private Sprite[] blockSprites;

    [SerializeField] private BatchCharacter _batchCharacter;
    [SerializeField] private SlimeMove _slimeMove;

    private BlockManager blockManager;

    Sequence seq;

    private void Awake() {
        blockManager = BlockManager.instance;
        string map_json = Resources.Load("StageMap/" + stageInfo).ToString();
        batchSO = _batchCharacter.batchSO = DomiBatch.ConvertToBatchSO(map_json);
        seq = DOTween.Sequence();
    }

    public Block[,] Create() {
        int blockCount = batchSO.blockCount;

        float size = blockManager.size / blockCount;
        float width = BlockWidth() * 1.1f;
        BlockManager.instance.tileSize = width;
        float half = width * (blockCount - 1) / 2;

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
        StartCoroutine(Diagonal(blockCount, width, size, offset));
        
        return blocks;
    }

    private IEnumerator Diagonal(int blockCount, float width, float size, Vector2 offset) {
        int currentX = 0, currentY = 0;
        int tileSprite = 0;

        for (int y = 0; y < blockCount; y++) {
            currentY = y;
            for (int x = 0; x <= y; x++) {
                Block tile = Block(width, x, currentY, offset, tileSprite);
                blocks[currentY, x] = tile;
                currentY--;
            }
            tileSprite = tileSprite == 0 ? 1 : 0;
            yield return new WaitForSeconds(0.1f);
        }

        for (int x = 1; x < blockCount; x++) {
            currentX = x;
            for (int y = blockCount - 1; y > x - 1; y--) {
                Block tile = Block(width, currentX, y, offset, tileSprite);
                blocks[y, currentX] = tile;
                currentX++;
            }
            tileSprite = tileSprite == 0 ? 1 : 0;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.6f);
        EndCreate();
    }

    private Block Block(float width, int x, int y, Vector2 offset, int spriteNum) {
        float size = BlockManager.instance.tileSize;

        GameObject obj = new GameObject();
        obj.transform.parent = transform;
        obj.transform.position = new Vector2(width * x, width * y) + offset;
        
        SpriteRenderer objSpr = obj.AddComponent<SpriteRenderer>();
        objSpr.sprite = blockSprites[spriteNum];

        obj.transform.localScale = Vector3.one * size * 1.2f;
        seq.Join(obj.transform.DOScale(Vector3.one * size, 0.6f).SetEase(Ease.InQuad));

        Block tile = obj.AddComponent<Block>();
        tile.worldPos = obj.transform.position;
        tile.pos = new Vector2Int(x, y);

        return tile;
    }

    private void EndCreate() {
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
        _batchCharacter.EndCheck();
        _slimeMove.Move();
    }
}
