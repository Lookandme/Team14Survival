using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.XR;

public class ItemCraftWindow : MonoBehaviour
{
    private PlayerController controller;
    public GameObject itemCraftWindow;
    public GameObject inventoryWindow;
    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        controller.itemCraft += Toggle;
        itemCraftWindow.SetActive(false);
    }

    public void Toggle()
    {
        if (itemCraftWindow.activeInHierarchy)
        {
            itemCraftWindow.SetActive(false);
        }
        else
        {
            itemCraftWindow.SetActive(true);
        }
    }

}