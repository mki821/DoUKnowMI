using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class EnemyInfo {
    public GameObject Character;
    public Vector2 Coords;
}

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] EnemyInfo[] EnemyList;

    private void Awake() {
        TileCreate.TileCameraFinish += EnemySpawnStart;
    }
    private void OnDestroy() {
        TileCreate.TileCameraFinish -= EnemySpawnStart;
    }

    void EnemySpawnStart() {
        foreach (var EnemyData in EnemyList)
        {
            TileWay EnemyCoords = new TileWay((int)EnemyData.Coords.x, (int)EnemyData.Coords.y);
            EnemyConfig Enemy_Config = Instantiate( EnemyData.Character, TileManager.GetBlockToCoords(EnemyCoords).transform.position, Quaternion.identity ).GetComponent<EnemyConfig>();

            foreach (TileWay Coords in Enemy_Config.AttackCoords)
            {
                // 플레이어가 죽는거 콜라이더 넣어야함
            }
        }
    }
}
