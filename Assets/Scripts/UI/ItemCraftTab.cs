using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCraftTab : MonoBehaviour
{
    public CraftingRecipe craftingRecipe; //들고있을 아이템 데이터

    public UIInventory inventory; //인벤토리 접근

    public GameObject Tab; //해당 게임오브젝트

    public Image material1Icon; //재료1 이미지
    public Image material2Icon; //재료2 이미지
    public Image craftIcon; //조합 완료 이미지

    public Button button;
    public Image buttonBg; //버튼 배경색 비활성화시 회색, 활성화시 초록색
    private Color originalColor;

    public TextMeshProUGUI material1QuatityText;//재료 1필요양
    public TextMeshProUGUI material2QuatityText;//재료 2필요양
    public TextMeshProUGUI craftQuatityText;//가공 후 양

    int material1Quatity;
    int material2Quatity;
    int craftQuatity;

    public Outline outline; //이 이미지 외곽 아웃라인

    public bool isActivated = false; //버튼 활성화 여부

    ItemData printedMaterial; //스크립터블 오브젝트 출력.

    List<ItemData> materialsArray = new List<ItemData>();
    List<ItemData> resultsArray = new List<ItemData>();

    List<int> materialsQuantityArray = new List<int>(); //재료 양
    List<int> resultsQuantityArray = new List<int>(); //결과값 양
    ItemData material1;
    ItemData material2;
    ItemData result;

    private void OnEnable()
    {
        outline.enabled = isActivated;
    }

    private void Start()
    {
        originalColor = buttonBg.color; // 기존 색상 저장
        PrintMaterials(); //등록된 스크립터블오브젝트 정보가져오기
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

    // 스크립터블오브젝트 CraftingRecipe내부에 있는 스크립터블 오브젝트 ItemData들의 정보를 출력할 수있나?
    public void PrintMaterials()
    {
        materialsArray.Clear(); // 기존 내용 초기화

        // craftingRecipe.Materials의 각 ItemAmount에서 item을 추출하여 추가
        for (int i = 0; i < craftingRecipe.Materials.Count; i++)
        {
            ItemData itemData = craftingRecipe.Materials[i].item; // ItemAmount의 item에 접근
            materialsArray.Add(itemData); // materialsArray에 itemData 추가
        }
    }

    public void PrintResults()
    {
        resultsArray.Clear(); // 기존 내용 초기화

        // craftingRecipe.Materials의 각 ItemAmount에서 item을 추출하여 추가
        for (int i = 0; i < craftingRecipe.Results.Count; i++)
        {
            ItemData itemData = craftingRecipe.Results[i].item; // ItemAmount의 item에 접근
            resultsArray.Add(itemData); // materialsArray에 itemData 추가
        }
    }

    public void PrintMaterialsQuantity()
    {
        materialsQuantityArray.Clear(); // 기존 내용 초기화

        for (int i = 0; i < craftingRecipe.Materials.Count; i++)
        {
            int requireAmount = craftingRecipe.Materials[i].Amount;
            materialsQuantityArray.Add(requireAmount);
        }
    }

    public void PrintResultsQuantity()
    {
        resultsQuantityArray.Clear(); // 기존 내용 초기화

        for (int i = 0; i < craftingRecipe.Results.Count; i++)
        {
            int resultAmount = craftingRecipe.Results[i].Amount;
            resultsQuantityArray.Add(resultAmount);
        }
    }
    /*
    //재료가 있을 경우 버튼 활성화. 여기서는 초록색으로만 뜨게 하면 됨.
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
