namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("HairyNerd/MGJ96/HeroUpgradeLevelText")]
    public class HeroUpgradeLevelTextBehaviour : BehaviourBase
    {
        #region Members

        public HeroUpgradeType UpgradeType;

        protected Text Text { get; set; }

        #endregion

        #region Protected Methods

        protected void SetLevelText()
        {
            int level = GameState.Hero.GetStatLevel(UpgradeType) + 1;
            Text.text = $"{level}";
        }

        #endregion

        #region Event Handlers

        private void Hero_LevelUpgraded(object sender, System.EventArgs e)
        {
            SetLevelText();
        }

        #endregion

        #region Unity

        protected void OnDestroy()
        {
            GameState.Hero.LevelUpgraded -= Hero_LevelUpgraded;
        }

        protected override void Awake()
        {
            Text = GetComponent<Text>();

            GameState.Hero.LevelUpgraded += Hero_LevelUpgraded;

            SetLevelText();
        }        

        #endregion
    }
}