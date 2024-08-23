using System;
using System.Collections;
using Data_Management;
using Game_Management;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Item_Management
{
    public class PrizeCollector : MonoBehaviour
    {
        public RectTransform bagIcon; // Reference to the bag icon in the UI
        public float spawnRadius; // Radius around player to spawn prizes
        public float moveDuration; // Duration to move to the bag icon
        public float delayBetweenMoves; // Delay between each prize move
        public float randomMoveSpeed; // Speed for random movement during delay
        public float randomMoveRadius; // How far the prize can move randomly

        private void OnEnable()
        {
            Actions.PrizesAppeared += CollectPrizes;
        }

        private void OnDisable()
        {
            Actions.PrizesAppeared -= CollectPrizes;
        }
        
        // Call this method to collect prizes
        private void CollectPrizes(Transform player, int prizeAmount, ItemType prizeType)
        {
            // Convert player's world position to screen position
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);

            // Spawn multiple prizes
            for (int i = 0; i < prizeAmount; i++)
            {
                Vector3 randomPos = GetRandomPosition(screenPos);
                var prize = Pool.Instance.SpawnObject(randomPos, PoolItemType.Prize, transform);
                if (prize.TryGetComponent(out Image image))
                    image.sprite = PlayerDataManager.GetCertainItemSprite(prizeType);

                // Start coroutine to move the prize randomly away from the player, then move it to the bag icon
                StartCoroutine(RandomMoveAndMoveToBag(prize.transform, screenPos, delayBetweenMoves));
                delayBetweenMoves += 0.1f;
            }
        }

        // Get a random position around the player in screen space
        private Vector3 GetRandomPosition(Vector3 screenPos)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            float distance = Random.Range(0f, spawnRadius);
            return screenPos + new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
        }

        // Coroutine for random movement away from the player before moving to the bag
        private IEnumerator RandomMoveAndMoveToBag(Transform prize, Vector3 playerScreenPos, float waitTime)
        {
            Vector3 startPosition = prize.position;
            float elapsedTime = 0f;

            // Calculate direction away from the player
            Vector3 directionAwayFromPlayer = (prize.position - playerScreenPos).normalized;
            Vector3 randomTargetPosition = startPosition + directionAwayFromPlayer * randomMoveRadius;

            // Random movement while waiting
            while (elapsedTime < waitTime)
            {
                prize.position = Vector3.Lerp(startPosition, randomTargetPosition, elapsedTime / waitTime);
                elapsedTime += Time.deltaTime * randomMoveSpeed;
                yield return null;
            }

            // After random movement, move to the bag
            yield return MovePrizeToBag(prize);
        }

        // Coroutine to move the prize to the bag icon
        private IEnumerator MovePrizeToBag(Transform prize)
        {
            Vector3 startPosition = prize.position;
            Vector3 endPosition = bagIcon.position;

            float elapsedTime = 0f;
            while (elapsedTime < moveDuration)
            {
                prize.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            prize.position = endPosition;

            // After reaching the bag, deactivate the prize
            Actions.PrizeAddedToBag?.Invoke();
            Pool.Instance.DeactivateObject(prize.gameObject, PoolItemType.Prize);
        }
    }
}
