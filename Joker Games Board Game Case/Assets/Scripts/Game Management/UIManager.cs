using System.Collections.Generic;
using Data_Management;
using TMPro;
using UnityEngine;

namespace Game_Management
{
    public class UIManager : MonoBehaviour
    {
        // Public static property to access the instance
        public static UIManager Instance { get; private set; }
        public GameObject panelMainGameObject, menuGameObject, buttonDiceRoll, scrollBarDiceAmount, buttonReturnToMainMenu;
        private List<TMP_Text> diceAmounts = new List<TMP_Text>();


        // Ensure that the instance is unique and handle duplication
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
        }
        
        private void OnEnable()
        {
            Actions.NextTurn += NextTurn;
        }

        private void OnDisable()
        {
            Actions.NextTurn -= NextTurn;
        }

        public void LoadGame(bool isNew)
        {
            menuGameObject.SetActive(false);
            panelMainGameObject.SetActive(false);
            buttonDiceRoll.SetActive(true);
            buttonReturnToMainMenu.SetActive(true);
            if (isNew) Actions.NewGame?.Invoke();
            else Actions.LoadGame?.Invoke();
        }

        public void ReturnToMainMenu()
        {
            PlayerDataManager.SaveData();
            buttonReturnToMainMenu.SetActive(false);
            buttonDiceRoll.SetActive(false);
            menuGameObject.SetActive(true);
            panelMainGameObject.SetActive(true);
        }
        
        public void UpdateDiceAmount()
        {
            int amount = transform.GetSiblingIndex() + 1;
            PlayerDataManager.UpdateDiceAmount(amount);
        }
        
        public void RollDice()
        {
            buttonDiceRoll.SetActive(false);
            var diceAmount = PlayerDataManager.PlayerData.diceAmount;
            Actions.RollDice?.Invoke(diceAmount);
        }

        private void NextTurn()
        {
            buttonDiceRoll.SetActive(true);
        }
    
        // Start is called before the first frame update
        private void Start()
        {
            if (PlayerDataManager.MapOrder.Count <= 0)
            {
                menuGameObject.transform.GetChild(0).gameObject.SetActive(false);
            }

            foreach (Transform child in scrollBarDiceAmount.transform)
            {
                int amountNo = child.GetSiblingIndex() + 1;
                if (child.TryGetComponent(out TMP_Text text))
                    text.text = "x" + amountNo;
            }
        }
    }
}
