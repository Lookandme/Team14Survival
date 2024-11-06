using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCraftTab : MonoBehaviour
{
    public CraftingRecipe craftingRecipe; //������� ������ ������

    public UIInventory inventory; //�κ��丮 ����

    public GameObject Tab; //�ش� ���ӿ�����Ʈ

    public Image material1Icon; //���1 �̹���
    public Image material2Icon; //���2 �̹���
    public Image craftIcon; //���� �Ϸ� �̹���

    public Button button;
    public Image buttonBg; //��ư ���� ��Ȱ��ȭ�� ȸ��, Ȱ��ȭ�� �ʷϻ�
    private Color originalColor;

    public TextMeshProUGUI material1QuatityText;//��� 1�ʿ��
    public TextMeshProUGUI material2QuatityText;//��� 2�ʿ��
    public TextMeshProUGUI craftQuatityText;//���� �� ��

    int material1Quatity;
    int material2Quatity;
    int craftQuatity;

    public Outline outline; //�� �̹��� �ܰ� �ƿ�����

    public bool isActivated = false; //��ư Ȱ��ȭ ����

    ItemData printedMaterial; //��ũ���ͺ� ������Ʈ ���.

    List<ItemData> materialsArray = new List<ItemData>();
    List<ItemData> resultsArray = new List<ItemData>();

    List<int> materialsQuantityArray = new List<int>(); //��� ��
    List<int> resultsQuantityArray = new List<int>(); //����� ��
    ItemData material1;
    ItemData material2;
    ItemData result;

    private void OnEnable()
    {
        outline.enabled = isActivated;
    }

    private void Start()
    {
        originalColor = buttonBg.color; // ���� ���� ����
        PrintMaterials(); //��ϵ� ��ũ���ͺ������Ʈ ������������
        PrintResults();
        PrintResultsQuantity();
        PrintMaterialsQuantity();
        material1 = materialsArray[0];
        material2 = materialsArray[1];
        result = resultsArray[0];

        material1Icon.sprite = materialsArray[0].icon;
        material2Icon.sprite = materialsArray[1].icon;
        craftIcon.sprite = resultsArray[0].icon;

        material1QuatityText.text = materialsQuantityArray[0].ToString();
        material2QuatityText.text = materialsQuantityArray[1].ToString();
        craftQuatityText.text = resultsQuantityArray[0].ToString();

        material1Quatity = materialsQuantityArray[0];
        material2Quatity = materialsQuantityArray[1];
        craftQuatity = resultsQuantityArray[0];
    }

    // ��ũ���ͺ������Ʈ CraftingRecipe���ο� �ִ� ��ũ���ͺ� ������Ʈ ItemData���� ������ ����� ���ֳ�?
    public void PrintMaterials()
    {
        materialsArray.Clear(); // ���� ���� �ʱ�ȭ

        // craftingRecipe.Materials�� �� ItemAmount���� item�� �����Ͽ� �߰�
        for (int i = 0; i < craftingRecipe.Materials.Count; i++)
        {
            ItemData itemData = craftingRecipe.Materials[i].item; // ItemAmount�� item�� ����
            materialsArray.Add(itemData); // materialsArray�� itemData �߰�
        }
    }

    public void PrintResults()
    {
        resultsArray.Clear(); // ���� ���� �ʱ�ȭ

        // craftingRecipe.Materials�� �� ItemAmount���� item�� �����Ͽ� �߰�
        for (int i = 0; i < craftingRecipe.Results.Count; i++)
        {
            ItemData itemData = craftingRecipe.Results[i].item; // ItemAmount�� item�� ����
            resultsArray.Add(itemData); // materialsArray�� itemData �߰�
        }
    }

    public void PrintMaterialsQuantity()
    {
        materialsQuantityArray.Clear(); // ���� ���� �ʱ�ȭ

        for (int i = 0; i < craftingRecipe.Materials.Count; i++)
        {
            int requireAmount = craftingRecipe.Materials[i].Amount;
            materialsQuantityArray.Add(requireAmount);
        }
    }

    public void PrintResultsQuantity()
    {
        resultsQuantityArray.Clear(); // ���� ���� �ʱ�ȭ

        for (int i = 0; i < craftingRecipe.Results.Count; i++)
        {
            int resultAmount = craftingRecipe.Results[i].Amount;
            resultsQuantityArray.Add(resultAmount);
        }
    }
    /*
    //��ᰡ ���� ��� ��ư Ȱ��ȭ. ���⼭�� �ʷϻ����θ� �߰� �ϸ� ��.
    public void ActiveButton()
    {
        buttonBg.color = inventory.HasItem(material1, material2, material1Quatity, material2Quatity) ? Color.green : originalColor;
    }
    */

    public void OnClickButton()
    {

        if (inventory.CanCraft(material1, material2, material1Quatity, material2Quatity))
        {
            inventory.RemoveMaterialItem(material1, material1Quatity);
            inventory.RemoveMaterialItem(material2, material2Quatity);
            inventory.CraftItem(result);
        }

    }

}
