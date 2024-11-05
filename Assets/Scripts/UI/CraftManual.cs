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
        controller.isPrewviewActive = true;
        baseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        controller.canLook = true;
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
