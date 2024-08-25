using Data_Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Management
{
    public class UIManager : MonoBehaviour
    {
        public GameObject menuGameObject;
        public Button buttonLoadGame, buttonNewGame, buttonDiceRoll, buttonReturnToMainMenu, buttonAudio, openBag, closeBag, buttonDiceAmount;
        public TMP_Text textDiceResult;

        private Animator _animator;
        private Image _audioButtonImage;
        private Sprite _mute, _unmute;

        private void OnEnable()
        {
            Actions.NextTurn += NextTurn;
            Actions.DiceAmountChanged += OnDiceAmountChanged;
            Actions.PrizeAddedToBag += OnPrizeAddedToBag;
            Actions.MoveForward += OnDiceResult;
        }

        private void OnDisable()
        {
            Actions.NextTurn -= NextTurn;
            Actions.DiceAmountChanged -= OnDiceAmountChanged;
            Actions.PrizeAddedToBag -= OnPrizeAddedToBag;
            Actions.MoveForward -= OnDiceResult;
        }

        private void Start()
        {
            SetSprites();
            SetButtons();

            if (buttonDiceAmount.transform.GetChild(2).TryGetComponent(out TMP_Text text))
                text.text = "x" + PlayerDataManager.PlayerData.diceAmount;

            if (TryGetComponent(out Animator animator)) _animator = animator;

            if (PlayerDataManager.MapOrder.Count <= 0)
            {
                menuGameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        #region Game Management

        public void LoadGame(bool isNew)
        {
            Actions.ButtonTapped?.Invoke();
            if (isNew) Actions.NewGame?.Invoke();
            else Actions.LoadGame?.Invoke();

            GameStartAnimation(true);
        }

        private void ReturnToMainMenu()
        {
            Actions.ButtonTapped?.Invoke();
            PlayerDataManager.SaveData();
            GameStartAnimation(false);
            DiceButtonAnimation(false);
            Actions.GameEnd?.Invoke();
        }

        #endregion

        #region UI Animations

        private void GameStartAnimation(bool isStarting)
        {
            _animator.SetTrigger(isStarting ? "StartGame" : "EndGame");
        }

        private void OpenBagAnimation(bool isOpen)
        {
            _animator.SetBool("BagOpened", isOpen);
        }

        private void DiceButtonAnimation(bool isOn)
        {
            _animator.SetBool("DBOn", isOn);
        }

        private void DiceAmountButtonAnimation()
        {
            _animator.SetBool("DAOn", true);
        }
        
        private void PlayDiceResultAnimation()
        {
            _animator.Play("Show_Dice_Result");
        }

        private void OnPrizeAddedToBag()
        {
            _animator.Play("Add_Item_To_Bag");
        }

        #endregion

        #region Audio Management

        private void ChangeAudioMod()
        {
            Actions.ButtonTapped?.Invoke();
            PlayerDataManager.PlayerData.isMuted = !PlayerDataManager.PlayerData.isMuted;
            SetAudio();
        }

        private void SetAudio()
        {
            _audioButtonImage.sprite = PlayerDataManager.PlayerData.isMuted ? _mute : _unmute;
            Actions.AudioChanged?.Invoke(PlayerDataManager.PlayerData.isMuted);
            PlayerDataManager.SaveData();
        }

        #endregion

        #region Dice Management

        private void RollDice()
        {
            Actions.ButtonTapped?.Invoke();
            Actions.RollDice?.Invoke(PlayerDataManager.PlayerData.diceAmount);
            DiceButtonAnimation(false);
        }

        private void OnDiceResult(int result)
        {
            textDiceResult.text = result.ToString();
            PlayDiceResultAnimation();
        }

        private void NextTurn()
        {
            DiceButtonAnimation(true);
        }

        private void OnDiceAmountChanged(int amount)
        {
            _animator.SetBool("DAOn", false);
            if (buttonDiceAmount.transform.GetChild(2).TryGetComponent(out TMP_Text text))
                text.text = "x" + amount;
        }

        public void UpdateDiceAmount()
        {
            int amount = transform.GetSiblingIndex() + 1;
            PlayerDataManager.UpdateDiceAmount(amount);
        }

        #endregion

        #region UI Setup

        private void SetSprites()
        {
            _mute = Resources.Load<Sprite>("UI/ui_icon_main_menu_mute");
            _unmute = Resources.Load<Sprite>("UI/ui_icon_main_menu_unmute");

            if (_mute == null || _unmute == null)
            {
                Debug.LogError("One or more sprites failed to load. Check paths and ensure assets are in Resources folder.");
            }

            _audioButtonImage = buttonAudio.transform.GetChild(0).GetComponent<Image>();
            SetAudio();
        }

        private void SetButtons()
        {
            buttonLoadGame.onClick.AddListener(() => LoadGame(false));
            buttonNewGame.onClick.AddListener(() => LoadGame(true));
            buttonDiceRoll.onClick.AddListener(RollDice);
            buttonReturnToMainMenu.onClick.AddListener(ReturnToMainMenu);
            buttonAudio.onClick.AddListener(ChangeAudioMod);
            openBag.onClick.AddListener(() => OpenBagAnimation(true));
            closeBag.onClick.AddListener(() => OpenBagAnimation(false));
            buttonDiceAmount.onClick.AddListener(DiceAmountButtonAnimation);
        }

        #endregion
    }
}
