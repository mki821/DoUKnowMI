using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public List<Vector2> movePos = new List<Vector2>();

    private float speed = 8f;
    private LayerMask layer = 7;

    private void Start() {
        movePos.Add(transform.position);
    }

    public void Move() {
        StartCoroutine(M());
    }

    private IEnumerator M() {
        float t = 0;
        for(int i = 1; i < movePos.Count; i++) {
            float distance = (movePos[i - 1] - movePos[i]).magnitude;
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

    private bool CheckEnemy(Vector2 dir, float distance) {
        return Physics2D.Raycast(transform.position, dir, distance, layer);
    }
}
