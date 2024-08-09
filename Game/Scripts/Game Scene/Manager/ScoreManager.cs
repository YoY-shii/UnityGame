using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    //Field
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI resultText;

    int totalScore = 0;
    public int killedCount = 0;
    public int deathCount = 0;
    bool isAliveBoss = true;
    readonly int penaltyScore = 5000;

    void Start()
    {
        totalScoreText.text = "Score " + totalScore.ToString();
        resultText.enabled = false;
    }

    /// <summary>
    ///プレイヤー or Boss の生存チェックメソッド
    /// </summary>
    public void CheckAlive(IStatus status)
    {
        //下記の二つのifはリスコフの置換原則に違反
        if (status is Player && !status.IsAlive)
        {
            deathCount++;
            return;
        }

        if (status is Boss && !status.IsAlive)
        {
            isAliveBoss = status.IsAlive;
        }
    }

    /// <summary>
    ///Scoreを計算するメソッド
    /// </summary>
    public void GetScore(IHaveScore score, IStatus status)
    {
        //2体以上まとめて倒すとスコアとカウントがバグったのでIsAliveで判定後にカウントする方式に変更
        if (!status.IsAlive)
        {
            totalScore += score.Score;
            killedCount++;
        }

        totalScoreText.text = "Score " + totalScore.ToString();
    }

    /// <summary>
    ///条件次第でクリア画面変更するメソッド
    /// </summary>
    public void EvaluateResultText()
    {
        totalScore = totalScore + Player.Instance.Hp * 100 - deathCount * penaltyScore;

        if (totalScore <= 0)
        {
            totalScore = 0;
        }

        resultText.enabled = true;

        var result = "結果";
        var score = "スコア:";
        var title = "称号:";
        var name = "";

        if (Player.Instance.Hp == 100 && deathCount == 0 && !isAliveBoss)
        {
            name = "暇神";
        }

        else if (!isAliveBoss)
        {
            name = "ボスキラー";
        }

        else if (killedCount > 15)
        {
            name = "雑魚狩りマスター！";
        }

        else
        {
            title = "";
            name = "I hope you enjoyed it!";
        }

        resultText.text =
            $"{result}" +
            $"\n{score}{totalScore}" +
            $"\n{title}{name}";

        Invoke(nameof(ReturnStartScene), 3f);
    }

    void ReturnStartScene() => SceneManager.LoadScene("Start Scene");

}
