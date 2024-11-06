using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField] private GameObject baseUI;
    [SerializeField] private Craft[] craftFire;

    public List<GameObject> campfireItems;
    public List<GameObject> woodItmes;

    private GameObject preview;
    private GameObject prefab;
    private PlayerController controller;

    [SerializeField] private Transform transform;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    RaycastHit hit;
    private Camera camera;

    public void SlotClick(int slotNumber)
    {
        if (slotNumber < craftFire.Length)
        {
            preview = Instantiate(craftFire[slotNumber].previewPrefab, hit.point, Quaternion.identity);
            prefab = craftFire[slotNumber].prefab;
            controller.isPrewviewActive = true;
            baseUI.SetActive(false);
            controller.canLook = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void HideAllItems()
    {
        foreach (GameObject item in campfireItems)
        {
            item.SetActive(false);
        }
        foreach(GameObject item in woodItmes)
        {
            item.SetActive(false);
        }
    }
    public void showCampFire()
    {
        HideAllItems();
        foreach(GameObject item in campfireItems)
        {
            item.SetActive(true);
        }
    }

    public void showWoodItems()
    {
        HideAllItems();
        foreach(GameObject item in woodItmes)
        {
            item.SetActive(true) ;
        }
    }
    private void Start()
    {
        baseUI.SetActive(false);
        camera = Camera.main;
        controller = CharacterManager.Instance.Player.controller;
        controller.craftManual += Window;
        controller.escape += Cancle;
    }
    void Update()
    {
        if (controller.isPrewviewActive)
        {
            PreviewPostionUpdate();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }
    }
    void Build()
    {
        if (controller.isPrewviewActive && preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(prefab, hit.point, Quaternion.identity);
            Destroy(preview);
            isActive = false;
            controller.isPrewviewActive = false;
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
    public void OnCraftButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started && controller.isPrewviewActive)
        {
            // Y축을 기준으로 45도씩 회전
            preview.transform.Rotate(Vector3.up, 45f);
        }
    }

    void Cancle()
    {
        if (controller.isPrewviewActive)
        {
            Destroy(preview);
        }
        isActive = false;
        controller.isPrewviewActive = false;
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
