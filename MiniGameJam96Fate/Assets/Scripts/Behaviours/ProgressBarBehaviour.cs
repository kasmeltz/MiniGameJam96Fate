namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("HairyNerd/MGJ96/ProgressBar")]
    public class ProgressBarBehaviour : BehaviourBase
    {
        #region Members

        public Image Background;
        public Image Foreground;

        protected DungeonBehaviour DungeonBehaviour { get; set; }

        #endregion

        #region Public Methods

        public void SetValues(float current, float max)
        {
            var ratio = current / max;
            Foreground.fillAmount = Mathf.Min(1, ratio);
        }

        #endregion
    }
}
