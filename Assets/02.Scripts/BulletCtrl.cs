using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // ÃÑ¾Ë ÆÄ±«·Â
    public int damage;

    // ÃÑ¾Ë ¹ß»ç Èû
    public float force = 1500.0f;

    public float deathTime;

    private Rigidbody bulletRigidbody = null;
    private Transform bulletTransform = null;
    void Start()
    {
        Destroy(gameObject, deathTime);
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletTransform = GetComponent<Transform>();
        damage = FireCtrl.Instance.CUR_DAMAGE;
        // ÃÑ¾ËÀÇ ÀüÁø¹æÇâÀ¸·Î ÈûÀ» °¡ÇÑ´Ù.
        bulletRigidbody.AddForce(bulletTransform.forward * force);
    }
}
