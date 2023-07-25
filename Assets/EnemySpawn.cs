using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInfo {
    public GameObject Character;
    public Vector2 Coords;
}

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] EnemyInfo[] EnemyList;
    [SerializeField] Sprite WarningSprite;
    [SerializeField] Color WarningColor;

    private void Awake() {
        TileCreate.TileCameraFinish += EnemySpawnStart;
    }
    private void OnDestroy() {
        TileCreate.TileCameraFinish -= EnemySpawnStart;
    }

    private void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60; // 60fps 제한

        if (StageLoader.stageData != null)
            EnemyList = StageLoader.stageData.enemys;
    }

    void EnemySpawnStart() {
        foreach (var EnemyData in EnemyList)
        {
            TileWay EnemyCoords = new TileWay((int)EnemyData.Coords.x, (int)EnemyData.Coords.y);
            EnemyConfig Enemy_Config = Instantiate( EnemyData.Character, TileManager.GetBlockToCoords(EnemyCoords).transform.position, Quaternion.identity ).GetComponent<EnemyConfig>();

            print("------- Enemy ------");
            foreach (TileWay Coords in Enemy_Config.AttackCoords)
            {
                // 플레이어가 죽는거 콜라이더 넣어야함
                var diff = EnemyCoords + Coords;
                Vector3 RealCoords = TileManager.GetBlockToCoords(diff).transform.position;

                GameObject AttackEntity = new GameObject("AttackPosition");
                AttackEntity.transform.parent = Enemy_Config.transform;
                AttackEntity.transform.position = RealCoords;
                AttackEntity.transform.localScale = new(1,1); // 옵젝 부모 바꾸면 스케일 바꿔짐 ㅁㄴㅇㄹ
                AttackEntity.tag = "EnemyAttack";
                AttackEntity.layer = 7;

                // 콜라이더 생성해야지ㅣㅣㅣ (prefab으로 하는것보다 스끄립트로 하는게 더 최적화게 좋다함 [암튼 그럼])
                var AttackCollider = AttackEntity.AddComponent<CircleCollider2D>();
                AttackCollider.isTrigger = true;

                // 표시
                var attackSprite = AttackEntity.AddComponent<SpriteRenderer>();
                attackSprite.sprite = WarningSprite;
                attackSprite.color = WarningColor;
            }
        }
    }
}
