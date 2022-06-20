using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float turnSpeed = 4.0f; // 마우스 회전 속도
    private float xRotate = 0.0f;

    [SerializeField]
    private Transform camView;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float boost;

    private readonly float initHP = 100.0f;
    public float currHp;

    private Image hpBar;
    private bool _isDashing = false;
    private Rigidbody rigid;
    IEnumerator Start()
    {
        Cursor.visible = false;
        turnSpeed = 0.0f;
        boost = 0;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 4.0f;

        currHp = initHP;

        hpBar = GameObject.FindGameObjectWithTag("HPBAR")?.GetComponent<Image>();
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        moveDir.Normalize();

        if (!_isDashing)
        {
            transform.Translate(moveDir * (moveSpeed+boost) * Time.deltaTime);
        }

        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;

        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -50, 80);

        transform.eulerAngles = new Vector3(0, yRotate, 0);
        camView.eulerAngles = new Vector3(xRotate, yRotate, 0);

        if (Input.GetKey(KeyCode.Space))
        {
            if(Physics.Raycast(transform.position, Vector3.down, 0.05f))
                rigid.velocity = new Vector3(rigid.velocity.x, jumpPower, rigid.velocity.x);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            boost = moveSpeed;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            boost = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PUNCH")&&currHp>=0f)
        {
            currHp -= 5f;
            Debug.Log($"Player HP = {currHp}");
            DisplayHP();
            if(currHp<=0f)
            {
                PlayerDie();
            }
        }
    }

    private void PlayerDie()
    {
        Debug.Log("DIE");

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        foreach(GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
        GameManger.Instance().IsOver = true;
    }

    private void DisplayHP()
    {
        hpBar.fillAmount = currHp / initHP;
    }
}
