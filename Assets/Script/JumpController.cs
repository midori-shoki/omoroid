using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController = null;
    private float jumporce = 2.0f;

    new Rigidbody2D rigidbody2D;

    void Start()
    {
    }

    public void Down()
    {
        playerController.jumpButtonFlag = true;
    }

    public void Up()
    {
        playerController.jumpButtonFlag = false;
    }
}
