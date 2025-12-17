using UnityEngine;
using TMPro; // Wajib untuk Text
using System.Collections;
using System.Collections.Generic;

public class BowlingGameManager : MonoBehaviour
{
    [Header("Referensi")]
    public Pin[] pins;             // Masukkan 10 Pin ke sini
    public Transform ballObject;   // Masukkan Bola Bowling
    public Transform ballSpawnPos; // Posisi awal bola (tangan/holder)
    public TextMeshProUGUI scoreText; // Text Scoreboard

    [Header("Status Game")]
    public int currentFrame = 1;
    public int currentThrow = 1;   // Lemparan ke-1 atau ke-2
    private int[] rolls = new int[21]; // Menyimpan riwayat poin per lemparan (maks 21 kali)
    private int currentRollIndex = 0;
    
    private bool isGameRunning = false;
    private bool ballInGutter = false; // Flag bola masuk selokan

    void Start()
    {
        ResetGame();
    }

    public void StartTurn()
    {
        if(!isGameRunning) return;
        StartCoroutine(ProcessTurn());
    }

    IEnumerator ProcessTurn()
    {
        // 1. Tunggu 7 detik sampai bola nabrak & pin BENAR-BENAR selesai jatuh
        yield return new WaitForSeconds(7f);

        // 2. Hitung Pin yang jatuh (cek dulu apakah masuk selokan)
        int pinsDown = ballInGutter ? 0 : CountFallenPins();
        
        Debug.Log("Pin yang jatuh: " + pinsDown + " dari 10 pin.");
        
        // Reset flag selokan untuk lemparan berikutnya
        ballInGutter = false;
        
        // 3. Simpan Poin ke history
        rolls[currentRollIndex] = pinsDown;
        
        // Hitung skor sementara buat ditampilkan
        int totalScore = CalculateRealScore();
        scoreText.text = "Frame: " + currentFrame + " | Skor: " + totalScore;

        // 4. Logika Pindah Frame / Reset
        currentRollIndex++;

        if (currentThrow == 1)
        {
            // Cek STRIKE (Jatuh 10 di lemparan pertama)
            if (pinsDown == 10)
            {
                Debug.Log("STRIKE!");
                scoreText.text += " (STRIKE!)";
                NextFrame(); // Langsung pindah frame (skip lemparan ke-2)
            }
            else
            {
                // Kalau gak strike, lanjut lemparan ke-2
                currentThrow = 2;
                RemoveFallenPins(); // Hilangkan pin yang sudah jatuh
                ResetBallOnly();
            }
        }
        else // Lemparan ke-2
        {
            NextFrame();
        }
    }

    void NextFrame()
    {
        currentFrame++;
        currentThrow = 1;

        if (currentFrame > 10)
        {
            scoreText.text = "GAME OVER! Final Score: " + CalculateRealScore();
            isGameRunning = false;
        }
        else
        {
            ResetBallOnly();
            ResetAllPins(); // Berdirikan semua pin lagi buat frame baru
        }
    }

    // --- LOGIKA HITUNG SKOR DUNIA NYATA ---
    int CalculateRealScore()
    {
        int score = 0;
        int rollIndex = 0;

        for (int frame = 0; frame < 10; frame++)
        {
            if (IsStrike(rollIndex)) // Strike
            {
                score += 10 + rolls[rollIndex + 1] + rolls[rollIndex + 2];
                rollIndex += 1;
            }
            else if (IsSpare(rollIndex)) // Spare
            {
                score += 10 + rolls[rollIndex + 2];
                rollIndex += 2;
            }
            else // Normal
            {
                score += rolls[rollIndex] + rolls[rollIndex + 1];
                rollIndex += 2;
            }
        }
        return score;
    }

    bool IsStrike(int rollIndex) { return rolls[rollIndex] == 10; }
    bool IsSpare(int rollIndex) { return rolls[rollIndex] + rolls[rollIndex + 1] == 10; }

    // --- FUNGSI BANTUAN ---
    int CountFallenPins()
    {
        int count = 0;
        foreach (Pin p in pins)
        {
            // Hitung pin yang aktif DAN jatuh
            if (p.gameObject.activeSelf && p.IsFallen())
            {
                count++;
            }
        }
        return count;
    }

    // Hilangkan pin yang sudah jatuh (untuk lemparan ke-2)
    void RemoveFallenPins()
    {
        int removedCount = 0;
        int standingCount = 0;
        
        foreach (Pin p in pins)
        {
            if (p.gameObject.activeSelf)
            {
                if (p.IsFallen())
                {
                    p.HidePin(); // Sembunyikan pin yang jatuh
                    removedCount++;
                    Debug.Log(p.gameObject.name + " - JATUH (dihilangkan)");
                }
                else
                {
                    standingCount++;
                    Debug.Log(p.gameObject.name + " - BERDIRI (tetap ada)");
                }
            }
        }
        
        Debug.Log("=== HASIL: " + removedCount + " pin dihilangkan, " + standingCount + " pin masih berdiri ===");
    }

    void ResetAllPins()
    {
        foreach (Pin p in pins) p.ResetPin();
    }

    void ResetBallOnly()
    {
        Rigidbody rbBall = ballObject.GetComponent<Rigidbody>();
        rbBall.velocity = Vector3.zero;
        rbBall.angularVelocity = Vector3.zero;
        
        // Pindahkan bola ke script 'HandController' biar bisa diambil lagi
        // Atau taruh di spawn position
        ballObject.position = ballSpawnPos.position;
        ballObject.rotation = Quaternion.identity;
    }

    public void ResetGame()
    {
        currentFrame = 1;
        currentThrow = 1;
        currentRollIndex = 0;
        isGameRunning = true;
        ballInGutter = false; // Reset flag selokan
        
        // Reset array poin
        for(int i=0; i<rolls.Length; i++) rolls[i] = 0;

        ResetAllPins();
        ResetBallOnly();
        scoreText.text = "SIAP MAIN!";
    }

    // Fungsi dipanggil dari Trigger Selokan (GutterTrigger)
    public void OnBallEnteredGutter()
    {
        Debug.Log("Bola Masuk Selokan - 0 Poin!");
        ballInGutter = true;
    }
}