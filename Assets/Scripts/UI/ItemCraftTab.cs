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

    public TextMeshProUGUI material1QuatityText;//재료 1필요양
    public TextMeshProUGUI material2QuatityText;//재료 2필요양
    public TextMeshProUGUI craftQuatityText;//가공 후 양

    private Outline outline; //이 이미지 외곽 아웃라인

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
        SetUI();
    }

    private void SetUI()
    {
        PrintMaterials(); //등록된 스크립터블오브젝트 정보가져오기
        PrintResults();
        PrintResultsQuantity();
        PrintMaterialsQuantity();
        material1 = materialsArray[0];
        material2 = materialsArray[1];

        material1Icon.sprite = materialsArray[0].icon;
        material2Icon.sprite = materialsArray[1].icon;
        craftIcon.sprite = resultsArray[0].icon;

        material1QuatityText.text = materialsQuantityArray[0].ToString();
        material2QuatityText.text = materialsQuantityArray[1].ToString();
        craftQuatityText.text = resultsQuantityArray[0].ToString();
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
    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
    */
}
