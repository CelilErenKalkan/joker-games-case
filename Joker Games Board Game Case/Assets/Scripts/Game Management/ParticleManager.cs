using System;
using UnityEngine;

namespace Game_Management
{
    public enum ParticleType
    {
        LightPuff,
        Puff,
        Teleported,
        BeforeTeleportation,
        AfterTeleportation
    }

    public class ParticleManager : MonoBehaviour
    {
        // Public static property to access the instance
        public static ParticleManager Instance { get; private set; }

        private Pool _pool;

        // Ensure that the instance is unique and handle duplication
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
        }

        private void Start()
        {
            _pool = Pool.Instance;
        }

        public void CallParticle(ParticleType particleType, Vector3 position, float time = 1)
        {
            switch (particleType)
            {
                case ParticleType.LightPuff:
                    _pool.SpawnObject(position,PoolItemType.LightPuffParticle, null, time);
                    break;
                case ParticleType.Puff:
                    _pool.SpawnObject(position,PoolItemType.PuffParticle, null, time);
                    break;
                case ParticleType.Teleported:
                    _pool.SpawnObject(position,PoolItemType.TeleportedParticle, null, time);
                    break;
                case ParticleType.BeforeTeleportation:
                    _pool.SpawnObject(position,PoolItemType.BeforeTeleportationParticle, null, time);
                    break;
                case ParticleType.AfterTeleportation:
                    _pool.SpawnObject(position,PoolItemType.AfterTeleportationParticle, null, time);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(particleType), particleType, null);
            }
        }
    }
}
