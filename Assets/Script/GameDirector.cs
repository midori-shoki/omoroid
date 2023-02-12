using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.UI;
using TMPro;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController = null;


    private GameObject[] enemyObjects;
    private int enemyNum;

    [SerializeField]
    private TextMeshProUGUI enemysCounts;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private float timer;
    private int time;

    private string sceneResult;

    void Start()
    {
        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        enemyNum = enemyObjects.Length;

        enemysCounts.GetComponent<TextMeshProUGUI>().text = enemyNum.ToString();

        timer = float.Parse(timerText.text);
    }

    void Update()
    {
        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        enemyNum = enemyObjects.Length;

        enemysCounts.text = enemyNum.ToString();

        if (enemyNum == 0)
        {
            ClearSceneTransition();
        }

        timer -= Time.deltaTime;
        time = (int)timer;
        timerText.text = time.ToString();

        if (time == 0)
        {
            GameOverSceneTransition();
        }
    }

    public void GameOverSceneTransition()
    {
        sceneResult = "ゲームオーバー";
        SceneFade(this.GetCancellationTokenOnDestroy()).Forget();
    }

    public void ClearSceneTransition()
    {
        sceneResult = "クリア";
        SceneFade(this.GetCancellationTokenOnDestroy()).Forget();
    }

    //急に終わると変な感じがするので、1秒待つ
    private async UniTask SceneFade(CancellationToken cancellation_token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5), cancellationToken: cancellation_token);

        switch (sceneResult)
        {
            case "クリア":
                SceneManager.LoadScene("ClearScene");
                break;
            case "ゲームオーバー":
                //残機を減らす
                SceneManager.LoadScene("GameOverScene");

                break;
            default:
                Debug.Log("シーンをロードできません。");
                break;
        }
    }
}
