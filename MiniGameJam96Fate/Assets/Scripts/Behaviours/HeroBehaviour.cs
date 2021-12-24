namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEngine.UI;

    [AddComponentMenu("HairyNerd/MGJ96/Hero")]
    public class HeroBehaviour : ActorBehaviour
    {
        #region Members

        public Image KeyImage;

        public RectTransform DiePanel;
        
        public RectTransform WinPanel;


        public float WallSmashRecovery = 0.75f;

        public float WallSmashCountRecovery = 0.33f;

        public float MovementStep = 0.32f;

        public int StartingWalkBreaks = 5;

        public int MaximumWallBreaks = 10;

        public ProgressBarBehaviour WallSmashBar;

        public Text WallBreaksAvailableText;

        public bool HasWon { get; set; }

        protected float WallSmashPower { get; set; }

        protected float WallSmashCountTimer { get; set; }
        
        protected DungeonBehaviour DungeonBehaviour { get; set; }

        protected ReaperBehaviour Reaper { get; set; }

        protected int WallBreaksAvailable { get; set; }

        protected bool IsDead { get; set; }        

        protected bool HasKey { get; set; }

        protected Vector3 CameraVelocity { get; set; }

        #endregion

        #region Public Methods

        public void ObtainKey(bool obtainKey)
        {
            HasKey = obtainKey;

            KeyImage
                .gameObject
                .SetActive(obtainKey);
        }

        #endregion

        #region Protected Methods

        protected void UpdateWallBreakText()
        {
            WallBreaksAvailableText.text = $"{WallBreaksAvailable}";
        }

        protected void TryBreakWall()
        {
            if (WallSmashPower < 1)
            {
                return;
            }

            if (WallBreaksAvailable <= 0)
            {
                return;
            }

            Vector3Int cellPosition = Vector3Int.zero;
            TileBase tile = null;

            int dx = 0;
            int dy = 0;

            if (Direction.x < 0)
            {
                dx = -1;
            }
            else if (Direction.x > 0)
            {
                dx = 1;
            }

            if (Direction.y < 0)
            {
                dy = -1;
            }
            else if (Direction.y > 0)
            {
                dy = 1;
            }

            var newPos = transform.position + new Vector3(dx * MovementStep, dy * MovementStep, 0);

            cellPosition = DungeonBehaviour
                .Walls
                .WorldToCell(newPos);

            var cellBounds = DungeonBehaviour.Walls.cellBounds;     
            
            if (cellPosition.x == cellBounds.min.x || 
                cellPosition.x == cellBounds.max.x || 
                cellPosition.y == cellBounds.min.y || 
                cellPosition.y == cellBounds.max.y)
            {
                // CAN'T BREAK OUTER MOST WALL!
                return;
            }

            WallBreaksAvailable--;
            UpdateWallBreakText();

            WallSmashPower = 0;

            tile = DungeonBehaviour
                .Walls
                .GetTile(cellPosition);

            if (tile != null)
            {
                DungeonBehaviour
                    .Walls
                    .SetTile(cellPosition, null);
            }
        }

        protected void TryMove(int x, int y)
        {
            bool canMove = false;
            Vector3Int cellPosition = Vector3Int.zero;
            TileBase tile = null;

            var newPos = transform.position + new Vector3(x * MovementStep, y * MovementStep, 0);

            cellPosition = DungeonBehaviour
                .Walls
                .WorldToCell(newPos);

            tile = DungeonBehaviour
                .Walls
                .GetTile(cellPosition);

            if (tile == null)
            {
                canMove = true;
            }

            if (canMove)
            {
                transform.position = newPos;
            }            
        }

        protected void Reset()
        {
            CameraVelocity = Vector3.zero;
            transform.position = new Vector3(0, 0, 0);
            SetDirection(0, -1);
            ObtainKey(false);
            IsDead = false;
            WallBreaksAvailable = StartingWalkBreaks;
            UpdateWallBreakText();
            WallSmashPower = 0;
            WallSmashCountTimer = 0;
        }

        #endregion

        #region Unity

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (HasWon)
            {
                return;
            }

            var orb = collision
                .GetComponent<OrbBehaviour>();

            if (orb != null)
            {
                Reaper
                    .Freeze();

                MegaDestroy(orb.gameObject);
            }

            var reaper = collision
                .GetComponent<ReaperBehaviour>();

            if (reaper != null)
            {
                IsDead = true;

                DiePanel
                    .gameObject
                    .SetActive(true);
            }

            var key = collision
                .GetComponent<KeyBehaviour>();

            if (key != null)
            {
                ObtainKey(true);

                MegaDestroy(key.gameObject);
            }

            var door = collision
                .GetComponent<DoorBehaviour>();

            if (door != null)
            {
                if (HasKey)
                {
                    HasWon = true;
                    WinPanel
                        .gameObject
                        .SetActive(true);
                }
            }
        }

        protected void Update()
        {
            var cameraPos = Camera.main.transform.position;
            cameraPos = new Vector3(transform.position.x, transform.position.y, cameraPos.z);
            var currentVelocity = CameraVelocity;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, cameraPos, ref currentVelocity, 0.1f);
            CameraVelocity = currentVelocity;

            if (HasWon)
            {
                return;
            }

            if (IsDead)
            {
                return;
            }

            if (WallSmashPower < 1)
            {
                WallSmashPower += Time.deltaTime * WallSmashRecovery;

                if (WallSmashPower >= 1)
                {
                    WallSmashPower = 1;
                }
            }

            if (WallBreaksAvailable < MaximumWallBreaks)
            {
                if (WallSmashCountTimer < 1)
                {
                    WallSmashCountTimer += Time.deltaTime * WallSmashCountRecovery;

                    if (WallSmashCountTimer >= 1)
                    {
                        WallSmashCountTimer = 0;
                        WallBreaksAvailable++;

                        UpdateWallBreakText();
                    }
                }
            }

            WallSmashBar
                .SetValues(WallSmashPower, 1);

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SetDirection(0, -1);
                TryMove(0, -1);                
            }
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SetDirection(0, 1);
                TryMove(0, 1);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetDirection(-1 ,0);
                TryMove(-1, 0);
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetDirection(1, 0);
                TryMove(1, 0);
            }
            
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                TryBreakWall();
            }
        }

        protected override void Awake()
        {
            base
                .Awake();

            DungeonBehaviour = FindObjectOfType<DungeonBehaviour>();
            Reaper = FindObjectOfType<ReaperBehaviour>();

            Reset();
        }

        #endregion
    }
}
