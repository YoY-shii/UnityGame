using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player.Instance ←検索用
public class ReferenceDistance : MonoBehaviour
{
    [SerializeField] Transform transformCache;
    [SerializeField] Transform transformPlayerCache;

    void Awake()
    {
        transformCache = this.transform;
        transformPlayerCache = Player.Instance.transform;
    }

    public float CalcDistance()
    {
        return Vector3.SqrMagnitude(transformCache.position - transformPlayerCache.position);
    }
}
