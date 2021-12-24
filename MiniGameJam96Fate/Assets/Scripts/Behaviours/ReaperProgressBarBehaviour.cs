using UnityEngine;
using UnityEngine.UI;

namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours{
    public class ReaperProgressBarBehaviour : BehaviourBase
    {
        #region Members
        public float ReaperTimer = 0;

        private float CurrentReaperTime = 0;

        public Image Background;
        public Image Foreground;

        [SerializeField] private GameObject Reaper = null;
        #endregion
       

      

        // Update is called once per frame
        void Update()
        {
            if (ReaperTimer > CurrentReaperTime)
            {
                CurrentReaperTime += Time.deltaTime;
                Foreground.fillAmount = CurrentReaperTime / ReaperTimer;
                if (CurrentReaperTime > ReaperTimer)
                {
                    CurrentReaperTime = ReaperTimer;
                    Reaper.SetActive(true);
                }
                else
                {
                    Reaper.SetActive(false);
                }


            }
        }

        public void Reset()
        {
            Reaper.SetActive(false);
            CurrentReaperTime = 0;
        }
    }

}


