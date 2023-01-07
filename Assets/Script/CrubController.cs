using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrubController : EnemyController
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
        //this.spriteRenderer = GetComponent<SpriteRenderer>();
        defaultWalkForce = walkForce;
        direction = transform.localScale.x;
    }

    // Update is called once per frame
    void FixedUpdate()
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

        //if (spriteRenderer.isVisible)
        //{
        //    firstVisible = true;
        //}

        if (firstVisible)
        {
            //x方向へのスピード
            float speedX = Mathf.Abs(this.rigidbody2D.velocity.x);

            if (speedX < maxWalkforce)
            {
                rigidbody2D.AddForce(transform.right * walkForce);
            }

        }

    }
}
