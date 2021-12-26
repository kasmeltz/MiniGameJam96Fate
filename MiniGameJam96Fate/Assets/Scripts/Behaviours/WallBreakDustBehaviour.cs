namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("HairyNerd/MGJ96/WallBreakDustBehaviour")]
    public class WallBreakDustBehaviour : ActorBehaviour
    {
        public void AnimationFinished()
        {
            MegaDestroy(this.gameObject);
        }
    }
}
