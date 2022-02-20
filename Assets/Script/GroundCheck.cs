using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //player
    GameObject player;
    PlayerController playerController;

    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerController.gloundFlag = true;
        Debug.Log("何かが判定に入りました");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("判定中");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerController.gloundFlag = false;
        Debug.Log("何かが判定をでました");
    }
}