using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //플레이어의 능력치
    public float moveSpeed;
    public float dashSpeed;
    public float maxDashEnergy;
    public float maxLineEnergy;
    public float dashEnergy;
    public float lineEnergy;
    public int haveSouls;   //가지고 있는 소울양
    public int catchCount;
    float speed;
    bool isDash;
    public bool isDraw;
    public bool isLined;        //플레이어가 선을 따라 걸을 경우 
    public bool isComplete;

    public List<Tile> lineList = new List<Tile>();
    public Game game;
    public Game.Direction direction;

    public GameObject InteractIcon; //아이콘 오브젝트
    public Transform movePoint;
    public LayerMask StopMovementLayer;
    public LayerMask Visionable;

    private Vector2 boxSize = new Vector2(4.0f, 4.0f);//상호작용 할수 있는 거리 조정
    private Vector2 VisionSize = new Vector2(50.0f, 30.0f);//비전 거리(사실상 방전체)

    public GameObject[] enemies;

    //Draw 호출 쿨타임
    public float time;

    public Item item;
    Draw draw;

    public enum State
    {
        Idle,
        Move, 
        Dash,
        Vision
    }
    public State state;

    public static Player instance;

    private void Awake()
    {
        instance = this;
        isDraw = false;
        isLined = false;
        isComplete = false;
        direction = Game.Direction.NULL;
    }

    // Start is called before the first frame update
    void Start()
    {
        item = null;
        movePoint.parent = null;
        isDraw = false;
        isDash = false;
        catchCount = 0;
        time = 0f;
        state = State.Idle;

        draw = new Draw();

        speed = moveSpeed;
        dashEnergy = maxDashEnergy;
        lineEnergy = maxLineEnergy;

        game = Game.instance;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        isComplete = false;
        Control();

        if(state != State.Dash && dashEnergy < maxDashEnergy)
        {
            DashEnergyRecovery();
        }

        Line();

        if (isDraw == true && lineEnergy > 0)
        {
            Drawing();

            CheckQixComplete();         
        }
        else if (isDraw == false && lineEnergy < 6)
        {
            DrawEnergyRecovery();
        }

        ClearCheck();
        Debug.Log(catchCount);
        Debug.Log(enemies.Length);
    }

    public void Control()
    {
        if(Input.anyKey == false)
        {
            isDash = false;
            state = State.Idle;
            return;
        }
        Move();
        Dash();
        Action();
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Game.Direction.UP;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Game.Direction.LEFT;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Game.Direction.DOWN;
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = Game.Direction.RIGHT;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (isDash)
        {
            state = State.Dash;
            dashEnergy -= Time.deltaTime;
            if (dashEnergy <= 0)
            {
                isDash = false;
                speed = moveSpeed;
                return;
            }
            else
            {
                state = State.Move;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, StopMovementLayer))
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, StopMovementLayer))
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            }
        }
    }

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isDash = true;
            speed = dashSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isDash = false;
            speed = moveSpeed;
        }
    }

    public void Action()
    {
        if (Input.GetKeyDown(KeyCode.E))
            CheckInteraction();
    }

    public void Line()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDraw = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isDraw = false;

            for (int j = 0; j < lineList.Count; j++)
            {
                lineList[j].state = Tile.State.OUTSIDE;
            }

            lineList.Clear();
        }
    }

    public void Drawing()
    {
        for (int i = 0; i < game.tileList.Count; i++)
        {
            if (Vector3.Distance(transform.position, game.tileList[i].position) <= .05f)
            {
                Tile tile = game.tileList[i].GetComponent<Tile>();

                if (tile.state == Tile.State.INSIDE || tile.state == Tile.State.CLEAR)
                {
                    return;
                }

                tile.state = Tile.State.INSIDE;
                lineList.Add(game.tileList[i].GetComponent<Tile>());

                if (lineList.Count == 1)
                {
                    lineList[0].isStart = true;
                }
            }
            //end of first if
        }
        //end of for
    }

    public void CheckQixComplete()
    {
        //lineList.Count가 2개 이상일 경우에만 체크한다.
        if (lineList.Count < 2)
        {
            return;
        }

        //플레이어의 다음 이동 지점이 INSIDE타일이면 isLined가 true가 된다.
        for (int i = 0; i < game.tileList.Count; i++)
        {
            Tile currentTile = new Tile();
            Tile nextTile = new Tile();

            if (Vector3.Distance(movePoint.transform.position, game.tileList[i].position) <= .05f)
            {
                nextTile = game.tileList[i].GetComponent<Tile>();
            }
            else if (Vector3.Distance(transform.position, game.tileList[i].position) <= .05f)
            {
                currentTile = game.tileList[i].GetComponent<Tile>();
            }
            //end of if

            if ((currentTile.state == Tile.State.INSIDE) && (nextTile.state == Tile.State.INSIDE))
            {
                isLined = true;
            }
            else
            {
                isLined = false;
            }
            //end of if
        }
        //enf of for

        //isStart가 지정된 타일에 도착한 경우
        //isLined가 true면 lineList의 모든 값을 초기화한다.
        //isLined가 false면 땅따먹기를 시작한다.
        for (int i = 0; i < game.tileList.Count; i++)
        {
            if (Vector3.Distance(transform.position, game.tileList[i].position) <= .05f)
            {
                Tile tile = game.tileList[i].GetComponent<Tile>();
                if (tile.isStart == true)
                {
                    if (Game.instance.count >= 4)
                    {
                        QixComplete();
                    }
                }
                //end of second if
            }
            //end of first if
        }
        //end of for
    }

    //땅따먹기를 실행한다.
    public void QixComplete()
    {
        int i = 0;

        for (int x = 0; x < Game.instance.xSize; x++)
        {
            int y = 0;
            int tmp = 0;
            int start = 0;
            int end = 0;

            while (y < Game.instance.ySize)
            {
                if (game.tileList[i].GetComponent<Tile>().state == Tile.State.INSIDE)
                {
                    if (tmp == 0)
                    {
                        start = i;
                    }
                    else if (tmp == 1)
                    {
                        end = i;
                    }
                    tmp += 1;
                }
                i++;
                y++;
            }

            if (tmp >= 2)
            {
                for (int k = start; k <= end; k++)
                {
                    game.tileList[k].GetComponent<Tile>().state = Tile.State.INSIDE;
                }
            }
        }

        isComplete = true;

        //end of for

        for (int j = 0; j < game.tileList.Count; j++)
        {
            Tile tile = game.tileList[j].GetComponent<Tile>();

            if (tile.state == Tile.State.INSIDE)
            {
                tile.isStart = false;
                game.clearList.Add(tile);
            }
        }

        if (isComplete == true)
        {
            lineList.Clear();
        }

        Game.instance.Clear();
    }

    public void DashEnergyRecovery()
    {
        dashEnergy += 0.5f * Time.deltaTime;
    }

    public void DrawEnergyRecovery()
    {
        float time = 0;

        while(time > 2.0f)
        {
            time += Time.deltaTime;
        }

        lineEnergy += Time.deltaTime;
    }

    public void ClearCheck()
    {
        float CheckCount = game.tileList.Count;
        float ClearCount = game.clearList.Count;

        if ((ClearCount / CheckCount) >= 0.8f || catchCount == enemies.Length)
        {
            Debug.Log("Clear");
            SceneManager.LoadScene("Shop");
        }
        Debug.Log(ClearCount / CheckCount);
    }

    //->상호작용 아이콘 코딩
    public void OpenInteractableIcon()
    {
        //InteractIcon.SetActive(true);
        Debug.Log("근처에 상호작용 가능한 물체가 있습니다.");
    }

    public void CloseInteractableIcon()
    {
        //InteractIcon.SetActive(false);
        Debug.Log("근처에 상호작용 가능한 물체가 없습니다.");
    }

    void CheckInteraction()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, Vector2.zero);

        if (hits.Length > 0)
        {
            foreach(RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    return;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 originPos = transform.position;
        Gizmos.DrawWireCube(originPos, boxSize);
        Gizmos.DrawWireCube(originPos, VisionSize);
    }
}
