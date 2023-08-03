using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCheck : MonoBehaviour
{
    [SerializeField] private LayerMask blockLayer;

    [SerializeField] private SlimeMove _slimeMove;

    private Camera _cam;

    private BatchCharacter _batchCharacter;

    private void Awake() {
        _cam = Camera.main;
        _batchCharacter = GetComponent<BatchCharacter>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0, blockLayer);

            if(hit.collider != null) {
                Block block = hit.transform.GetComponent<Block>();

                _slimeMove.SetMovePos(block.worldPos);
            }
        }
    }
}
