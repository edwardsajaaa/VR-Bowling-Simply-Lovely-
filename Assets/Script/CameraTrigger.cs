using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Header("Kamera")]
    public GameObject mainCamera; // Kamera Player
    public GameObject pinCamera;  // Kamera Pin

    // Saat Bola MASUK ke dalam kotak area
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang masuk itu Bola? (Bukan pin/lantai)
        if (other.CompareTag("Ball"))
        {
            mainCamera.SetActive(false);
            pinCamera.SetActive(true);
        }
    }

    // Saat Bola KELUAR dari kotak area (misal saat direset balik ke tangan)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            pinCamera.SetActive(false);
            mainCamera.SetActive(true);
        }
    }
}