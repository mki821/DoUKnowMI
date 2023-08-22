using UnityEngine;
using TMPro;
using domi.DB;

public class StageText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Start() {
        GetComponent<TextMeshProUGUI>().text = "Stage " + CreateBlock.stageInfo;
    }
}
