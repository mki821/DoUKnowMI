using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStage : MonoBehaviour
{
    private CreateBlock _createBlock;

    public void StartStage() {
        Block[,] blocks = _createBlock.Create();
    }
}
