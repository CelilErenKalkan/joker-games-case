using System.Collections.Generic;
using Data_Management;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Management
{
    public class UIManager : MonoBehaviour
    {
        // Public static property to access the instance
        //public static UIManager Instance { get; private set; }
        public GameObject panelMainGameObject, menuGameObject, buttonDiceRoll, scrollBarDiceAmount, buttonReturnToMainMenu, buttonAudio, buttonVibration;
        private List<TMP_Text> _diceAmounts = new List<TMP_Text>();

        private PlayerData _playerData;

        private Sprite _mute, _unmute, _vOff, _vOn;
        

        // Ensure that the instance is unique and handle duplication
        /*private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
        }*/
        
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

        #region Top Right Corner

        public void ChangeAudioMod()
        {
            _playerData.isMuted = !_playerData.isMuted;
            
            buttonAudio.GetComponent<Image>().sprite = _playerData.isMuted ? _mute : _unmute;
            
            Actions.AudioChanged?.Invoke(_playerData.isMuted);
        }
        
        public void ChangeVibrationMod()
        {
            _playerData.isVibrationOff = !_playerData.isVibrationOff;
            
            buttonVibration.GetComponent<Image>().sprite = _playerData.isVibrationOff ? _vOn : _vOff;
            
            Actions.VibrationChanged?.Invoke(_playerData.isMuted);
        }

        public void ReturnToMainMenu()
        {
            PlayerDataManager.SaveData();
            buttonReturnToMainMenu.SetActive(false);
            buttonDiceRoll.SetActive(false);
            menuGameObject.SetActive(true);
            panelMainGameObject.SetActive(true);
            Actions.GameEnd?.Invoke();
        }

        #endregion

        public void UpdateDiceAmount()
        {
            int amount = transform.GetSiblingIndex() + 1;
            PlayerDataManager.UpdateDiceAmount(amount);
        }
        
        public void RollDice()
        {
            buttonDiceRoll.SetActive(false);
            var diceAmount = _playerData.diceAmount;
            Actions.RollDice?.Invoke(diceAmount);
        }

        private void NextTurn()
        {
            buttonDiceRoll.SetActive(true);
        }
    
        // Start is called before the first frame update
        private void Start()
        {
            _playerData = PlayerDataManager.PlayerData;
            _mute = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/2D/UI/ui_icon_main_menu_mute.png");
            _unmute = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/2D/UI/ui_icon_main_menu_unmute.png");
            _vOff = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/2D/UI/ui_icon_main_menu_vibration_off.png");
            _vOn = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/2D/UI/ui_icon_main_menu_vibration_on.png");
            
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
