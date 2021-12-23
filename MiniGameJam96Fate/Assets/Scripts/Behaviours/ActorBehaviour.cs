namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("HairyNerd/MGJ96/Actor")]
    public class ActorBehaviour : BehaviourBase
    {
        #region Members

        protected Animator Animator { get; set; }

        protected Vector2 Direction { get; set; }


        #endregion

        #region Public Methods

        public void SetDirection(float dx, float dy)
        {
            Direction = new Vector2(dx, dy);
             
            Animator
                .SetFloat("DirectionX", dx);

            Animator
                .SetFloat("DirectionY", dy);
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Animator = GetComponent<Animator>();
                
            SetDirection(0, -1);
        }

        #endregion
    }
}