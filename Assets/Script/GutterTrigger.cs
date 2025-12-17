using UnityEngine;

public class GutterTrigger : MonoBehaviour
{
    private BowlingGameManager gameManager;

    void Start()
    {
        // Cari BowlingGameManager di scene
        gameManager = FindObjectOfType<BowlingGameManager>();
        
        if (gameManager == null)
        {
            Debug.LogError("GutterTrigger: BowlingGameManager tidak ditemukan di scene!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang masuk adalah bola bowling
        if (other.CompareTag("Ball")) // Pastikan bola punya Tag "Ball"
        {
            Debug.Log("Bola masuk selokan!");
            
            if (gameManager != null)
            {
                gameManager.OnBallEnteredGutter();
            }
        }
    }
}
