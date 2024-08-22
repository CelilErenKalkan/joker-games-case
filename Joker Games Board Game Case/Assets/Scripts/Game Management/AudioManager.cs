using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Management
{
    public class AudioManager : MonoBehaviour
    {
        private Pool _pool;
        public List<AudioClip> soundList;

        private void OnEnable()
        {
            Actions.ButtonTapped += OnButtonTapped;
            Actions.GameStart += OnGameStart;
            Actions.NextTurn += OnNextTurn;
            Actions.GridAppeared += OnGridAppeared;
        }
        
        private void OnDisable()
        {
            Actions.ButtonTapped -= OnButtonTapped;
            Actions.GameStart -= OnGameStart;
            Actions.NextTurn -= OnNextTurn;
            Actions.GridAppeared -= OnGridAppeared;
        }

        private void Start()
        {
            _pool = Pool.Instance;
        }

        private void OnButtonTapped()
        {
            PlaySound(0);
        }

        private void OnGameStart()
        {
            PlaySound(1);
        }

        private void OnNextTurn()
        {
            PlaySound(2);
        }
        
        private void OnGridAppeared()
        {
            PlaySound(3);
            //PlaySound(4);
        }

        private void PlaySound(int index)
        {
            float time = soundList[index].length + 0.1f;
            var audioObject = Pool.Instance.SpawnObject(transform.position, PoolItemType.AudioSource, null, time);
            if (audioObject.TryGetComponent(out AudioSource audioSource))
            {
                audioSource.clip = soundList[index];
                audioSource.Play();
            }
        }
    }
    
}
