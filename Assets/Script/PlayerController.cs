using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    private float playerScaleX = 1;
    private float playerScaleY = 1;

    [SerializeField]
    private float walkForce = 2.0f;

    [SerializeField]
    private float jumpSpeed = 10.0f;

    public bool gloundFlag;
    public bool jumpButtonFlag;
    public bool attackButtonFlag;
    public float gravityScale;
    public float jumpHeight;

    private bool isJump;
    private float jumpPos = 0.0f;

    protected GameObject textDisplay;

    new Rigidbody2D rigidbody2D;

    public LayerMask groundlayer;

    public float attackDirection;

    void Start()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        currentAttackTime = attackTime;
        textDisplay = GameObject.Find("TextDisplay");
        playerScaleX = gameObject.transform.localScale.x;
        playerScaleY = gameObject.transform.localScale.y;
    }

    void FixedUpdate()
    {
        //キー入力されたら行動する
        float horizontalKey = Input.GetAxis("Horizontal");

        float xSpeed = 0.0f;
        float ySpeed = gravityScale;

        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(playerScaleX, playerScaleY, 1);
            xSpeed = speed;
        }
        else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-playerScaleX, playerScaleY, 1);
            xSpeed = -speed;
        }
        else
        {
            xSpeed = 0.0f;
        }

        float verticalKey = Input.GetAxis("Vertical");

        bool isGround = Physics2D.Linecast(transform.position, transform.position - transform.up, groundlayer);

        if (isGround)
        {
            if (verticalKey > 0)
            {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y;
                isJump = true;

                Debug.Log("上昇");
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, ySpeed);
            }
            else
            {
                Debug.Log("接地");
                isJump = false;
            }
        }
        else if (isJump)
        {
            //上ボタンを押されている。かつ、現在の高さがジャンプした位置から自分の決めた位置より下ならジャンプを継続する
            if (verticalKey > 0 && jumpPos + jumpHeight > transform.position.y)
            {
                ySpeed = jumpSpeed;
            }
            else
            {
                isJump = false;
            }
        }

        rigidbody2D.velocity = new Vector2(xSpeed, ySpeed);

        if (Input.GetKey("space"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        attackTime += Time.deltaTime; //attackTimeに毎フレームの時間を加算していく

        if (attackTime > currentAttackTime)
        {
            canAttack = true; //指定時間を超えたら攻撃可能にする
        }

        if (canAttack)
        {
            attackDirection = transform.localScale.x;
            //第一引数に生成するオブジェクト、第二引数にVector3型の座標、第三引数に回転の情報
            //Instantiate(lazer, attackPoint.position, Quaternion.identity);
            GameObject playerShot = Instantiate(lazer) as GameObject;
            playerShot.transform.position = attackPoint.position;
            canAttack = false; //攻撃フラグをfalseにする
            attackTime = 0f; //attackTimeを0に戻す
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
