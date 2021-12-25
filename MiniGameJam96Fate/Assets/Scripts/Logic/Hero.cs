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
        }

        #endregion

        #region Members

        public int Coins { get; set; }

        public Dictionary<HeroUpgradeType, int> Levels { get; set; }

        protected Dictionary<HeroUpgradeType, float[]> LevelStats = new Dictionary<HeroUpgradeType, float[]>
        {
            [HeroUpgradeType.MovementSpeed] = new float[] { 0.02f, 0.04f, 0.06f, 0.08f, 0.10f, 0.12f, 0.14f, 0.16f }
        };

        protected Dictionary<HeroUpgradeType, int[]> LevelCosts = new Dictionary<HeroUpgradeType, int[]>
        {
            [HeroUpgradeType.MovementSpeed] = new int[] { 0, 100, 200, 300, 400, 500, 600, 700 }
        };

        #endregion

        #region Public Methods

        public bool IsMaximumLevel(HeroUpgradeType upgradeType, int level)
        {
            var costs = LevelCosts[upgradeType];
            if (level >= costs.Length - 1)
            {
                return true;
            }

            return false;
        }

        public int GetUpgradeCost(HeroUpgradeType upgradeType, int level)
        {
            if (IsMaximumLevel(upgradeType, level))
            {
                return int.MaxValue;
            }

            var costs = LevelCosts[upgradeType];            
            return costs[level];
        }

        public bool CanUpgradeLevel(HeroUpgradeType upgradeType)
        {
            int currentLevel = Levels[upgradeType];

            if (IsMaximumLevel(upgradeType, currentLevel))
            {
                return false;
            }

            var cost = GetUpgradeCost(upgradeType, currentLevel);
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

            Levels[upgradeType]++;

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
