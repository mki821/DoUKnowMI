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

    [SerializeField] private GameObject clearPanel;
    [SerializeField] private GameObject failPanel;

    private Camera _cam;
    private ChangeStage _changeStage;

    public int BlockCount {
        get => blockCount;
        set {
            blockCount = value;
            SetTileSize();
        }
    }

    private void Awake() {
        if(instance == null) instance = this;

        _cam = Camera.main;
        _changeStage = (ChangeStage)GetComponent("ChangeStage");

        size = _cam.ViewportToWorldPoint(new Vector2(0.8f, 0)).x - _cam.ViewportToWorldPoint(new Vector2(0, 0)).x;

        SetTileSize();
    }

    private void SetTileSize() {
        tileSize = size / BlockCount;
    }

    public void EndStage(bool clear) {
        if (clear) _changeStage.NextStage();
        else _changeStage.SceneChange("DAZB3");
    }

    public void ShowPanel(bool clear) {
        if (clear) clearPanel.SetActive(true);
        else failPanel.SetActive(true);
    }
}
