using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderingTest : MonoBehaviour
{
    public static LineRenderingTest instance = null;
    [SerializeField] private PlayerMove player;
    private bool Ready = false;
    private List<TileWay> Selects = new();
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        if(instance == null) instance = this;

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
        GameObject hit_entity = Physics2D.Raycast(mousePos, Vector2.zero, 0, LayerMask.GetMask("Tile")).collider?.gameObject;
        
        if (hit_entity == null) return; // 없넹

        TileWay TileCoords = TileManager.GetCoordsToBlock(hit_entity);
        if (TileCoords == null || TileManager.IsEnemyToCoords(TileCoords) != null /* 선택한 곳에 적이 있남? */) return;

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

        if(Selects.Count != 0)
            foreach (var mki in TileWay.GetWays(Selects[Selects.Count - 1], coords))
            {
                Debug.Log($"{mki.x}, {mki.y}");
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
    void OnTileCreated() => StartCoroutine(ReadyWaiter());

    IEnumerator ReadyWaiter()
    {
        TileScreenAuto tileScreenAuto = GameObject.FindAnyObjectByType<TileScreenAuto>();

        while (!tileScreenAuto.FinishPosition()) yield return null;

        // 완료!
        Ready = true;
        SetWayCoords(new TileWay(4, 1)); // 유저 좌표 (임시, 초기값)
        TileCreate.TileCameraFinish?.Invoke();
    }

    public void EndEndEnd(){
        StartCoroutine("EndMoveEndMove");
    }

    private IEnumerator EndMoveEndMove(){
        for (int i = 0; i < Selects.Count; i++)
        {
            var item = Selects[i];

            if (i > 0) {
                var Last_Coords = Selects[i - 1];
                foreach (TileWay TileCoords in TileWay.GetWays(Last_Coords, item))
                    WayEnemyActive(TileCoords);
                WayEnemyActive(item); // GetWays는 마지막 좌표는 안주기 때문에 직접 해줘야함
            }

            Vector2 tarPos = TileManager.GetBlockToCoords(item).transform.position;
            Vector2 curPos = player.transform.position;
            float prev_distance = Vector2.Distance(tarPos, curPos);
            float t = 0;
            while (Vector2.Distance(player.transform.position, tarPos) > 0.1f) {
                t += Time.deltaTime / prev_distance;
                player.Move(Vector2.Lerp(player.transform.position, tarPos, t * Time.timeScale));
                yield return null;
            }
            
            // 다 하면 정직(확)한 자리로 감
            player.Move(tarPos);
        }
    }

    // 지나가는 길에 적이 있남?
    void WayEnemyActive(TileWay Coords) {
        GameObject Enemy = TileManager.IsEnemyToCoords(Coords);
        if (Enemy == null) return;

        StartCoroutine(RegisterSlowMotion(Enemy.transform));
    }
    
    IEnumerator RegisterSlowMotion(Transform EnemyCoordsTrm) {
        // 일단 거리가 좁아질때까지 기다리자.
        if (EnemyCoordsTrm.CompareTag("EnemyAttack")){
            yield return new WaitUntil(() => Vector3.Distance(player.transform.position, EnemyCoordsTrm.position) < 0.05f);

            StopCoroutine("EndMoveEndMove");
            Time.timeScale = 0;
        }
        else{
            yield return new WaitUntil(() => Vector3.Distance(player.transform.position, EnemyCoordsTrm.position) < 1.2f && Time.timeScale == 1);
            Time.timeScale = 0.05f;
            CameraManager.SlowCameraEnable(EnemyCoordsTrm.position);
        }
    }
}
