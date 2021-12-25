using System.Collections.Generic;

namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic
{
    public static class GameState
    {
        public static int Level { get; set; }

        public static List<GameStage> Stages = new List<GameStage>
        {
            new GameStage
            {
                Width = 70,
                Height = 35,
                RoomThreshold = 0.85f,
                RoomOverlapAmount = -1,
                MinRoomWidth = 3,
                MaxRoomWidth = 7,
                MinRoomHeight = 3,
                MaxRoomHeight= 7,
                FlareOrbCount = 5,
                FreezeOrbCount = 5,
                CoinCount = 10,
                ReaperAppearTime = 10,
                ReaperFreezeTime = 5,
                ReaperMovementStep = 0.04f,
                ReaperMovementCooldown = 5f
            },
            new GameStage
            {
                Width = 80,
                Height = 50,
                RoomThreshold = 0.8f,
                RoomOverlapAmount = -1,
                MinRoomWidth = 3,
                MaxRoomWidth = 7,
                MinRoomHeight = 3,
                MaxRoomHeight= 7,
                FlareOrbCount = 5,
                FreezeOrbCount = 5,
                CoinCount = 15,
                ReaperAppearTime = 10,
                ReaperFreezeTime = 4.5f,
                ReaperMovementStep = 0.04f,
                ReaperMovementCooldown = 7.5f
            },
            new GameStage
            {
                Width = 100,
                Height = 60,
                RoomThreshold = 0.75f,
                RoomOverlapAmount = -1,
                MinRoomWidth = 3,
                MaxRoomWidth = 7,
                MinRoomHeight = 3,
                MaxRoomHeight= 7,
                FlareOrbCount = 5,
                FreezeOrbCount = 5,
                CoinCount = 20,
                ReaperAppearTime = 10,
                ReaperFreezeTime = 4f,
                ReaperMovementStep = 0.04f,
                ReaperMovementCooldown = 10f
            },
            new GameStage
            {
                Width = 120,
                Height = 70,
                RoomThreshold = 0.7f,
                RoomOverlapAmount = -1,
                MinRoomWidth = 3,
                MaxRoomWidth = 7,
                MinRoomHeight = 3,
                MaxRoomHeight= 7,
                FlareOrbCount = 5,
                FreezeOrbCount = 5,
                CoinCount = 25,
                ReaperAppearTime = 10,
                ReaperFreezeTime = 3.5f,
                ReaperMovementStep = 0.06f,
                ReaperMovementCooldown = 10f
            },
            new GameStage
            {
                Width = 140,
                Height = 80,
                RoomThreshold = 0.65f,
                RoomOverlapAmount = -1,
                MinRoomWidth = 3,
                MaxRoomWidth = 7,
                MinRoomHeight = 3,
                MaxRoomHeight= 7,
                FlareOrbCount = 5,
                FreezeOrbCount = 5,
                CoinCount = 30,
                ReaperAppearTime = 10,
                ReaperFreezeTime = 3f,
                ReaperMovementStep = 0.06f,
                ReaperMovementCooldown = 12.5f
            },
            new GameStage
            {
                Width = 160,
                Height = 90,
                RoomThreshold = 0.6f,
                RoomOverlapAmount = -1,
                MinRoomWidth = 3,
                MaxRoomWidth = 7,
                MinRoomHeight = 3,
                MaxRoomHeight= 7,
                FlareOrbCount = 5,
                FreezeOrbCount = 5,
                CoinCount = 35,
                ReaperAppearTime = 10,
                ReaperFreezeTime = 2.5f,
                ReaperMovementStep = 0.06f,
                ReaperMovementCooldown = 15f
            },
            new GameStage
            {
                Width = 180,
                Height = 100,
                RoomThreshold = 0.6f,
                RoomOverlapAmount = -1,
                MinRoomWidth = 3,
                MaxRoomWidth = 7,
                MinRoomHeight = 3,
                MaxRoomHeight= 7,
                FlareOrbCount = 5,
                FreezeOrbCount = 5,
                CoinCount = 40,
                ReaperAppearTime = 10,
                ReaperFreezeTime = 2f,
                ReaperMovementStep = 0.08f,
                ReaperMovementCooldown = 17.5f
            }
        };

        public static GameStage CurrentStage
        {
            get
            {
                if (Level < 0 || Level >= Stages.Count)
                {
                    return null;
                }

                return Stages[Level];
            }
        }

        private static Hero _hero;

        public static Hero Hero
        {
            get
            {
                if (_hero == null)
                {
                    _hero = new Hero();
                }

                return _hero;
            }
        }
    }
}