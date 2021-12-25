namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("HairyNerd/MGJ96/HeroUpgradeButton")]
    public class HeroUpgradeButtonBehaviour : BehaviourBase
    {
        #region Members

        public HeroUpgradeType UpgradeType;

        public Text CostText;

        #endregion

        #region Public Methods

        public void DoUpgrade()
        {
            var hero = GameState.Hero;
            if (!hero.CanUpgradeLevel(UpgradeType))
            {
                return;
            }

            hero
                .UpgradeLevel(UpgradeType);
        }

        #endregion

        #region Event Handlers

        private void Hero_LevelUpgraded(object sender, System.EventArgs e)
        {
            SetState();
        }


        #endregion
        #region Protected Methods

        protected void SetState()
        {
            var hero = GameState.Hero;

            var button = GetComponent<Button>();            
            if (hero.CanUpgradeLevel(UpgradeType))
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }

            SetCostText();
        }

        protected void SetCostText()
        {
            var hero = GameState.Hero;

            if (hero.IsMaximumLevel(UpgradeType))
            {
                CostText.text = "";
            }
            else
            {
                var cost = hero.GetUpgradeCost(UpgradeType);
                CostText.text = $"{cost}";
            }
        }

        #endregion

        #region Unity

        protected void OnDestroy()
        {
            GameState.Hero.LevelUpgraded -= Hero_LevelUpgraded;
        }

        protected override void Awake()
        {
            GameState.Hero.LevelUpgraded += Hero_LevelUpgraded;

            SetState();            
        }

        #endregion
    }
}