using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner Instance;

    [SerializeField]
    private Bullet bulletPrefab;

    private List<Bullet> bullets = new List<Bullet>();

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnBullet(Vector3 position, Vector3 direction, float deltaTime, bool isEnemy)
    {
        Bullet bullet = Instantiate(bulletPrefab, position + direction * Bullet.Speed * deltaTime, Quaternion.identity);
        bullet.Initialize(direction, isEnemy);
        bullets.Add(bullet);
    }

    public void RemoveBullet(Bullet bullet)
    {
        bullets.Remove(bullet);
    }

    public void DestroyAllBullets()
    {
        foreach (var bullet in bullets)
        {
            bullet.DestroyBullet(true);
        }
        bullets.Clear();
    }
}
