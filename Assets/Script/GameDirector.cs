using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController = null;

    private string sceneResult;

    void Start()
    {
    }

    public void GameOverSceneTransition()
    {
        sceneResult = "ゲームオーバー";
        SceneFade(this.GetCancellationTokenOnDestroy()).Forget();
    }

    //急に終わると変な感じがするので、1秒待つ
    private async UniTask SceneFade(CancellationToken cancellation_token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation_token);

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
