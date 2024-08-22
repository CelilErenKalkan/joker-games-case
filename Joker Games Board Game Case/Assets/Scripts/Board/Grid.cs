using Game_Management;
using Item_Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Board
{
    public class Grid : MonoBehaviour
    {
        private Image _prizeIcon;
        private TMP_Text _prizeAmountText;

        public void SetGrid(ItemType itemType, int amount)
        {
            Actions.GridAppeared?.Invoke();
            
            if (itemType == ItemType.Empty) return;
            
            if (transform.GetChild(0).GetChild(0).GetChild(0).TryGetComponent(out Image image)) 
                _prizeIcon = image;
            if (transform.GetChild(0).GetChild(0).GetChild(1).TryGetComponent(out TMP_Text text)) 
                _prizeAmountText = text;
            
            _prizeIcon.enabled = true;
            var item = ItemFactory.CreateItem(itemType);
            _prizeIcon.sprite = item.GetIcon;
            _prizeAmountText.text = "x" + amount;
            GameManager.Instance.gameMap.Add(transform);
        }

        public void GridHasFallen()
        {
            Actions.GridHasFallen?.Invoke();
        }
    }
}
