using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScreenAuto : MonoBehaviour
{
    [SerializeField, Range(1, 10), Tooltip("얼마나 빠르게 조정할꺼임?")] float Speed = 5;
    Vector2 FinishVec = Vector2.zero; // 목표 좌표 (그니까.. 음 이 좌표로 이동할려고 할꺼임)

    void Update()
    {
        Vector2 All_Size = Vector2.zero;

        int index = 0;
        while (index < transform.childCount)
        {
                                                            /* 로컬좌표여야함 (안에 있는거라서!!! >__-) */
            All_Size += (Vector2)transform.GetChild(index).localPosition; // 안에 있는 블럭들 좌표 더해더해
            index++;
        }

        // 다 더했다면
        FinishVec = -(All_Size / transform.childCount); // finish 좌표 설정
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, FinishVec, Time.fixedDeltaTime * Speed);
    }
}
