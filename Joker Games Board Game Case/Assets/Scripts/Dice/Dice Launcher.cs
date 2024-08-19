using System.Collections;
using UnityEngine;
using Utils;

namespace Dice
{
    public class DiceLauncher : MonoBehaviour
    {
        private Pool _pool;
        
        // Start is called before the first frame update
        private void Start()
        {
            _pool = Pool.Instance;
            //StartCoroutine(AutomaticDiceLauncher());
            int amount = Random.Range(1, 6);
            LaunchDice(amount);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private Vector3 SelectRandomSpawnPoint()
        {
            Vector3 defaultPos = transform.position;
            float x = Random.Range(defaultPos.x - 3, defaultPos.x + 3);
            return new Vector3(x, defaultPos.y, defaultPos.z);
        }

        public void LaunchDice(int diceAmount)
        {
            for (var i = 0; i < diceAmount; i++)
            {
                _pool.SpawnObject(SelectRandomSpawnPoint(), PoolItemType.Dice, null);
            }
        }

        private IEnumerator AutomaticDiceLauncher()
        {
            while (true)
            {
                int amount = Random.Range(1, 6);
                LaunchDice(amount);
                10.0f.GetWait();
            }
        }
    }
}
