using System.Collections;
using System.Collections.Generic;
using domi.DB;
using UnityEngine;

public class Refill : MonoBehaviour
{
    DBstruct _DBstruct;
    private void Start() {
        StartCoroutine(Wait10Minute());
    }
    IEnumerator Wait10Minute() {
        yield return new WaitForSecondsRealtime(600);
        _DBstruct.health++;
    }
}
