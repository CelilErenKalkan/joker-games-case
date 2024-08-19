using System;
using Data_Management;
using UnityEngine;

namespace Game_Management
{
    public class GameManager : MonoBehaviour
    {
        // Public static property to access the instance
        public static GameManager Instance;

        public bool isPlayable;

        // Ensure that the instance is unique and handle duplication
        private void Awake()
        {
            // Check if an instance already exists
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Optional: Prevent destruction on scene load
            }
            else if (Instance != this)
            {
                Destroy(gameObject); // Destroy duplicate
            }

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
