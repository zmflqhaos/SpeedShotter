using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject sparkEffect;
    [SerializeField]
    private int MAX_HP;
    private int curHp;
    void Start()
    {
        curHp = MAX_HP;
        InvokeRepeating("CreateMonster", 2.0f, GameManger.Instance().createTime);
    }

    void CreateMonster()
    {
        GameObject _monster = GameManger.Instance().GetMonsterInPool();

        _monster?.transform.SetPositionAndRotation(transform.position + transform.forward * 1, transform.rotation);
        _monster?.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            Destroy(collision.gameObject);
            curHp -= FireCtrl.Instance.CUR_DAMAGE;
            ContactPoint cp = collision.GetContact(0);
            Quaternion rotSpark = Quaternion.LookRotation(-cp.normal);
            GameObject spark = Instantiate(sparkEffect, cp.point, rotSpark);
            Destroy(spark, 0.5f);

            Destroy(collision.gameObject);
            if (curHp <= 0)
            {
                CancelInvoke("CreateMonster");

                GameManger.Instance().DisplayScore(4999);
                gameObject.SetActive(false);
            }
        }
    }
}
