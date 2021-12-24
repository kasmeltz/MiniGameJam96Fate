namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    public class ReaperProgressBarBehaviour : ProgressBarBehaviour
    {
        #region Members
        
        public float ReaperTimer = 0;

        private float CurrentReaperTime = 0;

        [SerializeField] private GameObject Reaper = null;

        #endregion

        #region Unity

        protected void Update()
        {
            if (CurrentReaperTime < ReaperTimer)
            {
                CurrentReaperTime += Time.deltaTime;
                if (CurrentReaperTime >= ReaperTimer)
                {
                    CurrentReaperTime = ReaperTimer;
                    Reaper
                        .SetActive(true);
                }
                else
                {
                    Reaper
                        .SetActive(false);
                }

                SetValues(CurrentReaperTime, ReaperTimer);
            }
        }

        public void Reset()
        {
            Reaper
                .SetActive(false);

            CurrentReaperTime = 0;
        }

        #endregion
    }
}
