namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("HairyNerd/MGJ96/HeroCoinText")]
    public class HeroCoinTextBehaviour : BehaviourBase
    {
        #region Members

        protected Text Text { get; set; }

        #endregion

        #region Protected Methods

        protected void SetCoinText()
        {
            Text.text = $"{GameState.Hero.Coins}";
        }

        #endregion

        #region Event Handlers

        private void Hero_CoinsChanged(object sender, System.EventArgs e)
        {
            SetCoinText();
        }

        #endregion

        #region Unity

        protected void OnDestroy()
        {
            GameState.Hero.CoinsChanged -= Hero_CoinsChanged;
        }

        protected override void Awake()
        {
            Text = GetComponent<Text>();

            GameState.Hero.CoinsChanged += Hero_CoinsChanged;


            SetCoinText();
        }

        #endregion
    }
}