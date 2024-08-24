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
        private ParticleManager _particleManager;
        private Animator _animator;
        private int _currentGrid;
        private int _moveCount;

        private void OnEnable()
        {
            _gameManager = GameManager.Instance;
            _particleManager = ParticleManager.Instance;
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

        #region Particle Management

        private void CallParticle(ParticleType particleType, float y = 0.5f, float time = 1)
        {
            var position = transform.position;
            position.y += y;
            _particleManager.CallParticle(particleType, position, time);
        }

        private void CallLightPuffParticle() => CallParticle(ParticleType.LightPuff);

        private void CallPuffParticle() => CallParticle(ParticleType.Puff);

        private void CallTeleportParticle() => CallParticle(ParticleType.Teleported, 1.0f);

        private void CallBeforeTeleportationParticle() => CallParticle(ParticleType.BeforeTeleportation, 1.0f, 2.0f);

        private void CallAfterTeleportationParticle() => CallParticle(ParticleType.BeforeTeleportation, 1.0f);

        #endregion

        #region Event Management

        private void PlayerStepped() => Actions.PlayerStep?.Invoke();

        private void PlayerFinalStepped() => Actions.PlayerFinalStep?.Invoke();

        private void PlayerTeleportIn() => Actions.PlayerTeleportIn?.Invoke();

        private void PlayerTeleportOut() => Actions.PlayerTeleportOut?.Invoke();

        #endregion

        #region Movement and Rotation

        private void RotateToNextGrid()
        {
            if (_currentGrid < _gameManager.gameMap.Count - 1)
            {
                var target = _gameManager.gameMap[_currentGrid + 1];
                Vector3 directionToTarget = target.position - transform.position;
                directionToTarget.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = targetRotation;
            }
        }

        private void Move()
        {
            RotateToNextGrid();
            _animator.Play(_moveCount > 1 ? "Jump_To_New_Grid" : "Final_Jump");
        }

        private IEnumerator RotateAfterJump()
        {
            yield return new WaitForEndOfFrame();
            RotateToNextGrid();
        }

        private IEnumerator MoveAfterTeleport()
        {
            yield return new WaitForEndOfFrame();
            _currentGrid = 0;
            transform.position = _gameManager.gameMap[_currentGrid].position;
            _moveCount--;
            RotateToNextGrid();
            yield return 1.0f.GetWait();
            _animator.SetTrigger("TeleportOut");
        }

        public void GoToInitialGrid() => StartCoroutine(MoveAfterTeleport());

        public void ResetMove() => OnMoveForward(_moveCount - 1);

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

        #endregion

        #region Item Collection and Saving

        private void CollectItems()
        {
            int amount = PlayerDataManager.MapOrder[_currentGrid].itemAmount;
            ItemType type = PlayerDataManager.MapOrder[_currentGrid].itemType;
            if (type == ItemType.Empty) return;
            Actions.PrizesAppeared.Invoke(transform, amount, type);
            PlayerDataManager.Collect();
        }

        private void SetCurrentGrid() => PlayerDataManager.PlayerData.currentGrid = _currentGrid;

        #endregion

        private void OnMoveForward(int diceResult)
        {
            _moveCount = diceResult;
            Move();
        }

        private void OnGameEnd() => Pool.Instance.DeactivateObject(gameObject, PoolItemType.Player);
    }
}
