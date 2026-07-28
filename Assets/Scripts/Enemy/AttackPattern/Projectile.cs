using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float damage;
    [SerializeField]private float lifeTime = 5f;

    void Update()
    {
        Flying();
    }
    public void InitObj(Vector3 direction, float speed, float damage)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
    }
    void Flying()
    {
        transform.position = transform.position + direction * speed * Time.deltaTime;

        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerController player= other.GetComponent<PlayerController>();
        if(player != null && !player.IsDead)
        {
            player.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

}
