using System.Collections.Generic;
using Data_Management;
using Item_Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Management
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private Transform slotsParent;
        private readonly List<Image> slotImages = new List<Image>();
        private readonly List<TMP_Text> itemAmountList = new List<TMP_Text>();

        [SerializeField] private Button openInventory, closeInventory;
        [SerializeField] private List<Item> _itemList;

        // Start is called before the first frame update
        private void Start()
        {
            _itemList = PlayerDataManager.PlayerData.itemList;
            openInventory.onClick.AddListener(OpenBag);
            closeInventory.onClick.AddListener(CloseBag);
            SetSlots();
        }

        private void OpenBag()
        {
            Actions.ButtonTapped?.Invoke();

            SetInventoryList();
        }

        private void CloseBag()
        {
            Actions.ButtonTapped?.Invoke();
            
            SetInventoryList();
        }

        private void SetInventoryList()
        {
            for (var i = 0; i < slotsParent.childCount; i++)
            {
                if (i < _itemList.Count)
                {
                    slotImages[i].sprite = _itemList[i].GetIcon;
                    slotImages[i].enabled = true;
                    itemAmountList[i].text = "x" + _itemList[i].itemAmount;
                }
                else
                {
                    slotImages[i].sprite = null;
                    slotImages[i].enabled = false;
                    itemAmountList[i].text = "";
                }
            }
        }

        private void SetSlots()
        {
            for (var i = 0; i < slotsParent.childCount; i++)
            {
                if (slotsParent.GetChild(i).TryGetComponent(out Image image)) slotImages.Add(image);
                image.enabled = false;
                if (slotsParent.GetChild(i).GetChild(0).TryGetComponent(out TMP_Text text)) itemAmountList.Add(text);
            }
        }
    }
}