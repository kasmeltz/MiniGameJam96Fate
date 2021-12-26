namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.MathExtensions;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.Tilemaps;
    using UnityEngine.UI;

    [AddComponentMenu("HairyNerd/MGJ96/Hero")]
    public class HeroBehaviour : ActorBehaviour
    {
        #region Members

        public AudioClip[] FootStepAudioClips;

        public Image KeyImage;

        public WallBreakDustBehaviour WallBreakDustPrefab;

        public ProgressBarBehaviour WallSmashBar;

        public ProgressBarBehaviour SpeedBoostBar;

        public ReaperProgressBarBehaviour ReaperMode;

        public Text WallBreaksAvailableText;
       
        public bool HasWon { get; set; }

        protected float WallSmashPower { get; set; }

        protected float SpeedBoostPower { get; set; }

        protected DungeonBehaviour DungeonBehaviour { get; set; }

        protected ReaperBehaviour Reaper { get; set; }

        protected DoorBehaviour Door { get; set; }

        protected SoundEffectPlayerBehaviour SoundEffectPlayer { get; set; }

        protected Hero Hero { get; set; }

        protected float MovementStep { get; set; }

        protected float MovementSpeed { get; set; }

        protected float WallSmashRecovery { get; set; }

        protected int WallBreaksAvailable { get; set; }

        protected int WallSmashSize { get; set; }

        protected float MoveTimer { get; set; }

        protected bool IsDead { get; set; }

        protected bool HasKey { get; set; }

        protected int OrbCount { get; set; }

        protected Vector3 CameraVelocity { get; set; }

        protected AudioSource AudioSource { get; set; }
        protected float SpeedBoostRecover { get; set; }

        protected float SpeedBoostTimer { get; set; }
    
        protected bool IsSpeedBoostActive
        {
            get
            {
                return SpeedBoostTimer > 0;
            }
        }

        #endregion

        #region Public Methods

        public void ObtainCoin(CoinBehaviour coin)
        {
            Hero
                .ChangeCoins(coin.Value);
        }

        public void ObtainKey(bool obtainKey)
        {
            HasKey = obtainKey;

            KeyImage
                .gameObject
                .SetActive(obtainKey);
        }

        public void ObtainOrb(OrbBehaviour orb)
        {
            Door
                .Orbs[orb.OrbIndex]
                .gameObject
                .SetActive(true);

            OrbCount = 0;

            foreach(var doorOrb in Door.Orbs)
            {
                if (doorOrb.activeInHierarchy)
                {
                    OrbCount++;
                }
            }

            if (OrbCount == 4)
            {
                var key = FindObjectOfType<KeyBehaviour>(true);
                key
                    .gameObject
                    .SetActive(true);
            }
        }

        #endregion

        #region Protected Methods

        protected void UpdateWallBreakText()
        {
            WallBreaksAvailableText.text = $"{WallBreaksAvailable}";
        }

        protected void TrySpeedBoost()
        {
            if (SpeedBoostPower < 1)
            {
                return;
            }
            
            SpeedBoostPower = 0;
            SpeedBoostTimer = 0.5f;
        }

        protected bool TryBreakIndividualWall(Vector3Int cellPosition)
        {
            var cellBounds = DungeonBehaviour.Walls.cellBounds;

            if (Mathf.Abs(cellPosition.x - cellBounds.min.x) < 1 ||
                Mathf.Abs(cellPosition.x - cellBounds.max.x) < 2 ||
                Mathf.Abs(cellPosition.y - cellBounds.min.y) < 1 ||
                Mathf.Abs(cellPosition.y - cellBounds.max.y) < 2)
            {
                // CAN'T BREAK OUTER MOST WALL!
                return false;
            }

            var tile = DungeonBehaviour
                .Walls
                .GetTile(cellPosition);

            if (tile != null)
            {
                DungeonBehaviour
                    .Walls
                    .SetTile(cellPosition, null);

                var dust = Instantiate(WallBreakDustPrefab);
                dust.transform.position = new Vector3(cellPosition.x * 0.16f, cellPosition.y * 0.16f, 0);

                //var wallCellX = cellPosition.x + DungeonBehaviour.Dungeon.Width / 2;
                //var wallCellY = cellPosition.y + DungeonBehaviour.Dungeon.Height / 2;

                //DungeonBehaviour.Dungeon.Walls[wallCellY, wallCellX] = 0;

                return true;
            }

            return false;
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
            bool wasAWallBroken = false;

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

            int sy = cellPosition.y;
            int sx = cellPosition.x;

            var cellBounds = DungeonBehaviour.Walls.cellBounds;

            int depth = (WallSmashSize / 3);
            depth = Mathf.Max(depth, 1);

            int width = (WallSmashSize / 2);

            for (int i = 0; i < depth; i++)
            {
                if (dx != 0)
                {
                    for (int y = sy; y <= sy + (1 * width); y++)
                    {
                        cellPosition.y = y;
                        if (TryBreakIndividualWall(cellPosition))
                        {
                            wasAWallBroken = true;
                        }
                        else
                        {
                            break;
                        }
                    }

                    for (int y = sy - 1; y >= sy - (1 * width); y--)
                    {
                        cellPosition.y = y;
                        if (TryBreakIndividualWall(cellPosition))
                        {
                            wasAWallBroken = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (dy != 0)
                {                    
                    for (int x = sx; x <= sx + (1 * width); x++)
                    {
                        cellPosition.x = x;
                        if (TryBreakIndividualWall(cellPosition))
                        {
                            wasAWallBroken = true;
                        }
                        else
                        {
                            break;
                        }                        
                    }

                    for (int x = sx - 1; x >= sx - (1 * width); x--)
                    {
                        cellPosition.x = x;

                        if (TryBreakIndividualWall(cellPosition))
                        {
                            wasAWallBroken = true;
                        }
                        else
                        {
                            break;
                        }                        
                    }
                }

                if(!wasAWallBroken)
                {
                    break;
                }

                if (Direction.x < 0)
                {
                    cellPosition.x--;
                }
                else if (Direction.x > 0)
                {
                    cellPosition.x++;
                }

                if (Direction.y < 0)
                {
                    cellPosition.y--; 
                }
                else if (Direction.y > 0)
                {
                    cellPosition.y++;
                }
            }

            if (wasAWallBroken)
            {
                SoundEffectPlayer
                    .Play(SoundEffectEnum.BreakWall, 1);

                WallBreaksAvailable--;
                UpdateWallBreakText();
                
                //DungeonBehaviour
                    //.UpdateWalls();

                WallSmashPower = 0;
            }
        }

        protected bool CheckIfCanMove(int x, int y, Vector3 position, ref Vector3 moveToPosition)
        {
            float stepSize = 0.01f;
            Vector3 cellSize = DungeonBehaviour.Walls.layoutGrid.cellSize;
            moveToPosition.x = position.x;
            moveToPosition.y = position.y;
            bool hitAWall = false;

            var movementStep = MovementStep;
            if (IsSpeedBoostActive)
            {
                movementStep *= 2;
            }

            if (x != 0)
            {
                float endValue;
                if (x < 0)
                {
                    endValue = position.x - movementStep;
                }
                else
                {
                    endValue = position.x + movementStep;
                }

                do
                {
                    var roundedPosX = moveToPosition.x.Quantize(cellSize.x);
                    var roundedPosY = moveToPosition.y.Quantize(cellSize.y);
                    var quantizedPosition = new Vector3(roundedPosX, roundedPosY, 0);

                    if (!moveToPosition.x.CloseTo(quantizedPosition.x, 0.001f))
                    {
                        moveToPosition.x += stepSize * x;
                        continue;
                    }

                    var cellPosition = DungeonBehaviour
                        .Walls
                        .WorldToCell(quantizedPosition);

                    cellPosition.x += x;

                    int sy, ey;
                    if (position.y.CloseTo(quantizedPosition.y, 0.001f))
                    {
                        sy = cellPosition.y;
                        ey = cellPosition.y;
                    }
                    else if (position.y > quantizedPosition.y)
                    {
                        sy = cellPosition.y;
                        ey = cellPosition.y + 1;
                    }
                    else
                    {
                        sy = cellPosition.y - 1;
                        ey = cellPosition.y;
                    }

                    for (int cy = sy; cy <= ey; cy++)
                    {
                        cellPosition = new Vector3Int(cellPosition.x, cy, 0);

                        var tile = DungeonBehaviour
                            .Walls
                            .GetTile(cellPosition);

                        if (tile != null)
                        {
                            hitAWall = true;
                            break;
                        }
                    }

                    if (hitAWall)
                    {
                        cellPosition.x -= x;
                        moveToPosition.x = cellPosition.x * DungeonBehaviour.Walls.layoutGrid.cellSize.x;
                        break;
                    }
                    else
                    {
                        moveToPosition.x += stepSize * x;
                    }
                } while (!moveToPosition.x.CloseTo(endValue, 0.001f));
            }

            if (y != 0)
            {
                float endValue;
                if (y < 0)
                {
                    endValue = position.y - movementStep;
                }
                else
                {
                    endValue = position.y + movementStep;
                }

                do
                {
                    var roundedPosX = moveToPosition.x.Quantize(cellSize.x);
                    var roundedPosY = moveToPosition.y.Quantize(cellSize.y);
                    var quantizedPosition = new Vector3(roundedPosX, roundedPosY, 0);

                    if (!moveToPosition.y.CloseTo(quantizedPosition.y, 0.001f))
                    {
                        moveToPosition.y += stepSize * y;
                        continue;
                    }

                    var cellPosition = DungeonBehaviour
                        .Walls
                        .WorldToCell(quantizedPosition);

                    cellPosition.y += y;

                    int sx, ex;
                    if (position.x.CloseTo(quantizedPosition.x, 0.001f))
                    {
                        sx = cellPosition.x;
                        ex = cellPosition.x;
                    }
                    else if (position.x > quantizedPosition.x)
                    {
                        sx = cellPosition.x;
                        ex = cellPosition.x + 1;
                    }
                    else
                    {
                        sx = cellPosition.x - 1;
                        ex = cellPosition.x;
                    }

                    for (int cx = sx; cx <= ex; cx++)
                    {
                        cellPosition = new Vector3Int(cx, cellPosition.y, 0);

                        var tile = DungeonBehaviour
                            .Walls
                            .GetTile(cellPosition);

                        if (tile != null)
                        {
                            hitAWall = true;
                            break;
                        }
                    }

                    if (hitAWall)
                    {
                        cellPosition.y -= y;
                        moveToPosition.y = cellPosition.y * DungeonBehaviour.Walls.layoutGrid.cellSize.y;
                        break;
                    }
                    else
                    {
                        moveToPosition.y += stepSize * y;
                    }
                } while (!moveToPosition.y.CloseTo(endValue, 0.001f));
            }

            return 
                moveToPosition.x != position.x || 
                moveToPosition.y != position.y;           
        }

        protected void TryMove(int x, int y)
        {
            if (MoveTimer > 0)
            {
                return;
            }

            Vector3 moveToPosition = Vector3.zero;
            if (CheckIfCanMove(x, y, transform.position, ref moveToPosition))
            {
                if (IsSpeedBoostActive)
                {
                    MoveTimer += MovementSpeed * 0.25f;
                }
                else
                {
                    MoveTimer += MovementSpeed;
                }
                

                if (!AudioSource.isPlaying)
                {
                    int index = Random
                        .Range(0, FootStepAudioClips.Length);

                    AudioSource
                        .clip = FootStepAudioClips[index];

                    AudioSource.volume = 0.25f;

                    AudioSource
                        .Play();
                }

                transform.position = moveToPosition;
            }
        }

        protected void Reset()
        {
            MovementSpeed = Hero
                .GetStatValue(HeroUpgradeType.MovementSpeed);

            MovementStep = Hero
                .GetStatValue(HeroUpgradeType.MovementDistance);

            WallBreaksAvailable = (int)Hero
               .GetStatValue(HeroUpgradeType.WallSmashCount);

            WallSmashRecovery = Hero
               .GetStatValue(HeroUpgradeType.WallSmashRecovery);

            WallSmashSize = (int)Hero
                .GetStatValue(HeroUpgradeType.WallSmashPower);

            SpeedBoostRecover = Hero
                .GetStatValue(HeroUpgradeType.SpeedBoostRecover);

            CameraVelocity = Vector3.zero;
            SetDirection(0, -1);
            ObtainKey(false);
            UpdateWallBreakText();
            IsDead = false;
            WallSmashPower = 1;
            SpeedBoostPower = 1;

            int attempts = 0;
            Vector3Int cellPosition = new Vector3Int(0, 0, 0);
            bool incrementX = true;
            do
            {
                var tile = DungeonBehaviour
                    .Walls
                    .GetTile(cellPosition);

                if (tile == null)
                {
                    var pos = DungeonBehaviour
                        .Walls
                        .CellToWorld(cellPosition);

                    transform.position = pos;

                    pos.z = -10;
                    Camera.main.transform.position = pos;

                    break;
                }

                if (incrementX)
                {
                    cellPosition.x++;
                    incrementX = false;
                }
                else
                {
                    cellPosition.y++;
                    incrementX = true;
                }
 
                attempts++;
                if (attempts > 100)
                {
                    break;
                }
            } while (true);
            

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
                if (orb.gameObject.CompareTag("FlareOrb"))
                {
                    SoundEffectPlayer
                        .Play(SoundEffectEnum.ReaperDisappear, 1);

                    ReaperMode
                        .Reset();
                }
                else if (Reaper.gameObject.activeInHierarchy && orb.gameObject.CompareTag("FreezeOrb"))
                {
                    SoundEffectPlayer
                        .Play(SoundEffectEnum.OrbFreeze, 1);

                    Reaper
                        .Freeze();
                }
                else if (orb.gameObject.CompareTag("DoorOrb"))
                {
                    SoundEffectPlayer
                        .Play(SoundEffectEnum.OrbCollect, 1);

                    ObtainOrb(orb);
                }

                MegaDestroy(orb.gameObject);
            }

            var reaper = collision
                .GetComponent<ReaperBehaviour>();

            if (reaper != null)
            {
                SoundEffectPlayer
                    .Play(SoundEffectEnum.LoseStinger, 1);

                IsDead = true;

                ExecuteAfterTime(5, () =>
                {
                    SceneManager
                        .LoadSceneAsync("UpgradeScene");
                });
            }

            var key = collision
                .GetComponent<KeyBehaviour>();

            if (key != null)
            {
                SoundEffectPlayer
                    .Play(SoundEffectEnum.KeyCollect, 1);

                ObtainKey(true);

                MegaDestroy(key.gameObject);
            }

            var door = collision
                .GetComponent<DoorBehaviour>();

            if (door != null)
            {
                if (HasKey && OrbCount == 4)
                {
                    HasWon = true;                    

                    GameState.Level++;

                    var currentStage = GameState.CurrentStage;

                    if (currentStage == null)
                    {
                        SceneManager
                            .LoadSceneAsync("WinningScene");
                    }
                    else
                    {
                        Hero
                            .ChangeCoins(100 * GameState.Level);

                        SceneManager
                            .LoadSceneAsync("UpgradeScene");
                    }
                } 
                else
                {
                    SoundEffectPlayer
                        .Play(SoundEffectEnum.DoorWontOpen, 1);
                }
            }

            var coin = collision
                .GetComponent<CoinBehaviour>();

            if (coin != null)
            {
                ObtainCoin(coin);

                MegaDestroy(coin.gameObject);
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

            if (SpeedBoostTimer > 0)
            {
                SpeedBoostTimer -= Time.deltaTime;
                if (SpeedBoostTimer <= 0)
                {
                    SpeedBoostTimer = 0;
                }
            }

            if (MoveTimer > 0)
            {
                MoveTimer -= Time.deltaTime;
                if (MoveTimer <= 0)
                {
                    MoveTimer = 0;
                }
            }

            if (WallSmashPower < 1)
            {
                WallSmashPower += Time.deltaTime * WallSmashRecovery;

                if (WallSmashPower >= 1)
                {
                    WallSmashPower = 1;
                }
            }

            WallSmashBar
                .SetValues(WallSmashPower, 1);

            if (SpeedBoostPower < 1)
            {
                SpeedBoostPower += Time.deltaTime * SpeedBoostRecover;

                if (SpeedBoostPower >= 1)
                {
                    SpeedBoostPower = 1;
                }
            }

            SpeedBoostBar
                .SetValues(SpeedBoostPower, 1);

            if (Input.GetKey(KeyCode.DownArrow))
            {
                SetDirection(0, -1);
                TryMove(0, -1);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                SetDirection(0, 1);
                TryMove(0, 1);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                SetDirection(-1, 0);
                TryMove(-1, 0);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                SetDirection(1, 0);
                TryMove(1, 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) ||
                Input.GetKeyDown(KeyCode.Z))
            {
                TryBreakWall();
            }

            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.X))
            {
                TrySpeedBoost();
            }
        }

        protected override void Awake()
        {
            base
                .Awake();

            Hero = GameState.Hero;

            SoundEffectPlayer = FindObjectOfType<SoundEffectPlayerBehaviour>();
            DungeonBehaviour = FindObjectOfType<DungeonBehaviour>();
            Reaper = FindObjectOfType<ReaperBehaviour>();
            Door = FindObjectOfType<DoorBehaviour>();
            AudioSource = GetComponent<AudioSource>();
            
            Reset();
        }

        #endregion
    }
}