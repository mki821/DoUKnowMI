using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance = null;
    
    [Range(4, 16)] public int blockCount = 8;
    public float size = 8f;

    private void Awake() {
        if(instance == null) instance = this;
    }
}
