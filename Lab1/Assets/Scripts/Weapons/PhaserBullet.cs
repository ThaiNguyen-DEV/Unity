using UnityEngine;

public class PhaserBullet : MonoBehaviour
{

    private void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), PlayerController.Instance.GetComponent<Collider2D>());
    }
    void Update()
    {
        transform.position += new Vector3(0f, PhaserWeapon.Instance.speed * Time.deltaTime);
        if(transform.position.y > 5)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Critter") || collision.gameObject.CompareTag("Boss"))
        {
            gameObject.SetActive(false);
        }
    }
}
