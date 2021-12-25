namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using UnityEngine;

    public class ReaperProgressBarBehaviour : ProgressBarBehaviour
    {
        #region Members
        
        public float ReaperTimer { get; set; }

        private float CurrentReaperTime = 0;
        
        protected ReaperBehaviour Reaper { get; set; }

        #endregion

        #region Public Methods

        public void Reset()
        {
            CurrentReaperTime = 0;
            ReaperTimer = GameState.CurrentStage.ReaperAppearTime;

            Reaper
               .Reset();

            Reaper
                .gameObject
                .SetActive(false);
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Reaper = FindObjectOfType<ReaperBehaviour>();
            
            Reset();
        }

        protected void Update()
        {
            if (CurrentReaperTime < ReaperTimer)
            {
                CurrentReaperTime += Time.deltaTime;
                if (CurrentReaperTime >= ReaperTimer)
                {
                    CurrentReaperTime = ReaperTimer;
                    Reaper
                        .gameObject
                        .SetActive(true);
                }
                else
                {
                    Reaper
                        .gameObject
                        .SetActive(false);
                }

                SetValues(CurrentReaperTime, ReaperTimer);
            }
        }

        #endregion
    }
}
