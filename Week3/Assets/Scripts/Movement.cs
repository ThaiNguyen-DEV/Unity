using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current.aKey.isPressed) input.x = -1;
        if (Keyboard.current.dKey.isPressed) input.x = 1;

        if (Keyboard.current.wKey.isPressed) input.y = 1;
        if (Keyboard.current.sKey.isPressed) input.y = -1;

        rb.linearVelocity = new Vector2(input.x * moveSpeed, rb.linearVelocity.y);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
