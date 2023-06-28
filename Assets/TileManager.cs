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

    // ºí·° -> ÁÂÇ¥
    public static TileWay GetCoordsToBlock(GameObject block)
    {
        for (int y = 0; y < Blocks.GetLength(0); y++)
            for (int x = 0; x < Blocks.GetLength(1); x++)
                if (Blocks[y, x] == block) return new TileWay(x, y);

        return null;
    }

    // ÁÂÇ¥ -> ºí·°
    public static GameObject GetBlockToCoords(TileWay coords) => Blocks[coords.y, coords.x];
}
