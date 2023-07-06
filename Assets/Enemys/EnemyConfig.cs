using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : MonoBehaviour
{
    [SerializeField] Vector2[] _AttackCoords;
    public TileWay[] AttackCoords { get; private set; }

    private void Awake() {
        // Vector2 -> TileWay 변환
        AttackCoords = new TileWay[_AttackCoords.Length];
        for (int i = 0; i < _AttackCoords.Length; i++)
        {
            var data = _AttackCoords[i];
            AttackCoords[i] = new TileWay((int)data.x, (int)data.y);
        }
    }
}
