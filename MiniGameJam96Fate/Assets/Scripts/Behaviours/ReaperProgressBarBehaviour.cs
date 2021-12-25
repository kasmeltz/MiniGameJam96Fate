namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    public class ReaperProgressBarBehaviour : ProgressBarBehaviour
    {
        #region Members
        
        public float ReaperTimer = 0;

        private float CurrentReaperTime = 0;
        
        protected ReaperBehaviour Reaper { get; set; }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Reaper = FindObjectOfType<ReaperBehaviour>();
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

        public void Reset()
        {
            Reaper
                .Reset();

            Reaper
                .gameObject
                .SetActive(false);

            CurrentReaperTime = 0;
        }

        #endregion
    }
}
