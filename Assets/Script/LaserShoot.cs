using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LaserShoot : MonoBehaviour
{
    [Header("Laser")]
    public float distance = 20f;
    public GameObject laserPrefab;
    public Transform firePoint;

    [Header("UI")]
    public Image colorUI;
    public Image nextColorUI;
    public TextMeshProUGUI scoreText;

    public GameObject winPanel;        // YOU WIN
    public GameObject gameOverPanel;  // GAME OVER
    public GameObject restartButton;
    public GameObject nextSceneButton;

    [Header("Score")]
    public int score = 0;
    public int winScore = 300;

    // 👇 เพิ่มส่วนนี้เข้ามาสำหรับระบบเสียง
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    public BlockColor currentColor;
    public BlockColor nextColor;

    void Start()
    {
        Time.timeScale = 1f;

        // สุ่มสีเริ่มเกม
        currentColor = (BlockColor)Random.Range(0, 4);
        nextColor = (BlockColor)Random.Range(0, 4);

        UpdateColorUI();
        UpdateScoreUI();

        // ซ่อน UI ตอนเริ่ม
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (nextSceneButton != null) nextSceneButton.SetActive(false);
    }

    void Update()
    {
        // เช็กด้วยว่า Time.timeScale ไม่เป็น 0 (เกมไม่ถูก Pause หรือ Win ไปแล้ว) ถึงจะยิงได้
        if (Input.GetMouseButtonDown(0) && Time.timeScale > 0f)
        {
            ShootLaser();
        }
    }

    void ShootLaser()
    {
        // 👇 สั่งให้เล่นเสียงยิงเลเซอร์
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        BlockColor shootColor = currentColor;

        // เปลี่ยนสีถัดไป
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
                // ยิงโดนสีตรงกัน
                if (block.blockColor == shootColor)
                {
                    score += GetScoreByColor(block.blockColor);
                    UpdateScoreUI();

                    Destroy(hit.collider.gameObject);

                    if (score >= winScore)
                    {
                        WinGame();
                    }
                }
            }
        }

        // สร้างเลเซอร์
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        SpriteRenderer sr = laser.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = GetColor(shootColor);

        laser.transform.localScale = new Vector3(0.1f, length, 1f);
        laser.transform.position = firePoint.position + (Vector3)(dir * length / 2f);

        Destroy(laser, 0.1f);
    }

    // ======================
    // ชนะ
    // ======================
    void WinGame()
    {
        if (winPanel != null) winPanel.SetActive(true);
        if (restartButton != null) restartButton.SetActive(true);
        if (nextSceneButton != null) nextSceneButton.SetActive(true);

        Time.timeScale = 0f;
    }

    // ======================
    // ปุ่ม Restart
    // ======================
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ======================
    // ปุ่มไปซีนต่อไป
    // ======================
    public void NextScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Ending Scene");
    }

    // ======================
    // UI คะแนน
    // ======================
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score : " + score;
    }

    // ======================
    // UI สี
    // ======================
    void UpdateColorUI()
    {
        if (colorUI != null)
            colorUI.color = GetColor(currentColor);

        if (nextColorUI != null)
            nextColorUI.color = GetColor(nextColor);
    }

    // ======================
    // สีจริง
    // ======================
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

    // ======================
    // คะแนนแต่ละสี
    // ======================
    int GetScoreByColor(BlockColor color)
    {
        switch (color)
        {
            case BlockColor.Red: return 10;
            case BlockColor.Green: return 20;
            case BlockColor.Blue: return 30;
            case BlockColor.Yellow: return 40;
        }

        return 0;
    }
}