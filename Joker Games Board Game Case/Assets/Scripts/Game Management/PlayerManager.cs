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
        void Start()
        {
            _gameManager = GameManager.Instance;
            _gameManager.SetPlayer(this);
            _currentGrid = PlayerDataManager.PlayerData.currentGrid;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void SetCurrentGrid()
        {
            PlayerDataManager.PlayerData.currentGrid = _currentGrid;
            PlayerDataManager.SaveData();
        }

        public IEnumerator MoveForward(int moveTimes)
        {
            for (var i = 0; i < moveTimes; i++) 
            {
                _currentGrid += 1; 
                transform.position = _gameManager.gameMap[_currentGrid].position; 
                0.1f.GetWait();
            }

            SetCurrentGrid();
            Actions.NextTurn?.Invoke();
            yield return null;
        }
    }
}
