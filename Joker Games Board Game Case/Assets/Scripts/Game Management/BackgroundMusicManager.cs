using System.Collections.Generic;
using Data_Management;
using UnityEngine;

namespace Game_Management
{
    public class BackgroundMusicManager : MonoBehaviour
    {
        public int currentMusicIndex = -1;
        public AudioSource audioSource;
        public List<AudioClip> musicClip;

        [SerializeField] private float volume;

        private void OnEnable()
        {
            Actions.AudioChanged += SetAudioMod;

            SetAudioMod(PlayerDataManager.PlayerData.isMuted);
        }
        
        private void OnDisable()
        {
            Actions.AudioChanged -= SetAudioMod;
        }
        
        private void Start()
        {
            NextBackgroundMusic();
        }
        
        private void SetAudioMod(bool isMuted)
        {
            audioSource.volume = isMuted ? 0 : volume;
        }

        public void ChangeBackgroundMusic(int index)
        {
            audioSource.clip = musicClip[index];
            audioSource.Play();
        }

        private void NextBackgroundMusic()
        {
            if (currentMusicIndex == musicClip.Count-1) currentMusicIndex = 0;
            else currentMusicIndex++;
            audioSource.pitch = 1f / Time.timeScale;
            audioSource.clip = musicClip[currentMusicIndex];
            audioSource.Play();
        }
    }
}
