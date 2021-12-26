namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using System;
    using System.Collections;
    using UnityEngine;

    public abstract class BehaviourBase : MonoBehaviour
    {
        #region Protected Methods

        protected void MegaDestroy(GameObject gameObject)
        {
            Destroy(gameObject);
        }

        #endregion

        #region Timers

        public Coroutine ExecuteAfterTime(float seconds, Action action)
        {
            var coroutine = StartCoroutine(ExecuteAfterTimeCoroutine(seconds, action));

            return coroutine;
        }

        protected IEnumerator ExecuteAfterTimeCoroutine(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);

            action();
        }

        #endregion

        #region Unity

        protected virtual void Awake()
        {

        }

        #endregion
    }
}