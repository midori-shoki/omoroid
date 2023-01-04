using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5; //銃弾のスピード

    private GameObject Player;
    private PlayerController playerController;

    private float xDirection;

    Vector2 shootPos;

    void Start()
    {
        this.Player = GameObject.Find("Player");
        playerController = Player.GetComponent<PlayerController>();

        xDirection = playerController.transform.localScale.x;
        shootPos = transform.position;

    }

    // Update is called once per frame
    void  FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (xDirection < 0)
        {
            shootPos.x -= speed * Time.deltaTime; //x座標にspeedを加算
        }
        else
        {
            shootPos.x += speed * Time.deltaTime; //x座標にspeedを加算
        }

        transform.position = shootPos; //現在の位置情報に反映させる
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}