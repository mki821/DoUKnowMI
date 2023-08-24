using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public List<Vector2> enemyPos = new List<Vector2>();
    public Vector2 direction;
    public int curPos = 0;
    public bool isMoving = false;

    private float speed = 8f;
    private LayerMask layer = 7;
    private Animator animator;
    private SpriteRenderer rend;
    private DrawLine _drawLine;
    private float angle;
    private int i;
    private int enemyCnt;

#region Slime_Idle
    private const float Slime_Idle_left = 0;
    private const float Slime_Idle_diagonal_left = 0.2f;
    private const float Slime_Idle = 0.4f;
    private const float Slime_Idle_diagonal_right = 0.6f;
    private const float Slime_Idle_right = 0.8f;
    private const float Slime_Idle_back = 1;
    
#endregion

#region Slime_Atk
    private const float Slime_left_Atk = 0;
    private const float Slime_diagonal_left_Atk = 0.2f;
    private const float Slime_Atk = 0.4f;
    private const float Slime_diagonal_right_Atk = 0.6f;
    private const float Slime_right_Atk = 0.8f;
    private const float Slime_back_Atk = 1;

#endregion

    private void Start() {
        _drawLine = GameObject.Find("LineRenderer").GetComponent<DrawLine>();
        _drawLine._posList.Push(transform.position);
        animator = GetComponent<Animator>();
        animator.SetFloat("Slime_Idle", Slime_Idle);
        rend = GetComponent<SpriteRenderer>();
        rend.sortingOrder = 100;
    }

    public void Move() {
        StartCoroutine("M");
    }

    public void StopMove() {
        StopCoroutine("M");
    }

    private IEnumerator M() {
        isMoving = true;
        float t = 0;
        Vector3[] movePos = _drawLine._posList.ToArray();
        for(i = movePos.Length - 2; i > 0; i--) {
            float distance = (movePos[i - 1] - movePos[i]).magnitude;
            curPos++;
            SetSlimeDir(i);
            while(t < 1 * distance / speed) {
                transform.position = Vector2.Lerp(movePos[i], movePos[i - 1], t / distance * speed);
                t += Time.deltaTime;

                if ((transform.position - movePos[i - 1]).magnitude < BlockManager.instance.tileSize * 0.85f) {

                    if(i < 2) direction = (movePos[i - 1] - movePos[i]).normalized;
                    else direction = (movePos[i - 2] - movePos[i - 1]).normalized;
                }
                else {
                    direction = (movePos[i - 1] - movePos[i]).normalized;
                }

                //SetSlimeDir(i);
                yield return null;
            }
            t = 0;
        }
        CameraManager.SlowCameraDisable();

        yield return new WaitForSeconds(1f);

        isMoving = false;
        BlockManager.instance.ShowPanel(BatchCharacter.enemyColList.Count == 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Key")) {
            SoundManager.instance.GetKeySound();
        }
        else {
            SoundManager.instance.EatSound();
        }
    }


    public void SetMovePos(Vector2 pos) {
        _drawLine._posList.Push(pos);
    }

    public void RevertMovePos() {
        if (_drawLine._posList.Count > 1) _drawLine._posList.Pop();
    }

    public void ResetMovePos() {
        _drawLine._posList.Clear();
    }

    private IEnumerator TimeScale() {
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
    }

    private void SetSlimeDir(int i) {;
        Vector3[] movePos = _drawLine._posList.ToArray();
        //Vector2 dir  = _drawLine._posList.ToArray()[i] - _drawLine._posList.ToArray()[i - 1];
        Vector2 dir = movePos[i] -  movePos[i - 1];
        //if(i < _drawLine._posList.Count - 1 && (_drawLine._posList.ToArray()[i] - transform.position).magnitude < 0.2f) dir = _drawLine._posList.ToArray()[i + 1] - _drawLine._posList.ToArray()[i];
        
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        StartCoroutine(Check());
        animator.SetTrigger("IsAtk");
        if (Mathf.Abs(angle - (-90f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_back_Atk);
        }
        else if (Mathf.Abs(angle - (180f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_right_Atk);
        }
        else if (Mathf.Abs(angle - (0f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_left_Atk);
        }
        else if (Mathf.Abs(angle - (90f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_Atk);
        }
        else if (Mathf.Abs(angle - (-135f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_back_Atk);
        }
        else if (Mathf.Abs(angle - (45f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_diagonal_left_Atk);
        }
        else if (Mathf.Abs(angle - (135f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_diagonal_right_Atk);
        }
        else if (Mathf.Abs(angle - (-45f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_back_Atk);
        }
        // 더 추가 해야됨
        Debug.Log($"각도: {angle}");
        enemyCnt++;
    }

    private bool CheckEnemy(Vector2 dir, float distance, float angle) {
        dir = Quaternion.Euler(0, 0, angle) * dir;
        Debug.DrawRay(transform.position, dir * distance, Color.red);
        return Physics2D.Raycast(transform.position, dir, distance, layer);
    }

    private IEnumerator Check() {
        while (CheckEnemy(Vector2.right, 0.5f, angle)) {
            if (Mathf.Abs(angle - (-90f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_back);
            }
            else if (Mathf.Abs(angle - (180f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_right);
            }
            else if (Mathf.Abs(angle - (0f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_left);
            }
            else if (Mathf.Abs(angle - (90f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle);
            }
            else if (Mathf.Abs(angle - (-135f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_back);
            }
            else if (Mathf.Abs(angle - (45f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_diagonal_left);
            }
            else if (Mathf.Abs(angle - (135f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_diagonal_right);
            }
            else if (Mathf.Abs(angle - (-45f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_back);
            }

            if (i == 1) {
                CameraManager.SlowCameraEnable(enemyPos[enemyPos.Count - 1]);
                StartCoroutine(TimeScale());
                break;
            }
            yield return null;
        }
    }

    public void AddEnemyPos(Vector3 pos) {
        enemyPos.Add((Vector2)pos);
    }
}
