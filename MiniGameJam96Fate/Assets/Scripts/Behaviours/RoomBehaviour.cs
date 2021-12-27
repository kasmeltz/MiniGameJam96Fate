namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("HairyNerd/MGJ96/Room")]
    public class RoomBehaviour : BehaviourBase
    {
        #region Constructors

        public RoomBehaviour()
        {
            Neighbours = new List<RoomBehaviour>();
        }

        #endregion

        #region Members

        public int TileHeight;
        public int TileWidth;

        public List<RoomConnectionBehaviour> Connections;

        public List<RoomBehaviour> Neighbours { get; set; }

        #endregion

        #region

        public bool IsFilled
        {
            get
            {
                return Neighbours.Count >= Connections.Count;
            }
        }

        #endregion
    }
}