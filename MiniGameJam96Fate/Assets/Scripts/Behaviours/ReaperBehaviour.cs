namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("HairyNerd/MGJ96/Reaper")]
    public class ReaperBehaviour : ActorBehaviour
    {
        #region Members

        public Image ProximityOverlay;

        public float HeroProximityRedDistance = 2;

        public float MovementStep { get; set; }

        public float MovementCooldownRate { get; set; }

        public float FrozenSeconds { get; set; }

        protected float MovementTimer { get; set; }

        protected float FrozenTimer { get; set; }

        protected HeroBehaviour Hero { get; set; }

        protected DungeonBehaviour DungeonBehaviour { get; set; }

        #endregion

        #region Public Methods

        public void Freeze()
        {
            FrozenTimer = FrozenSeconds;
        }

        public void Reset()
        {
            var proximityColor = ProximityOverlay.color;
            proximityColor.a = 0;
            ProximityOverlay.color = proximityColor;

            MovementStep = GameState.CurrentStage.ReaperMovementStep;
            MovementCooldownRate = GameState.CurrentStage.ReaperMovementCooldown;
            FrozenSeconds = GameState.CurrentStage.ReaperFreezeTime;

            MovementTimer = 0;
            FrozenTimer = 0;

            var dungeon = DungeonBehaviour.Dungeon;

            var cellSize = DungeonBehaviour.Walls.layoutGrid.cellSize;

            int hw = dungeon.Width / 2;
            int hh = dungeon.Height / 2;

            System.Random rnd = new System.Random();
            int x = rnd.Next(hw - 8, hw - 2);
            int y = rnd.Next(hh - 8, hh - 2);

            int dx = rnd.Next(0, 2);
            if (dx == 1)
            {
                x *= -1;
            }

            int dy = rnd.Next(0, 2);
            if (dy == 1)
            {
                y *= -1;
            }

            transform.position = new Vector3(x * cellSize.x, y * cellSize.y, 0);
        }

        #endregion

        #region Protected Methods        

        protected void Move()
        {
            int x = 0;
            int y = 0;

            float dx = Hero.transform.position.x - transform.position.x;
            float dy = Hero.transform.position.y - transform.position.y;


            if (dx > 0.08f)
            {
                x = 1;
            }
            else if (dx < -0.08f)
            {
                x = -1;
            }
            
            if (dy > 0.08f)
            {
                y = 1;
            }
            else if (dy < -0.08f)
            {
                y = -1;
            }

            SetDirection(x, y);

            var newPos = transform.position + new Vector3(x * MovementStep, y * MovementStep, 0);
            transform.position = newPos;
        }

        #endregion

        #region Unity        

        protected void Update()
        {
            if (!gameObject.activeInHierarchy)
            {
                Reset();
            }

            if (Hero.HasWon)
            {
                ProximityOverlay
                    .gameObject
                    .SetActive(false);

                return;
            }

            var proximityColor = ProximityOverlay.color;
            if (proximityColor.a <= 0.25f)
            {
                proximityColor.a += Time.deltaTime * 0.25f;

                if (proximityColor.a > 0.25f)
                {
                    proximityColor.a = 0;
                }
            }
            ProximityOverlay.color = proximityColor;

            var distanceToHero = (transform.position - Hero.transform.position).magnitude;
            if (distanceToHero <= HeroProximityRedDistance)
            {
                ProximityOverlay
                    .gameObject
                    .SetActive(true);
            }
            else
            {
                ProximityOverlay
                    .gameObject
                    .SetActive(false);
            }

            if (FrozenTimer > 0)
            {
                FrozenTimer -= Time.deltaTime;
                if (FrozenTimer < 0)
                {
                    FrozenTimer = 0;
                }
            }

            if (FrozenTimer > 0)
            {
                return;
            }

            if (MovementTimer < 1)
            {
                MovementTimer += Time.deltaTime * MovementCooldownRate;

                if (MovementTimer >= 1)
                {
                    Move();
                    MovementTimer = 0;
                }
            }

        }

        protected override void Awake()
        {
            base
                .Awake();

            Hero = FindObjectOfType<HeroBehaviour>();
            DungeonBehaviour = FindObjectOfType<DungeonBehaviour>();

            Reset();
        }

        #endregion
    }
}
