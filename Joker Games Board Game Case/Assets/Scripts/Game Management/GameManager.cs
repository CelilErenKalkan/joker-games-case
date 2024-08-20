using System;
using System.Collections.Generic;
using Data_Management;
using UnityEngine;

namespace Game_Management
{
    public class GameManager : MonoBehaviour
    {
        // Public static property to access the instance
        public static GameManager Instance { get; private set; }

        public bool isPlayable;

        private int _totalResult, _diceCount;
        private PlayerManager _player;
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
            Actions.DiceResult += ResultCalculator;
            Actions.GameStart += GameStart;
        }

        private void OnDisable()
        {
            Actions.DiceResult -= ResultCalculator;
            Actions.GameStart -= GameStart;
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            
        }

        // Update is called once per frame
        private void Update()
        {
        
        }

        private void GameStart()
        {
            Pool.Instance.SpawnObject(gameMap[PlayerDataManager.PlayerData.currentGrid].transform.position, PoolItemType.Player, null);
            isPlayable = true;
        }

        public void SetPlayer(PlayerManager player)
        {
            _player = player;
        }

        private void ResultCalculator(int diceResult)
        {
            _totalResult += diceResult;
            _diceCount++;

            if (_diceCount >= PlayerDataManager.PlayerData.diceAmount)
            {
                StartCoroutine(_player.MoveForward(PlayerDataManager.PlayerData.diceAmount));
                _diceCount = 0;
            }
        }
    }
}
