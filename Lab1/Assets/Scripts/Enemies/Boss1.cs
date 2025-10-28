using NUnit.Compatibility;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    private float speedX;
    private float speedY;
    private bool charging;
    private float switchInterval;
    private float switchTimer;
    private int lives;
    private FlashWhite flashWhite;

    [SerializeField] private GameObject destroyEffect;

    private Animator animator;
    void Start()
    {
        lives = 20;
        animator = GetComponent<Animator>();
        EnterChargeState();
        flashWhite = GetComponent<FlashWhite>();

    }

    void Update()
    {
        float playerPosition = PlayerController.Instance.transform.position.y;
        if (switchTimer > 0)
        {
            switchTimer -= Time.deltaTime;
        }
        else { 
            if(charging && transform.position.y > playerPosition)
            {
                EnterPatrolState();
            }
            else
            {
                EnterChargeState();
            }
        }

        if (transform.position.x > 2.5 || transform.position.x < -2.5)
        {
            speedX *= -1;
        }else if (transform.position.y < playerPosition)
        {
            EnterChargeState();
        }

        bool boost = PlayerController.Instance.boosting;
        float moveY;
        if(boost && !charging)
        {
            moveY = GameManager.Instance.worldSpeed * Time.deltaTime * -0.5f;
        }
        else
        {
            moveY = speedY * Time.deltaTime;
        }
        float moveX = speedX * Time.deltaTime;
        transform.position += new Vector3(moveX, moveY);
        if (transform.position.y < -7)
        {
            Destroy(gameObject);
        }
    }

    void EnterPatrolState() {
        speedY = 0;
        speedX = Random.Range(-2f, 2f);
        switchInterval = Random.Range(5f, 10f);
        switchTimer = switchInterval;
        charging = false;
        animator.SetBool("charging", false);
    }

    void EnterChargeState()
    {
        if (!charging)
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.charge);
        speedY = -1f;
        speedX = 0;
        switchInterval = Random.Range(0.6f, 1.5f);
        switchTimer = switchInterval;
        charging = true;
        animator.SetBool("charging", true);
        
    }

    public void TakeDamage(int damage) {
        //AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.hit);
        lives -= damage;
        flashWhite.Flash();
        if (lives <= 0)
        {
            Instantiate(destroyEffect, transform.position, transform.rotation);
            //AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.boom);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
        }
    }
}
