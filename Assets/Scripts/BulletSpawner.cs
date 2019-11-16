using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner Instance;

    [SerializeField]
    private Bullet bulletPrefab;

    private List<GameObject> bullets = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnBullet(Vector3 position, Vector3 direction, float deltaTime, bool isEnemy)
    {
        Bullet bullet = Instantiate(bulletPrefab, position + direction * Bullet.Speed * deltaTime, Quaternion.identity);
        bullet.Initialize(direction, isEnemy);
        bullets.Add(bullet.gameObject);
    }

    public void RemoveBullet(GameObject bullet)
    {
        bullets.Remove(bullet);
    }

    public void DestroyAllBullets()
    {
        foreach (var bullet in bullets)
        {
            Destroy(bullet);
        }
        bullets.Clear();
    }
}
