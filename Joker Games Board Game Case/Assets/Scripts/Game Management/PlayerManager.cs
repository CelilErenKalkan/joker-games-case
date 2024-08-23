using System.Collections;
using Data_Management;
using Item_Management;
using UnityEngine;
using Utils;

namespace Game_Management
{
    public class PlayerManager : MonoBehaviour
    {
        private GameManager _gameManager;
        private int _currentGrid;
        private Animator _animator;
        private int _moveCount;

        // Start is called before the first frame update
        private void OnEnable()
        {
            _gameManager = GameManager.Instance;
            _currentGrid = PlayerDataManager.PlayerData.currentGrid;
            if (TryGetComponent(out Animator animator)) _animator = animator;

            Actions.MoveForward += OnMoveForward;
            Actions.GameEnd += OnGameEnd;
            Actions.NextTurn?.Invoke();
        }
        
        private void OnDisable()
        {
            Actions.MoveForward -= OnMoveForward;
            Actions.GameEnd -= OnGameEnd;
        }

        private void CollectItems()
        {
            int amount = PlayerDataManager.MapOrder[_currentGrid].itemAmount;
            ItemType type = PlayerDataManager.MapOrder[_currentGrid].itemType;
            if (type == ItemType.Empty) return;
            Actions.PrizesAppeared.Invoke(transform, amount, type);
            PlayerDataManager.Collect();
        }
        
        private void SetCurrentGrid()
        {
            PlayerDataManager.PlayerData.currentGrid = _currentGrid;
        }

        private void RotateToNextGrid()
        {
            // Ensure that we're not at the last grid and we have a next grid to rotate towards
            if (_currentGrid < _gameManager.gameMap.Count - 1)
            {
                var target = _gameManager.gameMap[_currentGrid + 1];

                // Calculate the direction from the current object to the target object
                Vector3 directionToTarget = target.position - transform.position;

                // Set y to zero to ignore vertical rotation
                directionToTarget.y = 0;

                // Calculate the rotation to face the target
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                // Apply rotation to the current object
                transform.rotation = targetRotation;
            }
        }

        private void Move()
        {
            // Rotate before initiating the jump to ensure the object faces the next grid
            RotateToNextGrid();
            //_animator.SetTrigger(_moveCount > 1 ? "Jump" : "FinalJump");
            _animator.Play(_moveCount > 1 ? "Jump_To_New_Grid" : "Final_Jump");
        }
        
        private IEnumerator RotateAfterJump()
        {
            // Wait for the end of the current frame to ensure the animation is fully finished
            yield return new WaitForEndOfFrame();

            // Now rotate to the next grid
            RotateToNextGrid();
        }
        
        private IEnumerator MoveAfterTeleport()
        {
            // Wait for the end of the current frame to ensure the animation is fully finished
            yield return new WaitForEndOfFrame();

            _currentGrid = 0;
            transform.position = _gameManager.gameMap[_currentGrid].position;
            _moveCount--;
            RotateToNextGrid();

            yield return 1.0f.GetWait();
            _animator.SetTrigger("TeleportOut");
        }

        public void GoToInitialGrid()
        {
            StartCoroutine(MoveAfterTeleport());
        }

        public void ResetMove()
        {
            OnMoveForward(_moveCount - 1);
        }

        public void MoveToNextGrid()
        {
            _moveCount--;
            if (_currentGrid + 2 >= _gameManager.gameMap.Count)
            {
                _animator.SetTrigger("TeleportIn");
            }
            else
            {
                _currentGrid++;

                // Wait for the animation to finish before rotating
                StartCoroutine(RotateAfterJump());

                if (_moveCount > 0)
                {
                    Move();
                }
                else
                {
                    transform.position = _gameManager.gameMap[_currentGrid].position;
                    SetCurrentGrid();
                    CollectItems();
                    PlayerDataManager.SaveData();
                    Actions.NextTurn?.Invoke();
                }
            }
        }

        private void OnMoveForward(int diceResult)
        {
            _moveCount = diceResult;
            Move();
        }

        private void OnGameEnd()
        {
            Pool.Instance.DeactivateObject(gameObject, PoolItemType.Player);
        }
    }
}
