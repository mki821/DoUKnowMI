using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderingTest : MonoBehaviour
{
    private bool Ready = false;
    private List<TileWay> Selects = new();
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        TileCreate.TileCreateFinish += OnTileCreated;
    }

    private void OnDestroy()
    {
        TileCreate.TileCreateFinish -= OnTileCreated; // 리스너 해제 해야됭
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || !Ready) return;
        // 마우스 눌렀따

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject hit_entity = Physics2D.Raycast(mousePos, Vector2.zero, LayerMask.GetMask("Tile")).collider?.gameObject;
        
        if (hit_entity == null) return; // 없넹

        TileWay TileCoords = TileManager.GetCoordsToBlock(hit_entity);
        if (TileCoords == null) return;

        if (Selects.Count > 0 && (
            ( /* 전꺼 선택한거랑 같음 */
                Selects[Selects.Count - 1].x == TileCoords.x
                && Selects[Selects.Count - 1].y == TileCoords.y
            )
            /* Angle 체크 실패면 X,Y 각각 같은지 비교함 */
            || (!AngleCheck(Selects[^1], TileCoords) && Selects[^1].x != TileCoords.x && Selects[^1].y != TileCoords.y)
        )) return;


        print($"{TileCoords.x}, {TileCoords.y}");
        SetWayCoords(TileCoords);
    }

    void SetWayCoords(TileWay coords)
    {
        var BlockEntity = TileManager.GetBlockToCoords(coords);
        if (BlockEntity == null)
        {
            Debug.LogError($"[LineRenderingTest] 경로를 지정할 수 없습니다. : {coords.x}, {coords.y}");
            return;
        }

        Selects.Add(coords);
        _lineRenderer.positionCount = Selects.Count;
        _lineRenderer.SetPosition(Selects.Count - 1, BlockEntity.transform.position);
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Collider2D hit = Physics2D.Raycast(mousePos, Vector2.zero).collider;

    //        if (hit != null && (pos.Count == 0 ||
    //           (hit.transform.position.x == pos[count - 1].x ||
    //           hit.transform.position.y == pos[count - 1].y || AngleCheck(hit))))
    //        {
    //            if (hit.gameObject.CompareTag("Tile"))
    //            {
    //                if (_lineRenderer.positionCount == count)
    //                {
    //                    _lineRenderer.positionCount++;
    //                }

    //                _lineRenderer.SetPosition(count, hit.transform.position);
    //                pos.Add(hit.transform.position);
    //                count++;
    //            }
    //        }
    //    }
    //}

    private bool AngleCheck(TileWay a, TileWay b)
    {
        Vector2 vec = new Vector2(b.x, b.y) - new Vector2(a.x, a.y);

        float x = Mathf.Abs(vec.x);
        float y = Mathf.Abs(vec.y);

        Vector2 v2 = new Vector2(x, y).normalized - Vector2.zero /* vector zero는 왜 뺴는거징 ＼（〇_ｏ）／ */;

        return Mathf.Round(Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg) == 45;
    }

    private bool True() => true; // 민기 코드

    // 이벤트 리스너
    void OnTileCreated()
    {
        Ready = true;
        SetWayCoords(new TileWay(4, 1)); // 유저 좌표 (임시, 초기값)
    }
}
