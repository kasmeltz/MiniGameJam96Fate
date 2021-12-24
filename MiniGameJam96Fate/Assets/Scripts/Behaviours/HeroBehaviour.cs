namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.MathExtensions;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEngine.UI;

    [AddComponentMenu("HairyNerd/MGJ96/Hero")]
    public class HeroBehaviour : ActorBehaviour
    {
        #region Members

        public AudioClip[] FootStepAudioClips;

        public Image KeyImage;

        public RectTransform DiePanel;

        public RectTransform WinPanel;


        public float WallSmashRecovery = 0.75f;

        public float WallSmashCountRecovery = 0.33f;

        public float MovementStep = 0.32f;

        public int StartingWalkBreaks = 5;

        public int MaximumWallBreaks = 10;

        public ProgressBarBehaviour WallSmashBar;
        public ReaperProgressBarBehaviour ReaperMode;

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

        protected Rigidbody2D Rigidbody2D { get; set; }

        protected AudioSource AudioSource { get; set; }


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

            Vector3 cellSize = DungeonBehaviour.Walls.layoutGrid.cellSize;

            var newPos = transform.position +
                new Vector3(dx * cellSize.x, dy * cellSize.y, 0);

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

        protected bool CheckIfCanMove(int x, int y, Vector3 position, Vector3 quantizedPosition)
        {
            var isAtXBorder = quantizedPosition
                .x
                .CloseTo(position.x, 0.01f);

            if (x != 0 && !isAtXBorder)
            {
                return true;
            }

            var isAtYBorder = quantizedPosition
                .y
                .CloseTo(position.y, 0.01f);

            if (y != 0 && !isAtYBorder)
            {
                return true;
            }

            var cellPosition = DungeonBehaviour
                .Walls
                .WorldToCell(quantizedPosition);

            if (x > 0)
            {
                cellPosition.x++;
            }
            else if (x < 0)
            {
                cellPosition.x--;
            }

            if (y > 0)
            {
                cellPosition.y++;
            }
            else if (y < 0)
            {
                cellPosition.y--;
            }

            if (quantizedPosition.x.CloseTo(position.x, 0.01f) &&
                quantizedPosition.y.CloseTo(position.y, 0.01f))
            {
                var tile = DungeonBehaviour
                    .Walls
                    .GetTile(cellPosition);

                return tile == null;
            }

            if (x != 0)
            {
                var sy = cellPosition.y;
                var ey = cellPosition.y;

                if (position.y > quantizedPosition.y)
                {
                    ey++;
                }
                else if (position.y < quantizedPosition.y)
                {
                    sy--;
                }

                for(int cy = sy;cy <= ey;cy++)
                {
                    cellPosition = new Vector3Int(cellPosition.x, cy, 0);

                    var tile = DungeonBehaviour
                        .Walls
                        .GetTile(cellPosition);

                    if (tile != null)
                    {
                        return false;
                    }
                }
            }

            if (y != 0)
            {
                var sx = cellPosition.x;
                var ex = cellPosition.x;

                if (position.x > quantizedPosition.x)
                {
                    ex++;
                }
                else if (position.x < quantizedPosition.x)
                {
                    sx--;
                }

                for (int cx = sx; cx <= ex; cx++)
                {
                    cellPosition = new Vector3Int(cx, cellPosition.y, 0);

                    var tile = DungeonBehaviour
                        .Walls
                        .GetTile(cellPosition);

                    if (tile != null)
                    {
                        return false;
                    }
                }
            }

            return true;

        }

        protected void TryMove(int x, int y)
        {
            var position = transform.position;
            Vector3 cellSize = DungeonBehaviour.Walls.layoutGrid.cellSize;

            var roundedPosX = position.x.Quantize(cellSize.x);
            var roundedPosY = position.y.Quantize(cellSize.y);
            var quantizedPosition = new Vector3(roundedPosX, roundedPosY, 0);

            if(CheckIfCanMove(x, y, position, quantizedPosition))
            {
                if (Random.value >= 0.5f)
                {
                    int index = Random
                        .Range(0, FootStepAudioClips.Length);

                    AudioSource
                        .PlayOneShot(FootStepAudioClips[index], 0.5f);
                }

                var movementVector = new Vector3(x * MovementStep, y * MovementStep, 0);
                position = transform.position + movementVector;
                position.x = position.x.Quantize(MovementStep);
                position.y = position.y.Quantize(MovementStep);
                transform.position = position;
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
                if (orb.IsFlareOrb)
                {
                    ReaperMode.Reset();

                    MegaDestroy(orb.gameObject);
                }
                else if (Reaper.gameObject.activeInHierarchy)
                {
                    Reaper
                    .Freeze();

                    MegaDestroy(orb.gameObject);
                }

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
                SetDirection(-1, 0);
                TryMove(-1, 0);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetDirection(1, 0);
                TryMove(1, 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                TryBreakWall();
            }
        }

        protected override void Awake()
        {
            base
                .Awake();

            Rigidbody2D = FindObjectOfType<Rigidbody2D>();
            DungeonBehaviour = FindObjectOfType<DungeonBehaviour>();
            Reaper = FindObjectOfType<ReaperBehaviour>();
            AudioSource = GetComponent<AudioSource>();

            Reset();
        }

        #endregion
    }
}