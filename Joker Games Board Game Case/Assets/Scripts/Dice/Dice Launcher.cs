using System.Collections.Generic;
using Data_Management;
using Game_Management;
using UnityEngine;

namespace Dice
{
    public class DiceLauncher : MonoBehaviour
    {
        private Pool _pool;
        private int _totalResult, _diceCount;
        private List<GameObject> _spawnedDices;

        private void OnEnable()
        {
            Actions.RollDice += LaunchDice;
            Actions.DiceResult += ResultCalculator;
        }

        private void OnDisable()
        {
            Actions.RollDice -= LaunchDice;
            Actions.DiceResult -= ResultCalculator;
        }
        
        private void Start()
        {
            _pool = Pool.Instance;
            _spawnedDices = new List<GameObject>();
        }

        // Selects a random spawn point near the current transform's position
        private Vector3 SelectRandomSpawnPoint()
        {
            Vector3 defaultPos = transform.position;
            float x = Random.Range(defaultPos.x - 3, defaultPos.x + 3);
            return new Vector3(x, defaultPos.y, defaultPos.z);
        }

        // Launches the specified amount of dice
        private void LaunchDice(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var dice = _pool.SpawnObject(SelectRandomSpawnPoint(), PoolItemType.Dice, null);
                _spawnedDices.Add(dice);
            }
            Time.timeScale = 2; // Increases time scale for faster dice rolling
        }

        // Removes all spawned dice and returns them to the pool
        private void RemoveAllDices()
        {
            while (_spawnedDices.Count > 0)
            {
                _pool.DeactivateObject(_spawnedDices[0], PoolItemType.Dice);
                _spawnedDices.RemoveAt(0);
            }
        }
        
        // Calculates the total result of the dice roll and moves the player if all dice have been rolled
        private void ResultCalculator(int diceResult)
        {
            _totalResult += diceResult;
            _diceCount++;

            if (_diceCount >= PlayerDataManager.PlayerData.diceAmount)
            {
                Time.timeScale = 1; // Resets time scale after dice rolling is complete
                RemoveAllDices();
                Actions.MoveForward?.Invoke(_totalResult);
                _diceCount = 0;
                _totalResult = 0;
            }
        }
    }
}
