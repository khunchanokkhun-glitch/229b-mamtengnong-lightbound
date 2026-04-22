using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    public float distance = 20f;
    public GameObject laserPrefab;
    public Transform firePoint;

    public BlockColor currentColor; // สีที่ยิง

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootLaser();
        }
    }

    void ShootLaser()
    {
        //  สุ่ม 4 สี
        currentColor = (BlockColor)Random.Range(0, 4);

        Vector2 dir = firePoint.up;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir, distance);

        float length = distance;

        if (hit.collider != null)
        {
            length = hit.distance;

            Block block = hit.collider.GetComponent<Block>();

            if (block != null)
            {
                if (block.blockColor == currentColor)
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        //  สร้างเลเซอร์
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        //  เปลี่ยนสีเลเซอร์
        SpriteRenderer sr = laser.GetComponent<SpriteRenderer>();

        switch (currentColor)
        {
            case BlockColor.Red:
                sr.color = Color.red;
                break;
            case BlockColor.Green:
                sr.color = Color.green;
                break;
            case BlockColor.Blue:
                sr.color = Color.blue;
                break;
            case BlockColor.Yellow:
                sr.color = Color.yellow;
                break;
        }

        //  ปรับความยาวเลเซอร์
        laser.transform.localScale = new Vector3(0.1f, length, 1f);

        //  ขยับให้เริ่มจากปากยาน
        laser.transform.position = firePoint.position + (Vector3)(dir * length / 2f);

        //  ลบเลเซอร์
        Destroy(laser, 0.1f);

        // debug
        Debug.DrawRay(firePoint.position, dir * length, Color.white, 0.2f);
    }
}