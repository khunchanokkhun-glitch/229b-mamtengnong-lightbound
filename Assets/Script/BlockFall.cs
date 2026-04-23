using UnityEngine;

public class BlockFall : MonoBehaviour
{
    public float fallSpeed = 3f;   // ความเร็วตก

    void Update()
    {
        // ⬇️ เลื่อนลง
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    // 💀 ชนจรวด = แพ้
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GAME OVER");

           
        }
    }
}