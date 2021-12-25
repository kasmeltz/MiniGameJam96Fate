namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using UnityEngine;

    [AddComponentMenu("HairyNerd/MGJ96/Coin")]
    public class CoinBehaviour : BehaviourBase
    {
        #region Members

        public int Value { get; set; }

        #endregion

        #region Unity 

        protected override void Awake()
        {
            base
                .Awake();

            Value = (int)(10 + Mathf.Round(GameState.Level * 2.5f));
        }
        
        #endregion
    }
}
