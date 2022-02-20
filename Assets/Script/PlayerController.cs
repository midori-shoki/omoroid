using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    VariableJoystick joystick;

    [SerializeField]
    float speed = 1.0f;

    [SerializeField]
    GameObject lazer; //レーザープレハブを格納
    [SerializeField]
    private Transform attackPoint;//アタックポイントを格納

    [SerializeField]
    private float attackTime = 0.2f; //攻撃の間隔
    [SerializeField]
    private float currentAttackTime; //攻撃の間隔を管理
    [SerializeField]
    private bool canAttack; //攻撃可能状態かを指定するフラグ

    //private CharacterController controller;
    private Vector3 direction;
    private int xDirection;
    private bool moveFlag;
    private const int playerScale = 3;

    [SerializeField]
    private float walkForce = 2.0f;

    const float maxWalkForce = 3.2f;

    [SerializeField]
    private float jumpForce = 10.0f;

    private float maxJumpForce = 0.2f;
    public bool gloundFlag;
    public bool jumpButtonFlag;
    public bool attackButtonFlag;
    private bool jumpFlag;
    private float oldPostion;

    protected GameObject textDisplay;

    new Rigidbody2D rigidbody2D;

    public LayerMask groundlayer;

    public enum playerPositon : int
    {
        right = 1,
        left = -1
    }

    public int playerDireciton = (int)playerPositon.right;

    void Start()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        currentAttackTime = attackTime;
        textDisplay = GameObject.Find("TextDisplay");
        xDirection = playerScale;
    }

    void Update()
    {
        //キャラが向く方向
        transform.localScale = new Vector3(xDirection, playerScale, 1);
        

        if (moveFlag)
        {
            Move();
        }

        if (Input.GetKeyDown("space"))
        {
            jumpButtonFlag = true;
        }

        if (jumpButtonFlag)
        {
            //oldPostion = rigidbody2D.velocity.y;
            Jump();
        }

        if (Input.GetKeyUp("space"))
        {
            jumpButtonFlag = false;
        }

        if (attackButtonFlag)
        {
            Attack();
        }
    }

    public void FixedUpdate()
    {
        direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;

        if (direction.x > 0)
        {
            xDirection = playerScale;
            playerDireciton = (int)playerPositon.right;
        }
        else　if (direction.x < 0)
        {
            xDirection = -playerScale;
            playerDireciton = (int)playerPositon.left;
        }

        if (direction.x == 0)
        {
            moveFlag = false;
        }
        else
        {
            moveFlag = true;
        }
    }

    private void Move()
    {
        //x方向へのスピード
        Vector2 speed = transform.right * (xDirection / playerScale) * walkForce;

        if (Mathf.Abs(speed.x) > maxWalkForce)
        {
            if (speed.x > 0)
            {
                speed.x = maxWalkForce;
            }
            else
            {
                speed.x = -maxWalkForce;
            }
        }

        Debug.Log($"今:{speed.x}");

        rigidbody2D.AddForce(speed);
    }

    private void Jump()
    {
        bool grounded = Physics2D.Linecast(transform.position, transform.position - transform.up, groundlayer);

        if (grounded)
        {
            jumpFlag = true;
            Debug.Log("ジャンプできる");
        }

        if (transform.localPosition.y < 1 && jumpFlag == true)
        {
            rigidbody2D.AddForce(transform.up * jumpForce);
        }

        if (oldPostion > rigidbody2D.velocity.y)
        {
            jumpFlag = false;
            Debug.Log("ジャンプできない");
        }
    }

    private void Attack()
    {
        attackTime += Time.deltaTime; //attackTimeに毎フレームの時間を加算していく

        if (attackTime > currentAttackTime)
        {
            canAttack = true; //指定時間を超えたら攻撃可能にする
        }

        if (attackButtonFlag) //ボタンを押したら
        {
            if (canAttack)
            {
                //第一引数に生成するオブジェクト、第二引数にVector3型の座標、第三引数に回転の情報
                Instantiate(lazer, attackPoint.position, Quaternion.identity);
                canAttack = false;　//攻撃フラグをfalseにする
                attackTime = 0f;　//attackTimeを0に戻す
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("敵と接触");
            //textDisplay.SetActive(true);
            Destroy(gameObject);
        }
    }
}
