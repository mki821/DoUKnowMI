using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    static CinemachineVirtualCamera _SlowCamera;
    [SerializeField] CinemachineVirtualCamera SlowCamera;

    private void Awake() {
        if (_SlowCamera == null)
            _SlowCamera = SlowCamera;
    }

    public static void SlowCameraEnable(Vector3 coords) {
        if (_SlowCamera.Follow == null) {
            GameObject LookEntity = new GameObject("Cam_Dummy");
            LookEntity.transform.position = coords;
            _SlowCamera.Follow = LookEntity.transform;
        }

        _SlowCamera.Follow.position = coords;
        _SlowCamera.gameObject.SetActive(true);
    }

    public static void SlowCameraDisable() {
        _SlowCamera.gameObject.SetActive(false);
    }
}
