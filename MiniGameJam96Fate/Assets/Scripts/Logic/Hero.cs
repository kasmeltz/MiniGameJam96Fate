using System;
using System.Collections.Generic;

namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic
{
    public class Hero
    {
        #region Constructors

        public Hero()
        {
            Coins = 0;
            Levels = new Dictionary<HeroUpgradeType, int>();
            Levels[HeroUpgradeType.MovementSpeed] = 0;
            Levels[HeroUpgradeType.MovementDistance] = 0;
            Levels[HeroUpgradeType.WallSmashCount] = 0;
            Levels[HeroUpgradeType.WallSmashRecovery] = 0;
            Levels[HeroUpgradeType.WallSmashPower] = 0;
            Levels[HeroUpgradeType.SpeedBoostRecover] = 0;
        }

        #endregion

        #region Members

        public int Coins { get; set; }

        public Dictionary<HeroUpgradeType, int> Levels { get; set; }

        protected Dictionary<HeroUpgradeType, float[]> LevelStats = new Dictionary<HeroUpgradeType, float[]>
        {
            [HeroUpgradeType.MovementSpeed] = new float[] { 0.2f, 0.18f, 0.16f, 0.14f, 0.12f, 0.10f, 0.08f, 0.07f, 0.06f, 0.05f },
            [HeroUpgradeType.MovementDistance] = new float[] { 0.02f, 0.04f, 0.08f, 0.16f },
            [HeroUpgradeType.WallSmashRecovery] = new float[] { 0.2f, 0.25f, 0.4f, 0.6f, 0.8f, 1f, 1.25f, 1.75f, 2f, 2.5f },
            [HeroUpgradeType.WallSmashCount] = new float[] { 2, 4, 6, 8, 10, 12, 15, 20, 25, 30, 35, 40, 45, 50, 75, 100, 200, 300, 400, 500 },
            [HeroUpgradeType.WallSmashPower] = new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            [HeroUpgradeType.SpeedBoostRecover] = new float[] { 0.05f, 0.06f, 0.075f, 1f, 1.1f, 1.2f },
        };

        protected Dictionary<HeroUpgradeType, int[]> LevelCosts = new Dictionary<HeroUpgradeType, int[]>
        {
            [HeroUpgradeType.MovementSpeed] = new int[] { 0, 20, 50, 90, 180, 250, 330, 420, 750, 1500 },
            [HeroUpgradeType.MovementDistance] = new int[] { 0, 50, 250, 500 },
            [HeroUpgradeType.WallSmashRecovery] = new int[] { 0, 20, 50, 120, 180, 250, 330, 420, 500, 750 },
            [HeroUpgradeType.WallSmashCount] = new int[] { 0, 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 500, 1000, 1500, 2000, 2500, 5000 },
            [HeroUpgradeType.WallSmashPower] = new int[] { 0, 50, 125, 225, 350, 500, 700, 1000, 2000, 5000 },
            [HeroUpgradeType.SpeedBoostRecover] = new int[] { 0, 20, 50, 120, 180, 250 },
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
