namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("HairyNerd/MGJ96/Room")]
    public class RoomBehaviour : BehaviourBase
    {
        #region Contructors

        public RoomBehaviour()
        {
            TopNeighbours = new List<RoomBehaviour>();
            BottomNeighbours = new List<RoomBehaviour>();
            LeftNeighbours = new List<RoomBehaviour>();
            RightNeighbours = new List<RoomBehaviour>();
        }

        #endregion

        public int TileWidth;
        
        public int TileHeight;

        public int TopEntrances;

        public int LeftEntrances;

        public int RightEntrances;

        public int BottomEntrances;

        public List<RoomBehaviour> TopNeighbours { get; set; }

        public List<RoomBehaviour> BottomNeighbours { get; set; }

        public List<RoomBehaviour> LeftNeighbours { get; set; }

        public List<RoomBehaviour> RightNeighbours { get; set; }

        public bool IsTopFilled
        {
            get
            {
                return TopNeighbours.Count >= TopEntrances;
            }
        }

        public bool IsBottomFilled
        {
            get
            {
                return BottomNeighbours.Count >= BottomEntrances;
            }
        }

        public bool IsLeftFilled
        {
            get
            {
                return LeftNeighbours.Count >= LeftEntrances;
            }
        }

        public bool IsRightFilled
        {
            get
            {
                return RightNeighbours.Count >= RightEntrances;
            }
        }
    }
}