using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 0;
    public int damage = 10;
    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;
    public Transform HitPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    public float camShakeAmt = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;

    SoundManager soundManager;

    float timeToFire = 0;
    Transform firePoint;
    Transform cloneBullet;

    // Use this for initialization
    void Awake () {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No FirePoint?");
        }
	}
    void Start()
    {
        soundManager = SoundManager.instance;
        if (soundManager == null)
            Debug.LogError("No SoundManager found");

        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null)
            Debug.LogError("No CameraShake script found on GM object");
    }

    // Update is called once per frame
    void Update () {
        if (fireRate == 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0) && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
	}
    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition-firePointPosition, 100, whatToHit);

        soundManager.PlaySound("GunshotSound");

        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100,Color.cyan);
        if(hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
                //Debug.Log("Hit " + hit.collider.name + " Damage: " + damage);
            }
            
        }
        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }
    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform cloneBullet = (Transform) Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        cloneBullet.parent = firePoint;
        LineRenderer lr = cloneBullet.GetComponent<LineRenderer>();

        if (lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);

        }
        Destroy(cloneBullet.gameObject, 0.04f);

        if (hitNormal != new Vector3(9999,9999,9999))
        {
            Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
        }

        Transform cloneMuzzleFlash = (Transform)Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation);
        cloneMuzzleFlash.parent = firePoint;

        float size = Random.Range(0.6f, 0.9f);
        cloneMuzzleFlash.localScale = new Vector3(size, size, 1);
        Destroy(cloneMuzzleFlash.gameObject,0.02f);

        camShake.Shake(camShakeAmt, camShakeLength);
    }
}
