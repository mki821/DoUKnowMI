using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    public static StageSO stageData { get; private set; }

    private void Awake() {
        stageData = null; // 초기화
        Application.targetFrameRate = 60; // 60fps 제한
    }

    public void GoStage(int stage) {
        StageSO _stageData = Resources.Load("StageMap/"+stage.ToString()) as StageSO;
        if (_stageData == null) {
            throw new System.Exception("[StageLoader] 스테이지 데이터가 없습니다. ("+stage.ToString()+")");
        }

        stageData = _stageData;
        
        // 씬 로드
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync("SampleScene");
        // asyncScene.allowSceneActivation = false;
    }
}
