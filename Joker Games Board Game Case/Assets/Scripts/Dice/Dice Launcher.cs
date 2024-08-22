using Game_Management;
using UnityEngine;

namespace Dice
{
    public class DiceLauncher : MonoBehaviour
    {
        private Pool _pool;
        
        private void OnEnable()
        {
            Actions.RollDice += LaunchDice;
        }

        private void OnDisable()
        {
            Actions.RollDice -= LaunchDice;
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            _pool = Pool.Instance;
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
                _pool.SpawnObject(SelectRandomSpawnPoint(), PoolItemType.Dice, null);
            }
        }
    }
}
