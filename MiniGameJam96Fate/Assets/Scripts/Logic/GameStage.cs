namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic
{
    public class GameStage
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public double RoomThreshold { get; set; }

        public int RoomOverlapAmount { get; set; }

        public int MinRoomWidth { get; set; }

        public int MaxRoomWidth { get; set; }

        public int MinRoomHeight { get; set; }

        public int MaxRoomHeight { get; set; }

        public int FlareOrbCount { get; set; }
        
        public int FreezeOrbCount { get; set; }
        
        public int CoinCount { get; set; }

        public float ReaperAppearTime { get; set; }

        public float ReaperFreezeTime { get; set; }

        public float ReaperMovementStep { get; set; }

        public float ReaperMovementCooldown { get; set; }
    }
}