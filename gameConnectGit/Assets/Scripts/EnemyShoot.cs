using System.Collections;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public float bulletSpeed;

    public float startTimeShotsAfterEvent;
    public float startTimeBtwShots;
    private float timeBtwShots;
    private bool isShooting;

    public GameObject bullet;
    public GameObject bulletSpot;
    private EnemyFollow enemyFollow;
    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        enemyFollow = gameObject.GetComponent<EnemyFollow>();
        timeBtwShots = startTimeShotsAfterEvent;
    }

    // Update is called once per frame
    void Update()
    {
        isShooting = false;

        // Sau khi Follow player, nghi mot thoi gian roi moi ban dan
        if (enemyFollow.IsFollow() || enemy.IsDamaged())
        {
            timeBtwShots = startTimeShotsAfterEvent;
        }

        // Sau mot khoan startTimeBtwShots thi ban dan
        if (timeBtwShots <= 0 && !enemyFollow.IsFollow())
        {
            isShooting = true;
            StartCoroutine(SpawnBullet());
            timeBtwShots = startTimeBtwShots;
        } else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    // Cho mot khoan thoi gian roi moi spawn bullet
    IEnumerator SpawnBullet()
    {
        yield return new WaitForSeconds(0.2f);
        // Dieu chinh dan quay theo huong cua chu so huu
        bullet.transform.localScale = transform.localScale;
        GameObject obj = Instantiate(bullet, bulletSpot.transform.position, Quaternion.identity);
        // Set gia tri speed va damage cho dan, gui kem GameObject nguoi ban dan
        obj.GetComponent<BulletMove>().SetBulletInfo(bulletSpeed, enemy.damage, gameObject);
    }

    public bool IsShooting()
    {
        return isShooting;
    }
}
