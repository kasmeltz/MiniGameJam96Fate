namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("HairyNerd/MGJ96/RoomConnection")]
    public class RoomConnectionBehaviour : BehaviourBase
    {
        public RoomConnectionDirection Direction;

        public bool IsFilled { get; set; }        
    }
}