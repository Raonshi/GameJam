using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //플레이어의 능력치
    public float moveSpeed;
    public float dashSpeed;
    public float maxDashEnergy;
    public float maxLineEnergy;

    public float dashEnergy;
    public float lineEnergy;
    float speed;

    bool isDash;
    public bool isDraw;

    Draw draw;

    public enum State
    {
        Idle,
        Move, 
        Dash,
    }
    public State state;

    public static Player instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isDraw = false;
        isDash = false;
        state = State.Idle;

        draw = new Draw();

        speed = moveSpeed;
        dashEnergy = maxDashEnergy;
        lineEnergy = maxLineEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        Control();

        if(state != State.Dash && dashEnergy < maxDashEnergy)
        {
            DashEnergyRecovery();
        }


        if(isDraw == true && lineEnergy > 0)
        {
            draw.Drawing();
        }
        else if(isDraw == false && lineEnergy < 6)
        {
            DrawEnergyRecovery();
        }
        
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
            if(isDash == true)
            {
                state = State.Dash;
                dashEnergy -= Time.deltaTime;

                if (dashEnergy <= 0)
                {
                    isDash = false;
                    speed = moveSpeed;
                    return;
                }
            }
            else
            {
                state = State.Move;
            }

            transform.rotation = Quaternion.Euler(0, 0, 90);
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (isDash == true)
            {
                state = State.Dash;
                dashEnergy -= Time.deltaTime;

                if (dashEnergy <= 0)
                {
                    isDash = false;
                    speed = moveSpeed;
                    return;
                }
            }
            else
            {
                state = State.Move;
            }

            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (isDash == true)
            {
                state = State.Dash;
                dashEnergy -= Time.deltaTime;

                if (dashEnergy <= 0)
                {
                    isDash = false;
                    speed = moveSpeed;
                    return;
                }
            }
            else
            {
                state = State.Move;
            }

            transform.rotation = Quaternion.Euler(0, 0, 270);
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (isDash == true)
            {
                state = State.Dash;
                dashEnergy -= Time.deltaTime;

                if(dashEnergy <= 0)
                {
                    isDash = false;
                    speed = moveSpeed;
                    return;
                }
            }
            else
            {
                state = State.Move;
            }

            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector2.right * speed * Time.deltaTime);
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
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Paranomal Site!");
        }
        
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            isDraw = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            isDraw = false;
        }    
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
}
