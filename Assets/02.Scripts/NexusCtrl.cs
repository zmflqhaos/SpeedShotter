using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject sparkEffect;
    [SerializeField]
    private int MAX_HP;
    [SerializeField]
    private Material invincibleMat;
    [SerializeField]
    private Material deinvincibleMat;
    private int curHp;
    private bool isInvincible = true;
    private MeshRenderer thisMesh;
    void Start()
    {
        thisMesh = GetComponent<MeshRenderer>();
        curHp = MAX_HP;
        thisMesh.material = invincibleMat;
    }

    private void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            isInvincible = false;
            thisMesh.material = deinvincibleMat;
        }
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
