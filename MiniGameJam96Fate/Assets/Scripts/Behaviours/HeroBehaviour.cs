namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("HairyNerd/MGJ96/Hero")]
    public class HeroBehaviour : ActorBehaviour
    {
        public float MovementStep = 0.32f;

        #region Unity

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SetDirection(0, -1);
                transform.Translate(new Vector3(0, -MovementStep, 0));
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SetDirection(0, 1);
                transform.Translate(new Vector3(0, MovementStep, 0));
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetDirection(-1 ,0);
                transform.Translate(new Vector3(-MovementStep, 0, 0));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.Translate(new Vector3(MovementStep, 0, 0));
                SetDirection(1, 0);
            }
        }

        #endregion
    }
}