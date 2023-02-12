using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HopperController : EnemyController
{

    [SerializeField]
    float walkForce = 0.0f;

    float maxWalkforce = 2.0f;

    //キャラの向き
    private float direction;

    //初めて見えた時だけ動かす
    private bool firstVisible = true;

    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;

    private float defaultWalkForce;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        //this.spriteRenderer = GetComponent<SpriteRenderer>();
        defaultWalkForce = walkForce;
        direction = transform.localScale.x;

        life = maxLife;

        //Sequenceのインスタンスを作成
        var sequence = DOTween.Sequence();

        //(5,0,0)の位置に4秒で2回ジャンプして移動する
        sequence.Append(transform.DOLocalJump(new Vector3(gameObject.transform.localPosition.x + 3f, gameObject.transform.localPosition.y + 0.05f), jumpPower: 2f, numJumps: 1, duration: 1f));
        sequence.Append(transform.DOLocalJump(new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + 0.05f), jumpPower: 2f, numJumps: 1, duration: 1f));
        //sequence.SetEase(Ease.Linear);
        this.Player = GameObject.Find("Player");
        //sequence.Append(transform.DOLocalJump(new Vector3(Player.transform.localPosition.x, gameObject.transform.localPosition.y + 0.1f), jumpPower: 2f, numJumps: 3, duration: 3f));
        sequence.Play().SetLoops(-1);
        sequence.SetLink(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        if (turnFlag)
        {
            walkForce *= -1;
            turnFlag = false;

            //向く方向
            direction = -1 * transform.localScale.x;
        }

        //キャラが向く方向
        transform.localScale = new Vector3(direction, transform.localScale.y, 1);

        bool isGround = Physics2D.Linecast(transform.position, transform.position - transform.up, groundlayer);

        if (isGround)
        {
            this.animator.SetBool("HopperJump", false);
        }
        else
        {
            this.animator.SetBool("HopperJump", true);
        }

        //if (spriteRenderer.isVisible)
        //{
        //    firstVisible = true;
        //}

        //if (firstVisible)
        //{
        //    //x方向へのスピード
        //    float speedX = Mathf.Abs(this.rigidbody2D.velocity.x);

        //    if (speedX < maxWalkforce)
        //    {
        //        rigidbody2D.AddForce(transform.right * walkForce);
        //    }

        //}
        //
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("弾と接触");
            life -= 40;

            if(life <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
