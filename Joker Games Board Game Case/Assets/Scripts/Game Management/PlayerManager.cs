using System.Collections;
using Data_Management;
using UnityEngine;
using Utils;

namespace Game_Management
{
    public class PlayerManager : MonoBehaviour
    {
        private GameManager _gameManager;
        private int _currentGrid;

        // Start is called before the first frame update
        private void OnEnable()
        {
            _gameManager = GameManager.Instance;
            _currentGrid = PlayerDataManager.PlayerData.currentGrid;
        }

        private void CollectItems()
        {
            PlayerDataManager.Collect();
        }
        
        private void SetCurrentGrid()
        {
            PlayerDataManager.PlayerData.currentGrid = _currentGrid;
        }

        public IEnumerator MoveForward(int moveTimes)
        {
            for (var i = 0; i < moveTimes; i++) 
            {
                _currentGrid += 1;
                if (_currentGrid >= _gameManager.gameMap.Count) _currentGrid = 0;
                transform.position = _gameManager.gameMap[_currentGrid].position;
                Debug.Log("Entered");
                0.5f.GetWait();
            }

            SetCurrentGrid();
            CollectItems();
            PlayerDataManager.SaveData();
            Actions.NextTurn?.Invoke();
            yield return null;
        }
    }
}
