using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCreate : MonoBehaviour
{
    [Header("���α�")]
    [SerializeField, Tooltip("����������"), Range(4, 16)]
    private int BlockSize = 4;
    [SerializeField]
    private GameObject blockPrefab;

    public static UnityEngine.Events.UnityAction TileCreateFinish;
    public static UnityEngine.Events.UnityAction TileCameraFinish;

    private float domiTime = 0.01f;

    private void Start()
    {
        if (StageLoader.stageData != null)
            BlockSize = StageLoader.stageData.BlockSize;

        TileManager.SetBlockSize(BlockSize);

        StartCoroutine(a());
    }

    private IEnumerator a()
    {
        // ���α� �ڵ� �����л����� �� �з���? ������������
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

        TileCreateFinish.Invoke(); // �غ� �Ϸ�!!!
    }
}
