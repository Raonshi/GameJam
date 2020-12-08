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
    public float dashEnergy;
    public int haveSouls;   //가지고 있는 소울양
    public int catchCount;
    public float speed;
    public bool isDash;
    public bool isDraw;
    public bool isLined;        //플레이어가 선을 따라 걸을 경우 
    public bool isComplete;
    public bool isParanomal;

    public List<Tile> lineList = new List<Tile>();
    public Game game;
    public Game.Direction direction;

    public GameObject InteractIcon; //아이콘 오브젝트
    public Transform movePoint;
    public LayerMask StopMovementLayer;
    public LayerMask Visionable;

    private Vector2 boxSize = new Vector2(4.0f, 4.0f);//상호작용 할수 있는 거리 조정


    
    public int lineDuplicateCount = 0;

    public GameObject currentInteract;
    public int currentInteractIndex = 0;
    public GameObject paranomalFilter;

    //Draw 호출 쿨타임
    public float time;

    public bool hasKey;

    public Item item;

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
        currentInteract = null;
        isDraw = false;
        isDash = false;
        isParanomal = false;
        hasKey = false;
        catchCount = 0;
        time = 0f;
        state = State.Idle;

        speed = moveSpeed;
        dashEnergy = maxDashEnergy;

        game = Game.instance;
    }

    // Update is called once per frame
    void Update()
    {
        isLined = false;
        time += Time.deltaTime;
        
        isComplete = false;
        Control();

        if(state != State.Dash && dashEnergy < maxDashEnergy)
        {
            DashEnergyRecovery();
        }

        if (Input.GetKey(KeyCode.LeftShift) == false)
        {
            Line();
        }

        if (isDraw == true)
        {
            Drawing();


            CheckQixComplete();
        }

        Debug.Log(catchCount);
    }

    public void Control()
    {
        if(Input.anyKey == false)
        {
            isDash = false;
            state = State.Idle;
            return;
        }

        if(Input.GetKey(KeyCode.LeftShift) == false)
        {
            Move();
            Dash();
            Action();
        }
        //ParanomalVision();
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isDash = true;
            speed = dashSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
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

    public void ParanomalVision()
    {
        Debug.Log("파라노말 비전 활성화");
        // 파라노말 비전
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isParanomal = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentInteract = null;
            Camera.instance.player = Player.instance.transform;
            isParanomal = false;
        }

        paranomalFilter.SetActive(isParanomal);



        ViewEnemyWay();

        SelectInteractObject();

    }


    public void ViewEnemyWay()
    {
        GameObject[] enemyArray;

        if (isParanomal == false)
        {
            enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemyArray.Length; i++)
            {
                enemyArray[i].GetComponent<Enemy>().trail.SetActive(false);
            }
            return;
        }

        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].GetComponent<Enemy>().trail.SetActive(true);
        }
    }

    public void SelectInteractObject()
    {
        if (isParanomal == false)
        {
            return;
        }

        GameObject[] interactArray = GameObject.FindGameObjectsWithTag("Interact");

        currentInteract = interactArray[currentInteractIndex];

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentInteractIndex++;
            if (currentInteractIndex >= interactArray.Length)
            {
                currentInteractIndex = interactArray.Length - 1;
            }
            currentInteract = interactArray[currentInteractIndex];
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentInteractIndex--;
            if (currentInteractIndex < 0)
            {
                currentInteractIndex = 0;
            }
            currentInteract = interactArray[currentInteractIndex];
        }
        Camera.instance.player = currentInteract.transform;
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

            LineDispose();
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
        
        for (int i = 0; i < lineList.Count - 1; i++)
        {
            if (Vector3.Distance(transform.position, lineList[i].transform.position) <= .05f)
            {
                Debug.Log("line duplicate count ++!!!");
                lineDuplicateCount++;
                break;
            }
        }

        if (lineDuplicateCount >= lineList.Count)
        {
            //isLined = true;
            LineDispose();
        }
        else
        {
            //isLined = false;
            ComeBackStart();
        }
    }


    public void ComeBackStart()
    {
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
                    
                    lineDuplicateCount = 0;
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
        int start = 0; 
        int end = 0;

        for (int x = 0; x < Game.instance.xSize; x++)
        {
            //아래부터 INSIDE인 타일을 찾는다.
            //INSIDE인 타일이 나타나면 해당 타일 값을 저장한 뒤 for문을 탈출한다.
            for (int y1 = 0; y1 <game.ySize; y1++)
            {
                int tmp = (x * 15) + y1;
                Tile tile = game.tileList[tmp].GetComponent<Tile>();
                if (tile.state == Tile.State.INSIDE)
                {
                    start = tmp;
                    break;
                }
            }

            //위부터 INSIDE인 타일을 찾는다.
            //INSIDE인 타일이 나타나면 해당 타일 값을 저장한 뒤 for문을 탈출한다.
            for (int y2 = Game.instance.ySize - 1; y2 >= 0; y2--)
            {
                int tmp = (x * 15) + y2;
                Tile tile = game.tileList[tmp].GetComponent<Tile>();
                if (tile.state == Tile.State.INSIDE)
                {
                    end = tmp;
                    break;
                }
            }

            //start == end일 경우 선이므로 건너뛴다.
            if (start == end)
            {
                continue;
            }

            //start부터 end까지 반복문으로 INSIDE로 만든다.
            for (int k = start; k <= end; k++)
            {
                game.tileList[k].GetComponent<Tile>().state = Tile.State.INSIDE;
            }

            start = 0;
            end = 0;
        }
        //end of for
        isComplete = true;
        

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

    public void LineDispose()
    {
        for (int i = 0; i < lineList.Count; i++)
        {
            lineList[i].state = Tile.State.OUTSIDE;
            lineList[i].isStart = false;
        }
        lineList.Clear();
        lineDuplicateCount = 0;
    }

    private void OnDrawGizmos()
    {
        Vector3 originPos = transform.position;
        Gizmos.DrawWireCube(originPos, boxSize);
    }
}
