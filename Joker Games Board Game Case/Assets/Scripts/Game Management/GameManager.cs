using System.Collections.Generic;
using Data_Management;
using UnityEngine;
using Utils;

namespace Game_Management
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public bool isPlayable;
        public List<Transform> gameMap;

        private void Awake()
        {
            HandleSingletonInstance();
            LoadGameData();
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

        #region Game Lifecycle

        private void OnGameStart()
        {
            // Spawn the player at the current grid position
            var player = Pool.Instance.SpawnObject(gameMap[PlayerDataManager.PlayerData.currentGrid].transform.position, PoolItemType.Player, null);

            // Assign the camera to follow the player
            if (Camera.main != null)
            {
                Camera.main.GetComponent<SmoothCameraFollow>().target = player.transform;
            }

            isPlayable = true;
        }

        private void OnGameEnd()
        {
            // Reset camera target and set the game as unplayable
            if (Camera.main != null)
            {
                Camera.main.GetComponent<SmoothCameraFollow>().target = null;
            }

            isPlayable = false;
        }

        #endregion

        private void HandleSingletonInstance()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void LoadGameData()
        {
            // Load player data and map order
            PlayerDataManager.LoadData();
            PlayerDataManager.LoadMapOrder();
        }
    }
}
