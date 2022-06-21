using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private Material bullet;
    [SerializeField]
    private Material atk;
    private MeshRenderer[] thismat;

    private bool _IsBullet;
    void Start()
    {
        thismat = GetComponentsInChildren<MeshRenderer>();
        _IsBullet = (Random.value > 0.5f);
        if(_IsBullet)
        {
            thismat[0].material = bullet;
            thismat[1].material = bullet;
        }   
        else
        {
            thismat[0].material = atk;
            thismat[1].material = atk;
        }
    }

    private void Update()
    {
        transform.Rotate(0, 0.8f, 0);    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("PLAYER"))
        {
            if(_IsBullet)
            {
                FireCtrl.Instance.ChangeBulletAndDamage(200, -2);
            }
            else
            {
                FireCtrl.Instance.ChangeBulletAndDamage(-200, 2);
            }
            Destroy(gameObject);
        }
    }
}
