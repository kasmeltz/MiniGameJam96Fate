namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("HairyNerd/MGJ96/GameStageText")]
    public class GameStageTextBehaviour : BehaviourBase
    {
        #region Members

        protected Text Text { get; set; }

        #endregion

        #region Protected Methods

        protected void SetStageText()
        {
            Text.text = $"Stage {GameState.Level + 1}";
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            Text = GetComponent<Text>();

            SetStageText();
        }

        #endregion
    }
}