using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Rigidbody2D rb;
    private Animator animator;
    private FlashWhite flashWhite;
    private Vector2 playerDirection;

    [SerializeField] private float moveSpeed = 5f;

    public bool boosting = false;

    [SerializeField] private float energy;
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float energyRegen = 0.1f;

    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private ParticleSystem engineEffect;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        flashWhite = GetComponent<FlashWhite>();
        energy = maxEnergy;

        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
        health = maxHealth;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
    }

    void Update()
    {
        if (Keyboard.current != null && Time.timeScale > 0)
        {
            float directionX = 0f;
            float directionY = 0f;

            if (Keyboard.current.aKey.isPressed) directionX = -1f;
            if (Keyboard.current.dKey.isPressed) directionX = 1f;
            if (Keyboard.current.wKey.isPressed) directionY = 1f;
            if (Keyboard.current.sKey.isPressed) directionY = -1f;

            playerDirection = new Vector2(directionX, directionY).normalized;

            animator.SetFloat("moveX", directionX);
            animator.SetFloat("moveY", directionY);

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                EnterBoost();
            else if (Keyboard.current.spaceKey.wasReleasedThisFrame)
                ExitBoost();

            if (Keyboard.current.jKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
                PhaserWeapon.Instance.Shoot();
        }

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed, playerDirection.y * moveSpeed);

        if (boosting)
        {
            if (energy >= 0.5f)
                energy -= 0.5f;
            else
                ExitBoost();
        }
        else if (energy < maxEnergy)
        {
            energy += energyRegen;
        }

        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
    }

    void EnterBoost()
    {
        if (energy > 10)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.boost);
            animator.SetBool("boosting", true);
            GameManager.Instance.SetWorldSpeed(7f);
            boosting = true;
            engineEffect.Play();
        }
    }

    public void ExitBoost()
    {
        animator.SetBool("boosting", false);
        GameManager.Instance.SetWorldSpeed(1f);
        boosting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            TakeDamage(1);
        else if(collision.gameObject.CompareTag("Boss"))
        {
            TakeDamage(5);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        flashWhite.Flash();

        if (health > 0)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.atack);
        }
        else
        {
            ExitBoost();
            GameManager.Instance.SetWorldSpeed(0f);
            gameObject.SetActive(false);
            Instantiate(destroyEffect, transform.position, transform.rotation);
            GameManager.Instance.GameOver();
            AudioManager.Instance.PlaySound(AudioManager.Instance.earth);
        } 
    }

}
