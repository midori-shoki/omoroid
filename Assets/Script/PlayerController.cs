using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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

    [SerializeField]
    private HitPointController hitPointController;

    public bool gloundFlag;
    public bool jumpButtonFlag;
    public bool attackButtonFlag;
    public float gravityScale;
    public float jumpHeight;

    private bool isJump;
    private float jumpPos = 0.0f;

    public float life;
    public float maxLife = 100;


    protected GameObject textDisplay;

    new Rigidbody2D rigidbody2D;

    public LayerMask groundlayer;

    public float attackDirection;

    GameObject gameDirectorObject;
    GameDirector gameDirector;

    [SerializeField]
    private SpriteRenderer sp;

    // ダメージ判定フラグ
    private bool isDamage { get; set; }

    void Start()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        currentAttackTime = attackTime;
        textDisplay = GameObject.Find("TextDisplay");
        playerScaleX = gameObject.transform.localScale.x;
        playerScaleY = gameObject.transform.localScale.y;

        gameDirectorObject = GameObject.Find("GameDirector");
        gameDirector = gameDirectorObject.GetComponent<GameDirector>();

        life = maxLife;
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
                //rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, ySpeed);
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

        // ダメージを受けている場合、点滅させる
        if (isDamage)
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            sp.color = new Color(1f, 1f, 1f, level);

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

            if (isDamage)
            {
                return;
            }

            Damage(20);
            OnDamage(this.GetCancellationTokenOnDestroy()).Forget();

            //textDisplay.SetActive(true);
        }
    }

    //ダメージ処理
    public void Damage(float damagePoint)
    {
        isDamage = true;
        hitPointController.GaugeReduction(damagePoint);
        life -= damagePoint;

        if (life <= 0)
        {
            Destroy(gameObject);
            gameDirector.GameOverSceneTransition();
        }
    }

    //ダメージ点滅コルーチン
    public async UniTask OnDamage(CancellationToken cancellation_token)
    {

        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation_token);

        // 通常状態に戻す
        isDamage = false;
        sp.color = new Color(1f, 1f, 1f, 1f);

    }
}
