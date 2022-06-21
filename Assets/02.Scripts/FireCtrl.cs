using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    private static FireCtrl instance;
    public static FireCtrl Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    public Transform camView;
    public float soundMix;
    // 총알 프리팹
    public GameObject bulletPrefab;

    // 총알 발사 좌표
    public Transform firePos;

    // 총소리 오디오 클립
    public AudioClip fireSound;
    public AudioClip startReloadSound;
    public AudioClip endReloadSound;
    
    [SerializeField]
    private int max_bullet;
    [SerializeField]
    private int current_bullet;
    [SerializeField]
    private int max_magazine;
    [SerializeField]
    private int current_magazine;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private TextMeshProUGUI bulletText;
    [SerializeField]
    private TextMeshProUGUI damageText;
    [SerializeField]
    private Image fill;
    [SerializeField]
    private int current_damage = 10;

    private bool _isDead;
    public bool IsDead
    {
        get { return _isDead; }
        set { _isDead = value; }
    }

    public int CUR_DAMAGE
    {
        get { return current_damage; }
    }

    private new AudioSource audio;
    private bool _isReloading=false;
    // Muzzleflash의 메쉬렌더러
    private MeshRenderer muzzleFlash;

    private float cool = 0;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;
        current_bullet = max_bullet;
        current_magazine = max_magazine;
        bulletText.SetText($"{current_bullet} / {current_magazine}");
        fill.fillAmount = 0;
    }

    void Update()
    {
        if (_isDead) return;
        // 마우스 왼쪽 버튼 클릭 했을 때, 
        if( Input.GetMouseButton(0) && cool<=0 && current_magazine>0&&!_isReloading)
        {
            Fire();
            StartCoroutine(ShowMuzzleFlash());
            cool = 0.1f;
        }
        if (Input.GetKey(KeyCode.R) && !_isReloading && current_magazine != max_magazine && current_bullet>0)
        {
            StartCoroutine(Reload());
        }
        cool -= Time.deltaTime;
        bulletText.SetText($"{current_magazine} / {current_bullet}");
        damageText.SetText($"Damage : {current_damage}");
        if(fill.fillAmount>=0)
        {
            fill.fillAmount -= Time.deltaTime / reloadTime;
        }
    }
    IEnumerator Reload()
    {
        fill.fillAmount = 1;
        audio.pitch = 1;
        audio.PlayOneShot(startReloadSound, 1f);
        _isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        audio.PlayOneShot(endReloadSound, 1f);
        if (current_bullet+ current_bullet + current_magazine >= 30)
        {
            current_bullet += (current_magazine - max_magazine);
            current_magazine = max_magazine;
        }
        else
        {
            current_magazine += current_bullet;
            current_bullet = 0;
        }    
        yield return new WaitForSeconds(0.65f);
        _isReloading = false;
    }

    void Fire()
    {
        // 프리팹을 인스턴스화하여 생성
        Instantiate(bulletPrefab, firePos.position, camView.rotation);
        soundMix = Random.Range(0.8f, 1.2f);
        audio.pitch = soundMix;
        audio.PlayOneShot(fireSound, 1.0f);
        current_magazine--;
    }

    IEnumerator ShowMuzzleFlash()
    {
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;

        muzzleFlash.material.mainTextureOffset = offset;

        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(Vector3.forward * angle);

        float scale = Random.Range(0.25f, 0.5f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.enabled = false;
    }

    public void ChangeBulletAndDamage(int curBullet, int damage)
    {
        current_bullet += curBullet;
        current_damage += damage;
    }
}
