using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI sub;

    bool? result = null;

    public void ShowUI(bool clear) {
        result = clear;
        
        title.text = clear ? "Clear!" : "<color=red>Fail!</color>";
        sub.text  = clear ? "아무키나 눌러 Lobby로 이동하세요." : "아무키나 눌러 다시 시도하세요.";
        gameObject.SetActive(true);
    }


    bool holdingDown;

    private void Update() {
        if (result == null) return;

        if (Input.anyKey) holdingDown = true;
        if (!Input.anyKey && holdingDown) {
            SceneManager.LoadScene(result.Value ? "Lobby" : "SampleScene");
        }
    }
}
