using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private float backgroundImageHeight;

    void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        backgroundImageHeight = (sprite.texture.height / sprite.pixelsPerUnit) * transform.localScale.y;
    }

    void Update()
    {
        float moveY = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(0, moveY);

        if (Mathf.Abs(transform.position.y) >= backgroundImageHeight)
        {
            transform.position = new Vector3(transform.position.x, 0f);
        }
    }
}
