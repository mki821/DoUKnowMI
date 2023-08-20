using System.Collections;
using System.Collections.Generic;
using TMPro;
using domi.DB;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    DBstruct _DBstruct;
    [SerializeField] TextMeshProUGUI now_Health; 
    
    private void Start() {
        StartCoroutine(Wait10Minute());
        HealthPatch();
    }

    IEnumerator Wait10Minute() {
        yield return new WaitForSecondsRealtime(600);
        _DBstruct.health++;
        HealthPatch();
    }
    public void HealthPatch()
    {
        now_Health.text = _DBstruct.health.ToString();
    }
}
