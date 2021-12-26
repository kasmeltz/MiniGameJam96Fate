namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    #region SoundEffectEnum 

    public enum SoundEffectEnum
    {
        DoorWontOpen,
        KeyCollect,
        OrbCollect,
        OrbFreeze,
        ReaperAppear,
        ReaperDisappear,
        ReaperBreathing,
        LoseStinger,
        BreakWall,
        UpgradeButton,
        StartButton,
        CoinCollect,
        WinStinger
    }

    #endregion

    [AddComponentMenu("HairyNerd/MGJ96/SoundEffectPlayer")]
    public class SoundEffectPlayerBehaviour : BehaviourBase
    {        
        #region Members

        public AudioSource AudioSource;

        public AudioClip[] Clips;

        public bool IsPlaying
        {
            get
            {
                return AudioSource.isPlaying;
            }
        }

        #endregion

        #region Public Methods

        public void Play(SoundEffectEnum soundEffect, float volume)
        {
            AudioSource
                .PlayOneShot(Clips[(int)soundEffect], volume);
        }

        #endregion

        protected override void Awake()
        {
            base
                .Awake();

            DontDestroyOnLoad(this);
        }
    }
}