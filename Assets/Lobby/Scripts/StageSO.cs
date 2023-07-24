using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StageMap", fileName = "Stage_domi")]
public class StageSO : ScriptableObject
{
    [SerializeField] Vector2 playerCoord;
    [SerializeField] EnemyInfo[] enemys;
}
