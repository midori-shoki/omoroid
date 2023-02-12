using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    GameObject player;
    Camera mainCamera;
    Vector3 playerPos;

    //カメラの最も左の座標
    private Vector2 leftMost;

    public Vector2 LeftMost
    {
        get { return leftMost; }
        set { leftMost = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //プレイヤーの位置に追従する
        playerPos = this.player.transform.position;

        var right = mainCamera.ViewportToWorldPoint(Vector2.right);
        Vector2 left = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        var center = mainCamera.ViewportToWorldPoint(Vector2.one * 0.5f);

        //plaerControllerで使う
        LeftMost = left;

        // Update 関数が呼び出された後に実行される
        //カメラを追従するには Update で移動された結果を基に常に LateUpdate で位置を更新する
        //if (center.x < playerPos.x)
        //{
            var pos = mainCamera.transform.position;

            //if (Mathf.Abs(pos.x - playerPos.x) >= 0.0000001f)
            //{
                mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y, pos.z);
            //}
        //}

        //stopPosition1背景の終点の位置
        //if (stopPosition.position.x - right.x < 0)
        //{
        //    StartCoroutine(INTERNAL_Clear());
        //    enabled = false;
        //}
    }
}
