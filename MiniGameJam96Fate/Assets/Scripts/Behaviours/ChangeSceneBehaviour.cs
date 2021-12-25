namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [AddComponentMenu("HairyNerd/MGJ96/ChangeScene")]
    public class ChangeSceneBehaviour : BehaviourBase
    {
        #region Public Methods

        public void ChangeScene(string sceneName)
        {
            SceneManager
                .LoadSceneAsync(sceneName);
        }

       #endregion
    }
}