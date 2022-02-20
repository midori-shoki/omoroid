using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5; //銃弾のスピード

    private GameObject Player;
    private PlayerController playerController;
    private int direcion = 0;

    Vector3 shootPos;

    void Start()
    {
        this.Player = GameObject.Find("Player");
        playerController = Player.GetComponent<PlayerController>();

        shootPos = transform.position;
        direcion = playerController.playerDireciton;

    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }

    public void Move()
    {
        if (direcion < 0)
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