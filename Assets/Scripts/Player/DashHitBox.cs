using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHitBox : MonoBehaviour
{
    private float damage;
    private HashSet<Enemy> hitEnemies = new HashSet<Enemy>();

    public void Activate(float damage)
    {
        this.damage = damage;
        hitEnemies.Clear();
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy) && !hitEnemies.Contains(enemy))
        {
            enemy.TakeDamage(damage);
            hitEnemies.Add(enemy);
        }
    }
}
