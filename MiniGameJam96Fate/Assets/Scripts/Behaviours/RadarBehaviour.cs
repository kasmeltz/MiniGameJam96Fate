namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    public class RadarBehaviour : ActorBehaviour
    {
        #region Members

        protected ReaperBehaviour Reaper { get; set; }

        [SerializeField] private Transform PlayerPivot = null;

        #endregion

        protected override void Awake()
        {
            base
                .Awake();

            Reaper = FindObjectOfType<ReaperBehaviour>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Reaper.gameObject.active)
            {
                gameObject.SetActive(true);
                transform.RotateAround(PlayerPivot.localPosition, Vector3.back, Time.deltaTime * 10f);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}