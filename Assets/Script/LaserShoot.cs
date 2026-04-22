using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    public float distance = 20f;
    public GameObject laserPrefab;
    public Transform firePoint;

    public BlockColor currentColor; // สีที่กำลังยิง

    void Update()
    {
        // คลิกยิง
        if (Input.GetMouseButtonDown(0))
        {
            ShootLaser();
        }

        // 🔥 (ออปชัน) กดเปลี่ยนสี
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentColor = BlockColor.Red;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentColor = BlockColor.Green;
        }
    }

    void ShootLaser()
    {
        Vector2 dir = firePoint.up;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir, distance);

        float length = distance;

        if (hit.collider != null)
        {
            length = hit.distance;

            Block block = hit.collider.GetComponent<Block>();

            if (block != null)
            {
                // ✅ เช็คสีตรงกันเท่านั้น
                if (block.blockColor == currentColor)
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        // สร้างเลเซอร์
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        // ปรับความยาว
        laser.transform.localScale = new Vector3(0.1f, length, 1f);

        // ขยับให้เริ่มจากปากยาน
        laser.transform.position = firePoint.position + (Vector3)(dir * length / 2f);

        // ลบเลเซอร์
        Destroy(laser, 0.1f);

        // debug เส้นยิง
        Debug.DrawRay(firePoint.position, dir * length, Color.red, 0.2f);
    }
}