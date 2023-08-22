using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance = null;
    
    [Range(4, 16)] public int blockCount = 8;
    public float size = 8f;

    public float tileSize = 0f;

    public SlimeMove slimeMove;

    public int BlockCount {
        get => blockCount;
        set {
            blockCount = value;
            SetTileSize();
        }
    }

    private void Awake() {
        if(instance == null) instance = this;

        size = Screen.width / 250;

        SetTileSize();
    }

    private void SetTileSize() {
        tileSize = size / BlockCount;
    }
}
