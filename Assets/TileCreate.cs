using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCreate : MonoBehaviour
{
    [Header("悪肯奄")]
    [SerializeField, Tooltip("生馬馬馬馬"), Range(4, 16)]
    private int BlockSize = 4;
    [SerializeField]
    private GameObject blockPrefab;
    private int Current_X = 0;
    private int Current_Y = 0;

    private float domiTime = 0.01f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(a());

        }
    }

    /*private IEnumerator TileCreate()
    {
        while(Current_Y < BlockSize)
        {
            domiTime -= 0.01f;
            Debug.Log($"{Current_X} | {Current_Y}");
            Instantiate(blockPrefab, new Vector3(Current_X, Current_Y) * 0.9f, Quaternion.identity);

            Current_X++;

            if (Current_X > BlockSize - 1)
            {
                Current_X = 0;
                Current_Y++;
            }

            yield return new WaitForSeconds(domiTime);
        }
    }*/

    private IEnumerator a()
    {
        int b = BlockSize * 2 - 1;
        int count = 0;
        while (count < b)
        {
            if(b / 2 + 1 > count)
            {
                Current_X = 0;
                Current_Y = count - Current_X;
                while (Current_Y >= 0)
                {
                    Instantiate(blockPrefab, transform.position + new Vector3(Current_X, Current_Y) * 0.9f, Quaternion.identity, transform);

                    Current_X++;
                    Current_Y--;

                    yield return new WaitForSeconds(domiTime);
                }
            }
            else
            {
                Current_X = count - (b / 2);
                Current_Y = BlockSize - 1;
                while (Current_X <= BlockSize - 1)
                {
                    Instantiate(blockPrefab, transform.position + new Vector3(Current_X, Current_Y) * 0.9f, Quaternion.identity, transform);

                    Current_X++;
                    Current_Y--;

                    yield return new WaitForSeconds(domiTime);
                }
            }
            count++;
        }
    }
}
