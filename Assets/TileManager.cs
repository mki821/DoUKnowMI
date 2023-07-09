using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static GameObject[,] Blocks { get; private set; }

    public static void SetBlockSize(int value)
    {
        Blocks = new GameObject[value, value];
    }
    public static void SetBlock(int x, int y, GameObject entity)
    {
        Blocks[y, x] = entity;
    }

    // ���� -> ��ǥ
    public static TileWay GetCoordsToBlock(GameObject block)
    {
        for (int y = 0; y < Blocks.GetLength(0); y++)
            for (int x = 0; x < Blocks.GetLength(1); x++)
                if (Blocks[y, x] == block) return new TileWay(x, y);

        return null;
    }

    // ��ǥ -> ����
    public static GameObject GetBlockToCoords(TileWay coords) => Blocks[coords.y, coords.x];

    ////////////// �� ����
    public static GameObject IsEnemyToCoords(TileWay coords) {
        Transform TileTrans = TileManager.GetBlockToCoords(coords).transform;
        Vector3 TileScaleHalf = TileTrans.localScale / 2;
        
        Collider2D hit = Physics2D.OverlapArea(TileTrans.position - TileScaleHalf, TileTrans.position + TileScaleHalf, LayerMask.GetMask("Enemy"));
        return hit == null ? null : hit.gameObject;
    }
}
