using UnityEngine;
using UnityEngine.SceneManagement;
using domi.DB;

public class ChangeStage : MonoBehaviour
{
    private DBstruct _db;

    public void SceneChange(string sceneName) {
        SoundManager.instance.BntClickSound();
        SceneManager.LoadScene(sceneName);
    }

    public void NextStage() {
        _db = DBmanager.GetData();
        SoundManager.instance.BntClickSound();
        if (_db.health > 0) {
            _db.health--;
            int stageNum = CreateBlock.stageInfo;

            if (stageNum % 20 == 1) {
                if (stageNum >= _db.keyUnlock) {
                    CreateBlock.stageInfo++;
                    CreateBlock.stageTileType = stageNum % 20 == 0 ? stageNum / 20 - 1 : stageNum / 20;
                    SceneManager.LoadScene("DAZB3");
                }
                else if (_db.keyAmount - 5 >= 0) {
                    _db.keyAmount -= 5;
                    CreateBlock.stageInfo++;
                    CreateBlock.stageTileType = stageNum % 20 == 0 ? stageNum / 20 - 1 : stageNum / 20;
                    SceneManager.LoadScene("DAZB3");
                }
                else {
                    SceneManager.LoadScene("Main");
                }
            }
            else{
                CreateBlock.stageInfo++;
                CreateBlock.stageTileType = stageNum % 20 == 0 ? stageNum / 20 - 1 : stageNum / 20;
                SceneManager.LoadScene("DAZB3");
            }

            DBmanager.Save();
        }
        else {
            SceneManager.LoadScene("Main");
        }
    }
}
