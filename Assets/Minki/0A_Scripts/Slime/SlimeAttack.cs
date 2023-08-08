using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.name);
        if(other.CompareTag("EnemyAttack")) {
            Destroy(gameObject);
        }
        else if(other.CompareTag("Enemy")) {
            Destroy(other.gameObject);
        }
    }
}
