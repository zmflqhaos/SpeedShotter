using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // 총알 발사 힘
    public float force = 1500.0f;

    public float deathTime;

    private Rigidbody bulletRigidbody = null;
    private Transform bulletTransform = null;
    void Start()
    {
        Destroy(gameObject, deathTime);
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletTransform = GetComponent<Transform>();
        // 총알의 전진방향으로 힘을 가한다.
        bulletRigidbody.AddForce(bulletTransform.forward * force);
    }
}
