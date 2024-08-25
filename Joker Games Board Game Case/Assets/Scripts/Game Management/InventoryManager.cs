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
        [SerializeField] private Button openInventory, closeInventory;
        [SerializeField] private List<Item> _itemList;

        private readonly List<Image> slotImages = new List<Image>();
        private readonly List<TMP_Text> itemAmountList = new List<TMP_Text>();

        private void OnEnable()
        {
            Actions.PrizeAddedToBag += SetInventoryList;
        }

        private void OnDisable()
        {
            Actions.PrizeAddedToBag -= SetInventoryList;
        }
        
        private void Start()
        {
            // Initialize item list and add button listeners
            _itemList = PlayerDataManager.PlayerData.itemList;
            openInventory.onClick.AddListener(OpenBag);
            closeInventory.onClick.AddListener(CloseBag);
            
            // Set up inventory slots
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
            // Update inventory UI with item icons and amounts
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
            // Set up slot images and item amount text fields
            for (var i = 0; i < slotsParent.childCount; i++)
            {
                var slot = slotsParent.GetChild(i);
                
                if (slot.TryGetComponent(out Image image))
                {
                    slotImages.Add(image);
                    image.enabled = false; // Initially disable slot images
                }

                if (slot.GetChild(0).TryGetComponent(out TMP_Text text))
                {
                    itemAmountList.Add(text);
                }
            }
        }
    }
}
