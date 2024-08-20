using System;
using Data_Management;
using UnityEngine;

namespace Game_Management
{
    public class GameManager : MonoBehaviour
    {
        // Public static property to access the instance
        public static GameManager Instance { get; private set; }

        public bool isPlayable;

        // Ensure that the instance is unique and handle duplication
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;

            // Load player data
            PlayerDataManager.LoadData();
        }
        
        // Start is called before the first frame update
        void Start()
        {
            isPlayable = true;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
