using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;
    private PlayerController controller;
    private PlayerConditions condition;
    private Camera camera;
    public LayerMask targetMask;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        camera = Camera.main;
    }
    private void Update()
    {

    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {

            curEquip.OnAttackInput();

            

        }
    }

    

    public void EquipNew(ItemData data)
    {
        UnEquip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
        ;
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

    public void MonsterHit()
    {
        EquipTool equipTool = curEquip as EquipTool;


        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, equipTool.attackDistance, targetMask))
        {

            hit.collider.GetComponent<IDamagable>().GetDamage(equipTool.damage);

        }
    }
}