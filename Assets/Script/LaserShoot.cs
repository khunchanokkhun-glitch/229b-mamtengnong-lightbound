using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LaserShoot : MonoBehaviour
{
    public float distance = 20f;
    public GameObject laserPrefab;
    public Transform firePoint;
    public GameObject winUI;

    public TextMeshProUGUI scoreText;
    public int score = 0;

    [Header("UI")]
    public Image colorUI;
    public Image nextColorUI;

    public BlockColor currentColor;
    public BlockColor nextColor;

    void Start()
    {
        currentColor = (BlockColor)Random.Range(0, 4);
        nextColor = (BlockColor)Random.Range(0, 4);

        UpdateColorUI();
        UpdateScoreUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootLaser();
        }
    }

    void ShootLaser()
    {
        BlockColor shootColor = currentColor;

        // เปลี่ยนสี
        currentColor = nextColor;
        nextColor = (BlockColor)Random.Range(0, 4);
        UpdateColorUI();

        Vector2 dir = firePoint.up;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir, distance);

        float length = distance;

        if (hit.collider != null)
        {
            length = hit.distance;

            Block block = hit.collider.GetComponent<Block>();

            if (block != null)
            {
                if (block.blockColor == shootColor)
                {
                    Destroy(hit.collider.gameObject);

                    // ⭐ คะแนนตามสี
                    int gained = GetScore(shootColor);
                    score += gained;

                    Debug.Log("+" + gained + " คะแนน");

                    UpdateScoreUI();
                    if (score >= 200)
                    {
                        winUI.SetActive(true);
                    }
                }
            }
        }
       
        // ยิงเลเซอร์
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        SpriteRenderer sr = laser.GetComponent<SpriteRenderer>();
        sr.color = GetColor(shootColor);

        laser.transform.localScale = new Vector3(0.1f, length, 1f);
        laser.transform.position = firePoint.position + (Vector3)(dir * length / 2f);

        Destroy(laser, 0.1f);

        Debug.DrawRay(firePoint.position, dir * length, Color.white, 0.2f);
    }

    //  คะแนนแต่ละสี
    int GetScore(BlockColor color)
    {
        switch (color)
        {
            case BlockColor.Red: return 10;
            case BlockColor.Green: return 20;
            case BlockColor.Blue: return 30;
            case BlockColor.Yellow: return 50;
        }
        return 0;
    }

    //  สีเลเซอร์
    Color GetColor(BlockColor color)
    {
        switch (color)
        {
            case BlockColor.Red: return Color.red;
            case BlockColor.Green: return Color.green;
            case BlockColor.Blue: return Color.blue;
            case BlockColor.Yellow: return Color.yellow;
        }
        return Color.white;
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateColorUI()
    {
        colorUI.color = GetColor(currentColor);
        nextColorUI.color = GetColor(nextColor);
    }
}