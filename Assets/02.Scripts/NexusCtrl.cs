using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject sparkEffect;
    [SerializeField]
    private int MAX_HP;
    private int curHp;
    private bool isInvincible = true;
    void Start()
    {
        curHp = MAX_HP;
    }

    private void Update()
    {
        if (gameObject.transform.childCount == 0)
            isInvincible = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET")&&!isInvincible)
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
                GameManger.Instance().IsOver = true;
                GameManger.Instance().isClear = true;
                GameManger.Instance().DisplayScore(9999);
                gameObject.SetActive(false);
            }
        }
    }
}
