namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("HairyNerd/MGJ96/Room")]
    public class RoomBehaviour : BehaviourBase
    {
        public int TileWidth;
        
        public int TileHeight;

        public int TopEntrances;

        public int LeftEntrances;

        public int RightEntrances;

        public int BottomEntrances;

        public RoomBehaviour Up { get; set; }

        public RoomBehaviour Down { get; set; }

        public RoomBehaviour Left { get; set; }

        public RoomBehaviour Right { get; set; }
    }
}