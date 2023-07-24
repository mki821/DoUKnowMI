using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static StageSO stageData { get; private set; }

    private void Awake() {
        stageData = null; // 초기화
    }

    
}
