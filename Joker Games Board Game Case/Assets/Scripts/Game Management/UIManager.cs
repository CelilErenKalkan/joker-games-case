using Data_Management;
using UnityEngine;

namespace Game_Management
{
    public class UIManager : MonoBehaviour
    {
        // Public static property to access the instance
        public static UIManager Instance { get; private set; }
        public GameObject panelMainGameObject, menuGameObject;
    

        // Ensure that the instance is unique and handle duplication
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
        }

        public void NewGame()
        {
            menuGameObject.SetActive(false);
            panelMainGameObject.SetActive(false);
            Actions.NewGame?.Invoke();
        }
        
        public void LoadGame()
        {
            menuGameObject.SetActive(false);
            panelMainGameObject.SetActive(false);
            Actions.LoadGame?.Invoke();
        }
    
        // Start is called before the first frame update
        private void Start()
        {
            if (PlayerDataManager.mapOrder.Count <= 0)
            {
                menuGameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
