using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EnemyController : MonoBehaviour
{
    protected GameObject Player;
    protected PlayerController playerController;

    protected Animator animator;
    protected GameObject gameDirector;
    //protected GameDirector gameDirectorScript;

    void Awake()
    {
        //Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.Player = GameObject.Find("Player");
        playerController = Player.GetComponent<PlayerController>();
        //this.animator = GetComponent<Animator>();

        //this.gameDirector = GameObject.Find("GameDirector");
        //gameDirectorScript = gameDirector.GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        //落下死
        if (this.transform.position.y <= -10)
        {
            DestroyEnemey();
        }
    }

    public void HitEnemey()
    {
        //animator.SetBool("UniDeath", true);
        //gameDirectorScript.ScoreChange("敵");
    }

    public void DestroyEnemey()
    {
        Debug.Log("敵が死亡");
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("弾と接触");
            Destroy(gameObject);
        }
    }
}

