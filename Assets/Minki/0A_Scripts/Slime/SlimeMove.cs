    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public List<Vector2> enemyPos = new List<Vector2>();
    public Vector2 direction;
    public int curPos = 0;

    private float speed = 8f;
    private LayerMask layer = 7;
    private Animator animator;
    private SpriteRenderer rend;
    private DrawLine _drawLine;
    private float angle;
    private int i;

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
        _drawLine._posList.Add(transform.position);
        animator = GetComponent<Animator>();
        animator.SetFloat("Slime_Idle", Slime_Idle);
        rend = GetComponent<SpriteRenderer>();
        rend.sortingOrder = 100;
        //Time.timeScale = 0.1f;
    }

    public void Move() {
        StartCoroutine(M());
    }

    private IEnumerator M() {
        float t = 0;
        for(i = 1; i < _drawLine._posList.Count; i++) {
            // if (i == movePos.Count - 1) {
            //     CameraManager.SlowCameraEnable(enemyPos[enemyPos.Count - 1]);
            //     StartCoroutine(TimeScale());
            // }
            float distance = (_drawLine._posList[i - 1] - _drawLine._posList[i]).magnitude;
            curPos++;
            SetSlimeDir(i);
            while(t < 1 * distance / speed) {
                transform.position = Vector2.Lerp(_drawLine._posList[i - 1], _drawLine._posList[i], t / distance * speed);
                t += Time.deltaTime;
                //SetSlimeDir(i);
                yield return null;
            }
            t = 0;
        }
        // if (Mathf.Abs(angle - (-90f)) < 0.0001f) {
        //     animator.SetFloat("Slime_Idle", Slime_Idle);
        // }
        // else if (Mathf.Abs(angle - (180f)) < 0.0001f) {
        //     animator.SetFloat("Slime_Idle", Slime_Idle);
        // }
        // else if (Mathf.Abs(angle - (0f)) < 0.0001f) {
        //     animator.SetFloat("Slime_Idle", Slime_Idle_right);
        // }
        // else if (Mathf.Abs(angle - (90f)) < 0.0001f) {
        //     animator.SetFloat("Slime_Idle", Slime_Idle_back);
        // }
        // else if (Mathf.Abs(angle - (-135f)) < 0.0001f) {
        //     animator.SetFloat("Slime_Idle", Slime_Idle_diagonal_left);
        // }
        // else if (Mathf.Abs(angle - (45f)) < 0.0001f) {
        //     animator.SetFloat("Slime_Idle", Slime_Idle_back);
        // }
        // else if (Mathf.Abs(angle - (135f)) < 0.0001f) {
        //     animator.SetFloat("Slime_Idle", Slime_Idle_back);
        // }
        // else if (Mathf.Abs(angle - (-45f)) < 0.0001f) {
        //     animator.SetFloat("Slime_Idle", Slime_Idle_diagonal_right);
        // }
        CameraManager.SlowCameraDisable();

        BlockManager.instance.ShowPanel(BatchCharacter.enemyColList.Count == 0);
    }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     if (i == movePos.Count - 1) {
    //         CameraManager.SlowCameraEnable(enemyPos[enemyPos.Count - 1]);
    //         StartCoroutine(TimeScale());
    //     }
    // }


    public void SetMovePos(Vector2 pos) {
        _drawLine._posList.Add(pos);
    }

    public void RevertMovePos() {
        if (_drawLine._posList.Count > 1) _drawLine._posList.Remove(_drawLine._posList[_drawLine._posList.Count - 1]);
    }

    public void ResetMovePos() {
        _drawLine._posList.Clear();
    }

    private IEnumerator TimeScale() {
        //yield return null;
        Time.timeScale = 0.01f;
        print(Time.timeScale);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        print(Time.timeScale);
    }

    private void SetSlimeDir(int i) {
        Vector2 dir  = _drawLine._posList[i] - _drawLine._posList[i - 1];
        if(i < _drawLine._posList.Count - 1 && (_drawLine._posList[i] - transform.position).magnitude < 0.2f) dir = _drawLine._posList[i + 1] - _drawLine._posList[i];
        direction = dir.normalized;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        StartCoroutine(Check());
        animator.SetTrigger("IsAtk");
        if (Mathf.Abs(angle - (-90f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_Atk);
        }
        else if (Mathf.Abs(angle - (180f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_left_Atk);
        }
        else if (Mathf.Abs(angle - (0f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_right_Atk);
        }
        else if (Mathf.Abs(angle - (90f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_back_Atk);
        }
        else if (Mathf.Abs(angle - (-135f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_diagonal_left_Atk);
        }
        else if (Mathf.Abs(angle - (45f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_back_Atk);
        }
        else if (Mathf.Abs(angle - (135f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_back_Atk);
        }
        else if (Mathf.Abs(angle - (-45f)) < 0.0001f) {
            animator.SetFloat("Slime_Atk", Slime_diagonal_right_Atk);
        }
        // 더 추가 해야됨
        print("각도" + angle);
    }

    private bool CheckEnemy(Vector2 dir, float distance, float angle) {
        dir = Quaternion.Euler(0, 0, angle) * dir;
        Debug.DrawRay(transform.position, dir * distance, Color.red);
        return Physics2D.Raycast(transform.position, dir, distance, layer);
    }

    private IEnumerator Check() {
        while (CheckEnemy(Vector2.right, 0.5f, angle)) {
            if (Mathf.Abs(angle - (-90f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle);
            }
            else if (Mathf.Abs(angle - (180f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle);
            }
            else if (Mathf.Abs(angle - (0f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_right);
            }
            else if (Mathf.Abs(angle - (90f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_back);
            }
            else if (Mathf.Abs(angle - (-135f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_diagonal_left);
            }
            else if (Mathf.Abs(angle - (45f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_back);
            }
            else if (Mathf.Abs(angle - (135f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_back);
            }
            else if (Mathf.Abs(angle - (-45f)) < 0.0001f) {
                animator.SetFloat("Slime_Idle", Slime_Idle_diagonal_right);
            }

            if (i == _drawLine._posList.Count - 1) {
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
