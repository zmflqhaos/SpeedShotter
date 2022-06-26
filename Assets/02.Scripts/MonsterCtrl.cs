using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE,
        PLAYERDIE
    }
    public GameObject item;
    public State state = State.IDLE;
    public float traceDis = 10f;
    public float attackDis = 2f;
    public bool isDead = false;
    public SphereCollider hitCol;

    private Transform monsterTrs;
    private Transform playerTrs;
    private NavMeshAgent agent;
    private Animator anim;

    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    [SerializeField]
    private int initHp = 100;
    private int currHp;
    private GameObject bloodEffect;

    void OnEnable()
    {
        currHp = initHp;
        state = State.IDLE;
        isDead = false;
        monsterTrs = transform;
        playerTrs = GameObject.FindWithTag("PLAYER").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");

        GetComponent<CapsuleCollider>().enabled = true;
        SphereCollider[] components = GetComponentsInChildren<SphereCollider>();
        foreach (SphereCollider com in components)
        {
            com.enabled = true;
        }
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    private IEnumerator CheckMonsterState()
    {
        while(!isDead)
        {
            if (GameManger.Instance().IsOver)
                state = State.DIE;
            yield return new WaitForSeconds(0.3f);

            if (state == State.DIE)
                yield break;

            if (state == State.PLAYERDIE)
                yield break;
            float distance = Vector3.Distance(monsterTrs.position, playerTrs.position);

            if(distance<=attackDis)
            {
                state = State.ATTACK;
            }
            else if(distance<=traceDis)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }
    private IEnumerator MonsterAction()
    {
        while(!isDead)
        {
            switch(state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    anim.SetBool(hashTrace, false);
                    break;
                case State.TRACE:
                    agent.SetDestination(playerTrs.position);
                    agent.isStopped = false;
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    break;
                case State.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;
                case State.DIE:
                    isDead = true;
                    agent.isStopped = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                    anim.SetTrigger(hashDie);
                    hitCol.enabled = false;
                    SphereCollider[] components = GetComponentsInChildren<SphereCollider>();
                    foreach(SphereCollider com in components)
                    {
                        com.enabled = false;
                    }
                    yield return new WaitForSeconds(0.45f);
                    GiveItem();
                    yield return new WaitForSeconds(0.45f);
                    gameObject.SetActive(false);
                    break;
                case State.PLAYERDIE:
                    StopAllCoroutines();

                    agent.isStopped = true;
                    anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.3f));
                    anim.SetTrigger(hashPlayerDie);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("BULLET"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(HitStop());
            anim.SetTrigger(hashHit);

            Vector3 pos = collision.GetContact(0).point;
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);

            ShowBloodEffect(pos, rot);

            currHp -= FireCtrl.Instance.CUR_DAMAGE;
            if(currHp<=0)
            {
                state = State.DIE;
                GameManger.Instance().DisplayScore(997);
            }
        }
    }

    public void SendMessage(string hesh, SendMessageOptions send)
    {
        OnPlayerDie();
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTrs);
        Destroy(blood, 1.0f);
    }

    private void OnPlayerDie()
    {
        state = State.PLAYERDIE;
    }

    private void OnDrawGizmos()
    {
        if(state==State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(monsterTrs.position, traceDis);
        }
        if(state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(monsterTrs.position, attackDis);
        }
    }

    private void GiveItem()
    {
        float rand = Random.Range(0, 100f);    
        if(rand>70)
        {
            GameObject _item = Instantiate(item, transform);
            _item.transform.SetParent(null);
        }
    }

    private IEnumerator HitStop()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(0.2f);
        agent.isStopped = false;
    }
}
