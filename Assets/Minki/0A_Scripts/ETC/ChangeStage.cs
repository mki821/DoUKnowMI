using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeStage : MonoBehaviour
{
    public void SceneChange(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void NextStage() {
        CreateBlock.stageInfo++;
        int stageNum = CreateBlock.stageInfo;
        CreateBlock.stageTileType = stageNum % 20 == 0 ? stageNum / 20 - 1 : stageNum / 20;
        Debug.Log(stageNum % 20 == 0 ? stageNum / 20 - 1 : stageNum / 20);
        SceneManager.LoadScene("DAZB3");
    }
}
