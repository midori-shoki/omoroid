using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrubController : EnemyController
{

    [SerializeField]
    float walkForce = 0.0f;

    float maxWalkforce = 2.0f;

    //キャラの向き
    private int direction;

    //初めて見えた時だけ動かす
    private bool firstVisible = true;

    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        //this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        //向く方向
        if (walkForce > 0)
        {
            direction = -5;
        }
        else
        {
            direction = 5;
        }

        //キャラが向く方向
        transform.localScale = new Vector3(direction, 5, 1);

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
