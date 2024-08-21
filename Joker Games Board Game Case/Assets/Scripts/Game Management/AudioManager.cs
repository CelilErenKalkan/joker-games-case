using System.Collections;
using System.Collections.Generic;
using Data_Management;
using UnityEngine;

namespace Game_Management
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public List<AudioClip> soundList;

        public void PlayOneShot(int index, float volumeScale = 1)
        {
            audioSource.PlayOneShot(soundList[index], volumeScale);
        }

        private void OnEnable()
        {
            Actions.AudioChanged += SetAudioMod;
            Actions.ButtonTapped += OnButtonTapped;
            Actions.GameStart += OnGameStart;
            Actions.NextTurn += OnNextTurn;
            
            SetAudioMod(PlayerDataManager.PlayerData.isMuted);
        }
        
        private void OnDisable()
        {
            Actions.AudioChanged -= SetAudioMod;
            Actions.ButtonTapped -= OnButtonTapped;
            Actions.GameStart -= OnGameStart;
            Actions.NextTurn -= OnNextTurn;
        }

        private void SetAudioMod(bool isMuted)
        {
            audioSource.volume = isMuted? 0 : 1;
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

        public void PlaySound(int index)
        {
            audioSource.clip = soundList[index];
            audioSource.Play();
        }

        public IEnumerator PlaySoundWithTime(float time, int index)
        {
            yield return new WaitForSeconds(time);
            audioSource.clip = soundList[index];
            audioSource.Play();
        }

        public IEnumerator PlayTwoSound(int firstSound, int secondSound)
        {
            audioSource.clip = soundList[firstSound];
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            audioSource.clip = soundList[secondSound];
            audioSource.Play();
        }
    }
    
}
