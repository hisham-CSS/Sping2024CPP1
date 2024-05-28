using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float lifetime;

    //speed value is set by shoot script when the player fires
    [HideInInspector]
    public float xVel;
    [HideInInspector]
    public float yVel;

    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0) lifetime = 2.0f;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, yVel);
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy") && CompareTag("PlayerProjectile"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(10);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player") && CompareTag("EnemyProjectile"))
        {
            GameManager.Instance.lives--;
            Destroy(gameObject);
        }
    }
}
