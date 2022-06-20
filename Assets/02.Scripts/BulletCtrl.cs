using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // �Ѿ� �ı���
    public int damage;

    // �Ѿ� �߻� ��
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
        // �Ѿ��� ������������ ���� ���Ѵ�.
        bulletRigidbody.AddForce(bulletTransform.forward * force);
    }
}
