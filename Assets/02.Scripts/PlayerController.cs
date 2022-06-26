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
    private GameObject stopPanel;
    [SerializeField]
    private GameObject gun;

    private readonly float initHP = 100.0f;
    public float currHp;

    private Image hpBar;
    private Rigidbody rigid;
    IEnumerator Start()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 4.0f;
        currHp = initHP;

        hpBar = GameObject.FindGameObjectWithTag("HPBAR")?.GetComponent<Image>();
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (GameManger.Instance().IsOver) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        moveDir.Normalize();

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;

        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -50, 80);

        if(Time.timeScale==1)
        {
            transform.eulerAngles = new Vector3(0, yRotate, 0);
            camView.eulerAngles = new Vector3(xRotate, yRotate, 0);

            if (Input.GetKey(KeyCode.Space))
            {
                if (Physics.Raycast(transform.position, Vector3.down, 0.1f))
                    rigid.velocity = new Vector3(rigid.velocity.x, jumpPower, rigid.velocity.x);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        stopPanel.SetActive(!stopPanel.activeSelf);
        Cursor.visible = stopPanel.activeSelf;
        if(stopPanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PUNCH")&&currHp>=0f)
        {
            currHp -= 10f;
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
        GameManger.Instance().IsOver = true;
        GameManger.Instance().isClear = false;
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
