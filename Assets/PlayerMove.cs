using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private void Awake() {
        TileCreate.TileCameraFinish += SetPos;
    }

    private void OnDestroy() {
        TileCreate.TileCameraFinish -= SetPos;
    }

    public void SetPos(){
        transform.position = TileManager.GetBlockToCoords(StageLoader.stageData == null ? new TileWay(4, 1) : new TileWay((int)StageLoader.stageData.playerCoord.x, (int)StageLoader.stageData.playerCoord.y)).transform.position;
    }

    public void Move(Vector3 pos){
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("EnemyAttack")) return;
        Time.timeScale = 1;
        CameraManager.SlowCameraDisable();
        GetComponent<Animator>().SetBool("isAttack", false);
        Destroy(other.gameObject);
    }

    public void SetTime(float t){
        Time.timeScale = t;
    }
}
