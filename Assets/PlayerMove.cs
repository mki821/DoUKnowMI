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
        transform.position = TileManager.GetBlockToCoords(new TileWay(4, 1)).transform.position;
    }

    public void Move(Vector3 pos){
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("EnemyAttack")){
            //Time.timeScale = 0;
            Debug.Log("플레이어 죽음");
        }
        else {
            Time.timeScale = 1;
            CameraManager.SlowCameraDisable();
            Destroy(other.gameObject);
        }
    }
}
