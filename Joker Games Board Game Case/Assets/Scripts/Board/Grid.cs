using Game_Management;
using Item_Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Board
{
    public class Grid : MonoBehaviour
    {
        // Private fields for UI elements
        private Image _prizeIcon;
        private TMP_Text _prizeAmountText, _gridNoText;

        // Method to set up the grid with an item, amount, and grid number
        public void SetGrid(ItemType itemType, int amount, int gridNo)
        {
            Actions.GridAppeared?.Invoke(); // Trigger event when the grid appears

            // Cache child components for prize icon, amount text, and grid number text
            if (transform.GetChild(0).GetChild(0).GetChild(0).TryGetComponent(out Image image)) 
                _prizeIcon = image;
            if (transform.GetChild(0).GetChild(0).GetChild(1).TryGetComponent(out TMP_Text text)) 
                _prizeAmountText = text;
            if (transform.GetChild(0).GetChild(0).GetChild(2).TryGetComponent(out TMP_Text gridText)) 
                _gridNoText = gridText;

            // If the grid has an item, display its icon and amount
            if (itemType != ItemType.Empty)
            {
                _prizeIcon.enabled = true;
                var item = ItemFactory.CreateItem(itemType);
                _prizeIcon.sprite = item.GetIcon;
                _prizeAmountText.text = "x" + amount;
            }

            // Display the grid number
            _gridNoText.text = (gridNo + 1).ToString();

            // Add this grid's transform to the game map
            GameManager.Instance.gameMap.Add(transform);
        }

        // Method to invoke an event when the grid has fallen
        public void GridHasFallen()
        {
            Actions.GridHasFallen?.Invoke();
        }
    }
}