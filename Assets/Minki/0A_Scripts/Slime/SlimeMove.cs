using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public List<Vector2> movePos = new List<Vector2>();

    private float speed = 8f;
    private LayerMask layer = 7;
    private Animator animator;

    private const float Slime_Idle_left = 0;
    private const float Slime_Idle_diagonal_left = 0.2f;
    private const float Slime_Idle = 0.4f;
    private const float Slime_Idle_diagonal_right = 0.6f;
    private const float Slime_Idle_right = 0.8f;
    private const float Slime_Idle_back = 1;


    private void Start() {
        movePos.Add(transform.position);
        animator = GetComponent<Animator>();
        animator.SetFloat("Slime_Idle", Slime_Idle);
    }

    public void Move() {
        StartCoroutine(M());
    }

    private IEnumerator M() {
        float t = 0;
        for(int i = 1; i < movePos.Count; i++) {
            if (i == movePos.Count - 1) {
                //CameraManager.SlowCameraEnable(transform.position);
                StartCoroutine(TimeScale());
            }
            float distance = (movePos[i - 1] - movePos[i]).magnitude;
            SetSlimeDir(i);
            while(t < 1 * distance / speed) {
                transform.position = Vector2.Lerp(movePos[i - 1], movePos[i], t / distance * speed);
                t += Time.deltaTime;
                yield return null;
            }
            t = 0;
        }
    }


    public void SetMovePos(Vector2 pos) {
        movePos.Add(pos);
    }

    private IEnumerator TimeScale() {
        Time.timeScale = 0.05f;
        print(Time.timeScale);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        print(Time.timeScale);
    }

    private void SetSlimeDir(int i) {
        Vector2 dir  = movePos[i] - movePos[i - 1];
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (Mathf.Abs(angle - (-90f)) < 0.0001f)
            animator.SetFloat("Slime_Idle", Slime_Idle);
        else if (Mathf.Abs(angle - (180f)) < 0.0001f)
            animator.SetFloat("Slime_Idle", Slime_Idle_left);
        else if (Mathf.Abs(angle - (0f)) < 0.0001f)
            animator.SetFloat("Slime_Idle", Slime_Idle_right);
        // 더 추가 해야됨
        print("각도" + angle);
    }

    private bool CheckEnemy(Vector2 dir, float distance) {
        return Physics2D.Raycast(transform.position, dir, distance, layer);
    }
}
