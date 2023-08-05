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

public class DomiBatch {
    class TileData {
        public string character;
        public int rotate;
    }
    class JsonData {
        public string projectName;
        public int size;
        public int[] player;
        public Dictionary<string, TileData> data;
    }
    public static BatchSO ConverToBatchSO(string jsondata) {
        var decode = LitJson.JsonMapper.ToObject<JsonData>(jsondata);

        BatchSO _batch = new();
        _batch.batchObject = new BatchObject[decode.data.Count];

        // player 위치 누락, 타일 사이즈 누락
        
        ///////////////// batchObject
        int loop = 0;
        foreach (KeyValuePair<string, TileData> enemy in decode.data)
        {
            string[] SplitCoords = enemy.Key.Split(",");
            _batch.batchObject[loop] = new() {
                pos = new(int.Parse(SplitCoords[0]), int.Parse(SplitCoords[1])),
                type = (ObjectType)System.Enum.Parse(typeof(ObjectType), enemy.Value.character),
                dir = (EnemyDir)enemy.Value.rotate
            };

            loop++;
        }
        
        return _batch;
    }
}