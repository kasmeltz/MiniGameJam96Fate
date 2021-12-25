using System;
using System.Collections.Generic;

namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic
{
    public class Hero
    {
        #region Constructors

        public Hero()
        {
            Levels = new Dictionary<HeroUpgradeType, int>();
            Levels[HeroUpgradeType.MovementSpeed] = 0;
            Levels[HeroUpgradeType.WallSmashCount] = 0;
            Levels[HeroUpgradeType.WallSmashRecovery] = 0;
        }

        #endregion

        #region Members

        public int Coins { get; set; }

        public Dictionary<HeroUpgradeType, int> Levels { get; set; }

        protected Dictionary<HeroUpgradeType, float[]> LevelStats = new Dictionary<HeroUpgradeType, float[]>
        {
            [HeroUpgradeType.MovementSpeed] = new float[] { 0.02f, 0.04f, 0.06f, 0.08f, 0.10f, 0.12f, 0.14f, 0.16f },
            [HeroUpgradeType.WallSmashRecovery] = new float[] { 0.1f, 0.25f, 0.33f, 0.5f, 0.75f, 1f, 1.25f, 1.5f },
            [HeroUpgradeType.WallSmashCount] = new float[] { 2, 4, 6, 8, 10, 12, 15, 20, 25, 30, 35, 40, 45, 50 }
        };

        protected Dictionary<HeroUpgradeType, int[]> LevelCosts = new Dictionary<HeroUpgradeType, int[]>
        {
            [HeroUpgradeType.MovementSpeed] = new int[] { 0, 20, 50, 90, 180, 250, 330, 420 },
            [HeroUpgradeType.WallSmashRecovery] = new int[] { 0, 20, 50, 120, 180, 250, 330, 420 },
            [HeroUpgradeType.WallSmashCount] = new int[] { 0, 20, 20, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260 }
        };

        #endregion

        #region Events

        public event EventHandler CoinsChanged;
        protected void OnCoinsChanged()
        {
            CoinsChanged?
                .Invoke(this, EventArgs.Empty);
        }

        public event EventHandler LevelUpgraded;
        protected void OnLevelUpgraded()
        {
            LevelUpgraded?
                .Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Public Methods

        public void ChangeCoins(int amount)
        {
            Coins += amount;
            OnCoinsChanged();
        }

        public bool IsMaximumLevel(HeroUpgradeType upgradeType, int? level = null)
        {
            if (!level.HasValue)
            {
                level = GetStatLevel(upgradeType) + 1;
            }

            var costs = LevelCosts[upgradeType];
            if (level.Value >= costs.Length)
            {
                return true;
            }

            return false;
        }

        public int GetUpgradeCost(HeroUpgradeType upgradeType, int? level = null)
        {
            if (!level.HasValue)
            {
                level = GetStatLevel(upgradeType) + 1;
            }

            var costs = LevelCosts[upgradeType];

            if (level.Value >= costs.Length)
            {
                return int.MaxValue;
            }

            return costs[level.Value];
        }

        public bool CanUpgradeLevel(HeroUpgradeType upgradeType)
        {
            int currentLevel = Levels[upgradeType];

            if (IsMaximumLevel(upgradeType, currentLevel))
            {
                return false;
            }

            var cost = GetUpgradeCost(upgradeType, currentLevel + 1);
            if (cost > Coins)
            {
                return false;
            }

            return true;
        }

        public bool UpgradeLevel(HeroUpgradeType upgradeType)
        {
            int currentLevel = Levels[upgradeType];

            if (!CanUpgradeLevel(upgradeType))
            {
                return false;
            }

            var cost = GetUpgradeCost(upgradeType, currentLevel + 1);
            ChangeCoins(-cost);

            Levels[upgradeType]++;

            OnLevelUpgraded();
            return true;
        }

        public int GetStatLevel(HeroUpgradeType upgradeType)
        {
            return Levels[upgradeType];
        }

        public float GetStatValue(HeroUpgradeType upgradeType, int? level = null)
        {
            if (!level.HasValue)
            {
                level = Levels[upgradeType];
            }

            return LevelStats[upgradeType][level.Value];
        }

        #endregion
    }
}
