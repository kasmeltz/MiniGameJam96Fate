namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("HairyNerd/MGJ96/PlaySoundEffect")]
    public class PlaySoundEffectBehaviour : BehaviourBase
    {
        public SoundEffectEnum SoundEffect;
        public float SoundVolume;
               
        #region Public Methods

        public void Play()
        {
            SoundEffectPlayerBehaviour soundEffectPlayer = FindObjectOfType<SoundEffectPlayerBehaviour>();

            soundEffectPlayer.Play(SoundEffect, SoundVolume);
        }

        #endregion
    }
}