using System;
using System.Collections.Generic;
using Data_Management;
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
            Actions.GridHasFallen += OnGridHasFallen;
            Actions.PlayerStep += OnPlayerStep;
            Actions.PlayerFinalStep += OnPlayerFinalStep;
            Actions.PlayerTeleportIn += OnPlayerTeleportIn;
            Actions.PlayerTeleportOut += OnPlayerTeleportOut;
            Actions.DiceToDiceCollision += OnDiceToDiceCollision;
            Actions.DiceToFloorCollision += OnDiceToFloorCollision;
            Actions.PrizeAddedToBag += OnPrizeAddedToBag;
        }
        
        private void OnDisable()
        {
            Actions.ButtonTapped -= OnButtonTapped;
            Actions.GameStart -= OnGameStart;
            Actions.NextTurn -= OnNextTurn;
            Actions.GridAppeared -= OnGridAppeared;
            Actions.GridHasFallen -= OnGridHasFallen;
            Actions.PlayerStep -= OnPlayerStep;
            Actions.PlayerFinalStep -= OnPlayerFinalStep;
            Actions.PlayerTeleportIn -= OnPlayerTeleportIn;
            Actions.PlayerTeleportOut -= OnPlayerTeleportOut;
            Actions.DiceToDiceCollision -= OnDiceToDiceCollision;
            Actions.DiceToFloorCollision -= OnDiceToFloorCollision;
            Actions.PrizeAddedToBag -= OnPrizeAddedToBag;
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
        }
        
        private void OnGridHasFallen()
        {
            PlaySound(4);
        }
        
        private void OnPlayerStep()
        {
            PlaySound(5);
        }

        private void OnPlayerFinalStep()
        {
            PlaySound(6);
        }
        
        private void OnPlayerTeleportIn()
        {
            PlaySound(7);
        }
        
        private void OnPlayerTeleportOut()
        {
            PlaySound(8);
        }
        
        private void OnDiceToDiceCollision()
        {
            PlaySound(9);
        }
        
        private void OnDiceToFloorCollision()
        {
            PlaySound(10);
        }
        
        private void OnPrizeAddedToBag()
        {
            PlaySound(11);
        }

        private void PlaySound(int index)
        {
            if (PlayerDataManager.PlayerData.isMuted) return;
            
            float time = soundList[index].length + 0.1f;
            var audioObject = _pool.SpawnObject(transform.position, PoolItemType.AudioSource, null, time);
            if (audioObject.TryGetComponent(out AudioSource audioSource))
            {
                audioSource.pitch = 1f / Time.timeScale;
                audioSource.clip = soundList[index];
                audioSource.Play();
            }
        }
    }
    
}
