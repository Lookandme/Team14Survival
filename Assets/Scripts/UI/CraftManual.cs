using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class CraftManual : MonoBehaviour
{
    [System.Serializable]
    public class Craft
    {
        public string craftName;
        public GameObject prefab;
        public GameObject previewPrefab;
    }

    private bool isActive = false;
    private bool isPrewviewActive = false;

    [SerializeField] private GameObject baseUI;
    [SerializeField] private Craft[] craftFire;

    private GameObject preview;

    [SerializeField]  private Transform transform;

    public void SlotClick(int slotNumber)
    {
        preview = Instantiate(craftFire[slotNumber].previewPrefab, transform.position + transform.forward, Quaternion.identity);
        isPrewviewActive = true;
        baseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Window();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cancle();
        }
    }

    void Cancle()
    {
        if(isPrewviewActive)
        {
            Destroy(preview);
        }
        isActive = false;
        isPrewviewActive = false ;
        preview = null;

        baseUI.SetActive(false );
    }

    void Window()
    {
        if (!isActive)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }
    void OpenWindow()
    {
        isActive = true;
        baseUI.SetActive(true);
    }
    void CloseWindow()
    {
        isActive = false;
        baseUI.SetActive(false);
    }
}
