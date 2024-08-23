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
        
        // Start is called before the first frame update
        private void Start()
        {
            _pool = Pool.Instance;
            _spawnedDices = new List<GameObject>();
        }

        private Vector3 SelectRandomSpawnPoint()
        {
            Vector3 defaultPos = transform.position;
            float x = Random.Range(defaultPos.x - 3, defaultPos.x + 3);
            return new Vector3(x, defaultPos.y, defaultPos.z);
        }

        private void LaunchDice(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var dice = _pool.SpawnObject(SelectRandomSpawnPoint(), PoolItemType.Dice, null);
                _spawnedDices.Add(dice);
            }
            Time.timeScale = 3;
        }

        private void RemoveAllDices()
        {
            var count = _spawnedDices.Count;
            for (var i = 0; i < count; i++)
            {
                _pool.DeactivateObject(_spawnedDices[0], PoolItemType.Dice);
                _spawnedDices.RemoveAt(0);
            }
        }
        
        private void ResultCalculator(int diceResult)
        {
            _totalResult += diceResult;
            _diceCount++;

            if (_diceCount >= PlayerDataManager.PlayerData.diceAmount)
            {
                Time.timeScale = 1;
                RemoveAllDices();
                Actions.MoveForward?.Invoke(_totalResult);
                _diceCount = 0;
                _totalResult = 0;
            }
        }
    }
}
