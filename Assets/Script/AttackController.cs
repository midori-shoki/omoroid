using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController = null;
    private float jumporce = 2.0f;

    new Rigidbody2D rigidbody2D;

    void Start()
    {
        playerController.attackButtonFlag = false;
    }

    public void Down()
    {
        playerController.attackButtonFlag = true;
    }

    public void Up()
    {
        playerController.attackButtonFlag = false;
    }
}
