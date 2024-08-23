using System.Collections;
using Data_Management;
using Game_Management;
using UnityEngine;
using UnityEngine.UI;

namespace Item_Management
{
    public class PrizeCollector : MonoBehaviour
    {
        public RectTransform bagIcon; // Reference to the bag icon in the UI
        public float spawnRadius; // Radius around player to spawn prizes
        public float moveDuration; // Duration to move to the bag icon
        public float delayBetweenMoves; // Delay between each prize move

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

                // Start coroutine to move the prize to the bag icon
                StartCoroutine(MovePrizeToBag(prize.transform));
            }
        }

        // Get a random position around the player in screen space
        private Vector3 GetRandomPosition(Vector3 screenPos)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            float distance = Random.Range(0f, spawnRadius);
            return screenPos + new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
        }

        // Coroutine to move the prize to the bag icon
        private IEnumerator MovePrizeToBag(Transform prize)
        {
            // Wait for a little before moving the prize
            yield return new WaitForSeconds(delayBetweenMoves);

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
            Pool.Instance.DeactivateObject(prize.gameObject, PoolItemType.Prize);
        }
    }
}
