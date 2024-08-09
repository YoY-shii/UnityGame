using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour, IDamageable, IStatus, IHaveScore
{
    [SerializeField] ScoreManager scoreManager;

    //Status
    [SerializeField] int maxHp;
    int damage = 5;

    //Cache
    Transform transformCamCache;
    Transform transformCache;
    NavMeshAgent nav;

    //Property
    public int Hp { get; private set; }

    public bool IsAlive { get; private set; }

    public int Score { get; private set; }

    private void Awake()
    {
        TryGetComponent(out nav);
    }

    void Start()
    {
        transformCache = this.transform;
        transformCamCache = Camera.main.transform;
        Hp = maxHp;
        IsAlive = true;
        Score = 5000;
        nav.isStopped = false;
    }

    private void LateUpdate()
    {
        transformCache.LookAt(transformCamCache);
    }

    //以下ダメージ処理
    void IDamageable.Damage(int damage)
    {
        Hp -= damage;

        if (Hp <= 0)
        {
            Hp = 0;
            IsAlive = false;
            nav.isStopped = true;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var iInjurer = collision.gameObject.GetComponent<IInjurer>();

        if (iInjurer != null)
        {
            iInjurer.Damage(damage);
        }
    }

    private void OnDestroy()
    {
        scoreManager.GetScore(this,this);
        scoreManager.CheckAlive(this);
    }
}