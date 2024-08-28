using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : MonoBehaviour
{
    [SerializeField] EnemyPool enemyPool;
    [SerializeField] DetectObstacle detectObstacle;

    //Field
    [SerializeField] GameObject[] targets;
    int arrayNum = 16;
    readonly float interval = 5f;

    //Cache
    Transform transformCache;

    //Property
    /// <value>BombOrientationクラス、EstimateRangeクラス、およびLockOnCameraクラスへ</value>
    public GameObject SearchObj { get; private set; }

    private void Awake()
    {
        if (ReferenceEquals(enemyPool, null))
        {
            enemyPool = FindAnyObjectByType<EnemyPool>();
        }

        if (ReferenceEquals(detectObstacle, null))
        {
            detectObstacle = FindAnyObjectByType<DetectObstacle>();
        }
    }

    void Start()
    {
        transformCache = this.transform;
        targets = new GameObject[arrayNum];
    }

    void Update()
    {
        if (Time.frameCount % interval == 0f)
        {
            SearchforEnemies();
            //Shaker();
            detectObstacle.JudgeObstacle(this);
        }
    }

    /// <summary>
    ///最も近い敵を索敵
    /// </summary>
    void SearchforEnemies()
    {
        //直接書き込むのは冗長的になるのとinspectorで確認できるようにtargetsへ代入
        targets = enemyPool.Targets;

        //バブルソートで記述
        for (int i = 0; i < targets.Length; i++)
        {
            if (ReferenceEquals(targets[i], null)) return;

            for (int j = 0; j < targets.Length; j++)
            {
                var distanceI = Vector3.SqrMagnitude(transformCache.position - targets[i].transform.position);
                var distanceJ = Vector3.SqrMagnitude(transformCache.position - targets[j].transform.position);

                if (distanceI < distanceJ)
                {
                    //プレイヤーから最も近い敵をtarget[0]に寄せ、SearchObjに代入
                    (targets[j], targets[i]) = (targets[i], targets[j]);
                    SearchObj = targets[0];
                }
            }
        }
    }

    //void Shaker()
    //{
    //    targets = enemyPool.Targets;

    //    var left = 0;
    //    var right = arrayNum - 1;

    //    for (int i = left; i < right; i++)
    //    {
    //        var distanceI = Vector3.SqrMagnitude(transformCache.position - targets[i].transform.position);
    //        var distanceJ = Vector3.SqrMagnitude(transformCache.position - targets[i + 1].transform.position);

    //        if (distanceI < distanceJ)
    //        {
    //            //プレイヤーから最も近い敵をtarget[0]に寄せ、SearchObjに代入
    //            (targets[i + 1], targets[i]) = (targets[i], targets[i + 1]);
    //        }
    //    }

    //    for (int i = right; i > left; i--)
    //    {
    //        var distanceI = Vector3.SqrMagnitude(transformCache.position - targets[i].transform.position);
    //        var distanceJ = Vector3.SqrMagnitude(transformCache.position - targets[i - 1].transform.position);

    //        if (distanceI < distanceJ)
    //        {
    //            //プレイヤーから最も近い敵をtarget[0]に寄せ、SearchObjに代入
    //            (targets[i], targets[i - 1]) = (targets[i - 1], targets[i]);
    //        }
    //    }

    //    SearchObj = targets[0];
    //}
}
