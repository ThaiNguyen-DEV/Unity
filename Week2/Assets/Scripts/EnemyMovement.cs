using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;      
    [SerializeField] private Transform leftPoint;       
    [SerializeField] private Transform rightPoint;       

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        Move();
        CheckDirection();
    }

    private void Move()
    {
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    private void CheckDirection()
    {
        if (movingRight && transform.position.x >= rightPoint.position.x)
        {
            movingRight = false;
            Flip();
        }
        else if (!movingRight && transform.position.x <= leftPoint.position.x)
        {
            movingRight = true;
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
