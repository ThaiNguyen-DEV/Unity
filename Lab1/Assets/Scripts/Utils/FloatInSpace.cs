using UnityEngine;

public class FloatÍnpace : MonoBehaviour
{
    void Update()
    {
        float moveY = GameManager.Instance.worldSpeed * Time.deltaTime;
        transform.position += new Vector3(0, -moveY);
    }
}
