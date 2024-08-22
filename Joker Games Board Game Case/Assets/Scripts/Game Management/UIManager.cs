using Data_Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Management
{
    public class UIManager : MonoBehaviour
    {
        // Public GameObjects and Buttons
        public GameObject panelMainGameObject, menuGameObject, scrollBarDiceAmount;
        public Button buttonLoadGame, buttonNewGame, buttonDiceRoll, buttonReturnToMainMenu, buttonAudio, buttonVibration;

        // Private variables for UI elements and data
        private Image _audioButtonImage;
        private Image _vibrationButtonImage;

        private Sprite _mute, _unmute, _vOff, _vOn;

        private void OnEnable()
        {
            // Subscribe to the NextTurn action when this object is enabled
            Actions.NextTurn += NextTurn;
        }

        private void OnDisable()
        {
            // Unsubscribe from the NextTurn action when this object is disabled
            Actions.NextTurn -= NextTurn;
        }

        private void Start()
        {
            // Set up sprites, button listeners, and dice amount texts
            SetSprites();
            SetButtons();
            SetDiceAmountTexts();

            // Hide specific UI elements if no map order is available
            if (PlayerDataManager.MapOrder.Count <= 0)
            {
                menuGameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        public void LoadGame(bool isNew)
        {
            Actions.ButtonTapped?.Invoke();
            
            // Switch between new and loaded game states
            menuGameObject.SetActive(false);
            panelMainGameObject.SetActive(false);
            buttonDiceRoll.gameObject.SetActive(true);
            buttonReturnToMainMenu.gameObject.SetActive(true);
            if (isNew) Actions.NewGame?.Invoke();
            else Actions.LoadGame?.Invoke();
        }

        #region Top Left Corner

        private void ChangeAudioMod()
        {
            Actions.ButtonTapped?.Invoke();

            // Toggle audio mute state and update button sprite
            PlayerDataManager.PlayerData.isMuted = !PlayerDataManager.PlayerData.isMuted;
            Debug.Log(PlayerDataManager.PlayerData.isMuted);
            SetAudioOrVibration(true);
        }

        private void ChangeVibrationMod()
        {
            Actions.ButtonTapped?.Invoke();
            
            // Toggle vibration state and update button sprite
            PlayerDataManager.PlayerData.isVibrationOff = !PlayerDataManager.PlayerData.isVibrationOff;
            SetAudioOrVibration(false);
        }

        private void ReturnToMainMenu()
        {
            Actions.ButtonTapped?.Invoke();
            
            // Save data and return to the main menu
            PlayerDataManager.SaveData();
            buttonReturnToMainMenu.gameObject.SetActive(false);
            buttonDiceRoll.gameObject.SetActive(false);
            menuGameObject.SetActive(true);
            panelMainGameObject.SetActive(true);
            Actions.GameEnd?.Invoke();
        }

        #endregion

        #region In Game

        public void UpdateDiceAmount()
        {
            // Update the dice amount based on the current transform index
            int amount = transform.GetSiblingIndex() + 1;
            PlayerDataManager.UpdateDiceAmount(amount);
        }
        
        private void RollDice()
        {
            // Hide the dice roll button and trigger dice roll action
            buttonDiceRoll.gameObject.SetActive(false);
            
            Actions.ButtonTapped?.Invoke();
            Actions.RollDice?.Invoke(PlayerDataManager.PlayerData.diceAmount);
        }

        private void NextTurn()
        {
            // Show the dice roll button for the next turn
            buttonDiceRoll.gameObject.SetActive(true);
        }

        #endregion

        #region Setting Variables

        private void SetAudioOrVibration(bool isAudio)
        {
            switch (isAudio)
            {
                case true:
                    _audioButtonImage.sprite = PlayerDataManager.PlayerData.isMuted ? _mute : _unmute;
                    Actions.AudioChanged?.Invoke(PlayerDataManager.PlayerData.isMuted);
                    break;
                case false:
                    _vibrationButtonImage.sprite = PlayerDataManager.PlayerData.isVibrationOff ? _vOff : _vOn;
                    Actions.VibrationChanged?.Invoke(PlayerDataManager.PlayerData.isVibrationOff);
                    break;
            }
            
            PlayerDataManager.SaveData();
        }
        
        private void SetSprites()
        {
            // Load sprite assets from Resources
            _mute = Resources.Load<Sprite>("UI/ui_icon_main_menu_mute");
            _unmute = Resources.Load<Sprite>("UI/ui_icon_main_menu_unmute");
            _vOff = Resources.Load<Sprite>("UI/ui_icon_main_menu_vibration_off");
            _vOn = Resources.Load<Sprite>("UI/ui_icon_main_menu_vibration_on");

            if (_mute == null || _unmute == null || _vOff == null || _vOn == null)
            {
                Debug.LogError("One or more sprites failed to load. Check paths and ensure assets are in Resources folder.");
            }
            
            // Initialize button images and add listeners for button clicks
            _audioButtonImage = buttonAudio.GetComponent<Image>();
            _vibrationButtonImage = buttonVibration.GetComponent<Image>();

            SetAudioOrVibration(true);
            SetAudioOrVibration(false);
        }

        private void SetButtons()
        {
            buttonLoadGame.onClick.AddListener(() => LoadGame(false));
            buttonNewGame.onClick.AddListener(() => LoadGame(true));
            buttonDiceRoll.onClick.AddListener(RollDice);
            buttonReturnToMainMenu.onClick.AddListener(ReturnToMainMenu);
            buttonAudio.onClick.AddListener(ChangeAudioMod);
            buttonVibration.onClick.AddListener(ChangeVibrationMod);
        }

        private void SetDiceAmountTexts()
        {
            // Update dice amount texts based on child transforms
            foreach (Transform child in scrollBarDiceAmount.transform)
            {
                if (child.TryGetComponent(out TMP_Text text))
                {
                    int amountNo = child.GetSiblingIndex() + 1;
                    text.text = "x" + amountNo;
                }
                else
                {
                    Debug.LogError("TMP_Text component not found on child.");
                }
            }
        }

        #endregion
    }
}
