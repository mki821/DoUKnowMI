using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCreate : MonoBehaviour
{
    [Header("강민기")]
    [SerializeField, Tooltip("으하하하하"), Range(4, 16)]
    private int BlockSize = 4;
    [SerializeField]
    private GameObject blockPrefab;

    public static UnityEngine.Events.UnityAction TileCreateFinish;


    private float domiTime = 0.01f;

    private void Awake()
    {
        TileManager.SetBlockSize(BlockSize);
    }

    private void Start()
    {
        StartCoroutine(a());
    }

    private IEnumerator a()
    {
        // 강민기 코드 고등학생한테 다 털렸죠? ㅋㅋㅋㅋㅋㅋ
        int Current_X;
        int Current_Y;

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
                    var Block = Instantiate(blockPrefab, transform.position + new Vector3(Current_X, Current_Y) * 0.9f, Quaternion.identity, transform);
                    TileManager.SetBlock(Current_X, Current_Y, Block);

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
                    var Block = Instantiate(blockPrefab, transform.position + new Vector3(Current_X, Current_Y) * 0.9f, Quaternion.identity, transform);
                    TileManager.SetBlock(Current_X, Current_Y, Block);

                    Current_X++;
                    Current_Y--;

                    yield return new WaitForSeconds(domiTime);
                }
            }
            count++;
        }

        TileCreateFinish.Invoke(); // 준비 완료!!!
    }
}
