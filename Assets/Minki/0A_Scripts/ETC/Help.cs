using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject[] helpImages;

    [SerializeField] private GameObject prevGameobject = null;

    private int _curPage = 0;

    private int CurPage {
        get => _curPage;
        set {
            _curPage = Mathf.Clamp(value, 0, 3);
        }
    }

    public void ShowPanel() {
        helpPanel.SetActive(true);
        if (prevGameobject is not null) Destroy(prevGameobject);
        
        prevGameobject = Instantiate(helpImages[CurPage], helpPanel.transform.position, quaternion.identity, helpPanel.transform);
    }

    public void ClosePanel() {
        if (prevGameobject is not null) Destroy(prevGameobject);
        helpPanel.SetActive(false);
    }

    public void Next() {
        if (prevGameobject is not null) Destroy(prevGameobject);
        CurPage++;
        
        prevGameobject = Instantiate(helpImages[CurPage], helpPanel.transform.position, quaternion.identity, helpPanel.transform);
    }

    public void Prev() {
        if (prevGameobject is not null) Destroy(prevGameobject);
        CurPage--;

        prevGameobject = Instantiate(helpImages[CurPage], helpPanel.transform.position, quaternion.identity, helpPanel.transform);
    }
}
