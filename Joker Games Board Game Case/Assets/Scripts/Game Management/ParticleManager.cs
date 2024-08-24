using System;
using System.Collections.Generic;
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

        // Dictionary to cache ParticleType to PoolItemType mapping
        private Dictionary<ParticleType, PoolItemType> _particleTypeMapping;

        // Ensure that the instance is unique and handle duplication
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;

            // Initialize the dictionary
            _particleTypeMapping = new Dictionary<ParticleType, PoolItemType>
            {
                { ParticleType.LightPuff, PoolItemType.LightPuffParticle },
                { ParticleType.Puff, PoolItemType.PuffParticle },
                { ParticleType.Teleported, PoolItemType.TeleportedParticle },
                { ParticleType.BeforeTeleportation, PoolItemType.BeforeTeleportationParticle },
                { ParticleType.AfterTeleportation, PoolItemType.AfterTeleportationParticle }
            };
        }

        private void Start()
        {
            _pool = Pool.Instance;
        }

        public void CallParticle(ParticleType particleType, Vector3 position, float time = 1)
        {
            // Check if the particle type exists in the dictionary
            if (_particleTypeMapping.TryGetValue(particleType, out PoolItemType poolItemType))
            {
                _pool.SpawnObject(position, poolItemType, null, time);
            }
            else
            {
                Debug.LogError($"Unhandled particle type: {particleType}");
                throw new ArgumentOutOfRangeException(nameof(particleType), particleType, null);
            }
        }
    }
}
