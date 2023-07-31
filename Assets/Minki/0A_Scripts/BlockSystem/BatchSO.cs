using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BatchObject
{
    public Vector2Int pos;
    public ObjectType type;
    public EnemyDir dir;
}

[CreateAssetMenu(menuName = "SO/BatchSO")]
public class BatchSO : ScriptableObject
{
    public BatchObject[] batchObject;
}
