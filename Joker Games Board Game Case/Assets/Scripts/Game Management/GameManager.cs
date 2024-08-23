using System.Collections.Generic;
using Data_Management;
using UnityEngine;
using Utils;

namespace Game_Management
{
    public class GameManager : MonoBehaviour
    {
        // Public static property to access the instance
        public static GameManager Instance { get; private set; }

        public bool isPlayable;
        public List<Transform> gameMap;

        // Ensure that the instance is unique and handle duplication
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;

            // Load player data
            PlayerDataManager.LoadData();
            PlayerDataManager.LoadMapOrder();
        }
        
        private void OnEnable()
        {
            Actions.GameStart += OnGameStart;
            Actions.GameEnd += OnGameEnd;
        }

        private void OnDisable()
        {
            
            Actions.GameStart -= OnGameStart;
            Actions.GameEnd -= OnGameEnd;
        }

        private void OnGameStart()
        {
            var player =Pool.Instance.SpawnObject(gameMap[PlayerDataManager.PlayerData.currentGrid].transform.position, PoolItemType.Player, null);
            if (Camera.main != null) Camera.main.GetComponent<SmoothCameraFollow>().target = player.transform;
            isPlayable = true;
        }
        
        private void OnGameEnd()
        {
            if (Camera.main != null) Camera.main.GetComponent<SmoothCameraFollow>().target = null;
            isPlayable = false;
        }
    }
}
