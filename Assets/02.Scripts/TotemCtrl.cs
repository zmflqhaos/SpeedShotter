using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemCtrl : MonoBehaviour
{
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
            curHp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            if (curHp <= 0)
            {
                CancelInvoke("CreateMonster");
                GameManger.Instance().DisplayScore(4999);
                gameObject.SetActive(false);
            }
        }
    }
}
