namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("HairyNerd/MGJ96/ReaperRadar")]
    public class ReaperRadarBehaviour : BehaviourBase
    {
        #region Members

        protected Image RadarImage { get; set; }

        protected HeroBehaviour Hero { get; set; }

        protected ReaperBehaviour Reaper { get; set; }

        protected float SpinAngle { get; set; }

        #endregion


        #region Unity        

        protected void Update()
        {
            if (!Reaper.gameObject.activeInHierarchy)
            {
                SpinAngle += Time.deltaTime * 180;
                RadarImage.transform.rotation = Quaternion.Euler(0f, 0f, SpinAngle);
                return;
            }

            Vector3 diff = (Reaper.transform.position - Hero.transform.position).normalized;
            var atan2 = Mathf.Atan2(diff.y, diff.x);
            RadarImage.transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);                        
        }

        protected override void Awake()
        {
            base
                .Awake();

            RadarImage = GetComponent<Image>();
            Hero = FindObjectOfType<HeroBehaviour>();
            Reaper = FindObjectOfType<ReaperBehaviour>();
        }

        #endregion
    }
}
