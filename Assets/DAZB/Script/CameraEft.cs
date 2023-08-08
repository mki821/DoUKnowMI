using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEft : MonoBehaviour
{
    static CinemachineVirtualCamera _slowCam;
    [SerializeField] CinemachineVirtualCamera slowCam;

    private void Awake() {
        if (slowCam == null) {
            _slowCam = slowCam;
        }
    }

    public static void SlowEft(Vector3 target) {
        if (_slowCam.Follow == null) {
            GameObject LookObj = new("Test");
            LookObj.transform.position = target;
            _slowCam.Follow = LookObj.transform;
        }
        _slowCam.Follow.position = target;
        _slowCam.gameObject.SetActive(true);
    }

}
