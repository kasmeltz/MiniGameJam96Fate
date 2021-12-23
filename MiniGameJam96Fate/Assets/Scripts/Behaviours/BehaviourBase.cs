namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    public abstract class BehaviourBase : MonoBehaviour
    {
        #region Protected Methods

        protected void MegaDestroy(GameObject gameObject)
        {
            Destroy(gameObject);
        }

        #endregion

        #region Unity

        protected virtual void Awake()
        {

        }

        #endregion
    }
}