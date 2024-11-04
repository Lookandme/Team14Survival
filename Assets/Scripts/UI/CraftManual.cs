using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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
    public GameObject prefab;
    private PlayerController controller;

    [SerializeField] private Transform transform;
    [SerializeField]  private LayerMask layerMask;
    [SerializeField] private float range;

    RaycastHit hit;
    private Camera camera;

    public void SlotClick(int slotNumber)
    {
        preview = Instantiate(craftFire[slotNumber].previewPrefab, hit.point, Quaternion.identity);
        prefab = craftFire[slotNumber].prefab;
        isPrewviewActive = true;
        baseUI.SetActive(false);
    }

    private void Start()
    {
        camera = Camera.main;
        controller = CharacterManager.Instance.Player.controller;
        //controller.inventory += Toggle;
    }
    void Update() //개선필요
    {
        if (Input.GetKeyDown(KeyCode.F) && !isPrewviewActive)
        {
            Window();
        }

        if (isPrewviewActive)
        {
            PreviewPostionUpdate();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancle();
        }
    }
    void Build()
    {
        if (isPrewviewActive && preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(prefab, hit.point, Quaternion.identity);
            Destroy(preview);
            isActive = false;
            isPrewviewActive = false;
            prefab = null;
            preview = null;
        }
    }

    void PreviewPostionUpdate()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            if (hit.transform != null)
            {
                Vector3 location = hit.point;
                preview.transform.position = location;
            }
        }
    }
    /* public void Toggle()
     {
         if (inventoryWindow.activeInHierarchy)
         {
            BaseUI.SetActive(false);
            Destroy(preview);
         }
         else
         {
             BaseUI.SetActive(true);
         }
     }*/
    void Cancle()
    {
        if (isPrewviewActive)
        {
            Destroy(preview);
        }
        isActive = false;
        isPrewviewActive = false;
        preview = null;
        prefab = null;

        baseUI.SetActive(false);
    }

    void Window()
    {
        if (!isActive)
        {
            isActive = true;
            baseUI.SetActive(true);
        }
        else
        {
            isActive = false;
            baseUI.SetActive(false);
        }
    }
}
