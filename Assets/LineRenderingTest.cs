using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderingTest : MonoBehaviour
{
    public static LineRenderingTest instance = null;
    public Animator _animator;
    [SerializeField] private PlayerMove player;
    [SerializeField] ResultScreen _resultScreen;
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
        if (TileCoords == null) return;
        GameObject EnemyHit = TileManager.IsEnemyToCoords(TileCoords);
        if (EnemyHit != null /* 선택한 곳에 적이 있남? */ && !EnemyHit.CompareTag("EnemyAttack") /* 적이 공격하는 곳이 아닌감자 */) return;

        // 선택 한 사이에 적들이 없으면 안댐
        if (Selects.Count > 0) {
            bool hasEnemy = false;
            foreach (var WayCoord in TileWay.GetWays(Selects[^1], TileCoords)) {
                GameObject WayEnemy = TileManager.IsEnemyToCoords(WayCoord);
                if (WayEnemy != null && !WayEnemy.CompareTag("EnemyAttack")) {
                    hasEnemy = true;
                    break;
                }
            }
            if (!hasEnemy) return;
        }

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
        print(TileManager.Blocks.Length);
        SetWayCoords(StageLoader.stageData == null ? new TileWay(4, 1) : new TileWay((int)StageLoader.stageData.playerCoord.x, (int)StageLoader.stageData.playerCoord.y)); // 유저 좌표 (임시, 초기값)
        TileCreate.TileCameraFinish?.Invoke();
    }

    public void EndEndEnd(){
        if (!Ready) return;
        StartCoroutine("EndMoveEndMove");
    }

    private IEnumerator EndMoveEndMove(){
        for (int i = 0; i < Selects.Count; i++)
        {
            var item = Selects[i];

            Vector2 tarPos = TileManager.GetBlockToCoords(item).transform.position;
            bool WillDie = false; // 죽을 예정

            if (i > 0) {
                var Last_Coords = Selects[i - 1];
                TileWay[] WayList = TileWay.GetWays(Last_Coords, item);

                // 첫번째 우회 막기
                if (WayList.Length > 0) {
                    GameObject Enemy = TileManager.IsEnemyToCoords(Last_Coords); // 플레이어 자리에 enemy가 있음??
                    GameObject Next_Enemy = TileManager.IsEnemyToCoords(WayList[0]); // 그 다음 자리에 있음?
                    if (Enemy != null && Next_Enemy != null && Enemy.CompareTag("EnemyAttack") && !Next_Enemy.CompareTag("EnemyAttack") && Enemy.transform.parent == Next_Enemy.transform) {
                        WillDie = true;
                        tarPos = Enemy.transform.position;
                    }
                }

                for (int k = 0; k < WayList.Length && !WillDie; k++)
                {
                    GameObject AttackEnemy = WayEnemyActive(WayList[k], k == 0 ? Last_Coords : WayList[k - 1]);
                    if (AttackEnemy != null) { // ㅓ.. 죽는다!!
                        tarPos = AttackEnemy.transform.position; // 마지막 좌표를 바꿈
                        WillDie = true;
                        break; // 더이상 안해도 됨
                    }
                }
                
                // 마지막 목표 지점에 Enemy가 공격하는지
                if (!WillDie && i+1 == Selects.Count) {
                    GameObject LastEnemy = TileManager.IsEnemyToCoords(item); // 마지막 자리에 적 attack이 있슴?
                    if (LastEnemy != null && LastEnemy.CompareTag("EnemyAttack")) {
                        WillDie = true;

                        // 예외 처리 / 사이에 있는 enemy 다 가지고 와서 마지막 목표 지점 Attack 주인이 있으면 취소함 (Attack 주인이 죽을 예정이라서 ㅎㅎ)
                        // -- 이 코드는 플레이어 죽는지 예측은 가능하나, 코드에 레이턴시? 가 좀 문제가 있음
                        foreach (TileWay WayCoord in WayList)
                        {
                            GameObject WayEnemy = TileManager.IsEnemyToCoords(WayCoord);
                            if (WayEnemy != null && LastEnemy.transform.parent == WayEnemy.transform) {
                                WillDie = false;
                                break;
                            }
                        }
                    }
                }
            }

            // 예측 안내 코드는 비활함
            if (WillDie)
                Debug.LogWarning("[domi-DEBUG] 플레이어가 죽을 예정입니다.");

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

            // 죽을 예정임!! 그리고 마지막 좌표 근데 적 실종햇다ㅏㅏ
            // [!] 이 코드는 플레이어가 죽는지 예측을 할 수 없음
            // if (WillDie && i+1 == Selects.Count && TileManager.IsEnemyToCoords(item) == null) {
            //     WillDie = false;
            // }

            // 다 이동함
            if (WillDie) {
                CameraManager.SlowCameraEnable(tarPos);
                print("[domi-DEBUG] 플레이어가 죽었습니다.");
                _resultScreen.ShowUI(false);

                //////////////////// 플레이어 죽는 임시 코드 ////////////////////
                yield return new WaitForSeconds(0.1f);
                // 플레이어 주겅
                float Delay = 5f;
                float tttt = 0;
                SpriteRenderer renderer = player.GetComponent<SpriteRenderer>();
                player.GetComponent<CircleCollider2D>().enabled = false;
                while (tttt < 1)
                {
                    yield return null;
                    tttt += Time.deltaTime / Delay;
                    player.transform.localScale = Vector2.Lerp(player.transform.localScale, Vector2.one * 2, tttt);
                    renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.Lerp(renderer.color.a, 0, tttt));
                }
                CameraManager.SlowCameraDisable();
                //////////////////// 플레이어 죽는 임시 코드 ////////////////////
                yield break; // 더이상 체크 안함 [이미 죽었어... ㅡㅅㅡ]
            }
        }
        _resultScreen.ShowUI(GameObject.FindObjectsByType<EnemyConfig>(FindObjectsSortMode.None).Length == 0);
    }

    // 지나가는 길에 적이 있남?
    GameObject WayEnemyActive(TileWay Coords, TileWay Last_Coords) {
        GameObject Enemy = TileManager.IsEnemyToCoords(Coords);
        if (Enemy == null) return null;
        
        // 적이 때릴 수 있음
        if (Enemy.CompareTag("EnemyAttack")) {
            // GetWayToDirection + GetDirection 함수를 합치면 될것같은데 일단 따로 나눔
            // TileWay diffCoords = TileWay.GetWayToDirection(TileWay.GetDirection(Last_Coords, Coords));
            TileWay diffCoords = Coords - Last_Coords; // 간---단
            GameObject AttackEnemy_Owner = TileManager.IsEnemyToCoords(Coords + diffCoords);

            // attacker 주인이 있고, 진짜 주인인감?
            if (AttackEnemy_Owner != null && !AttackEnemy_Owner.CompareTag("EnemyAttack") && Enemy.transform.parent == AttackEnemy_Owner.transform) {
                return Enemy;
            }
            return null;
        }

        StartCoroutine(RegisterSlowMotion(Enemy.transform.position));
        return null;
    }
    
    IEnumerator RegisterSlowMotion(Vector3 EnemyCoords) {
        // 일단 거리가 좁아질때까지 기다리자.
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, EnemyCoords) < 1.2f && Time.timeScale == 1);
        Time.timeScale = 0.05f;
        _animator.SetBool("isAttack", true);
        CameraManager.SlowCameraEnable(EnemyCoords);
    }
}
