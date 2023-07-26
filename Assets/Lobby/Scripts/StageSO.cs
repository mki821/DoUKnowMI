using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StageMap", fileName = "Stage_domi")]
public class StageSO : ScriptableObject
{
    public Vector2 playerCoord;
    [Range(4,16)] public int BlockSize;
    public EnemyInfo[] enemys;
}
