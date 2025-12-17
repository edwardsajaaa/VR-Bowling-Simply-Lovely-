using UnityEngine;

public class Pin : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        // Simpan posisi awal buat reset nanti
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Fungsi untuk mengecek apakah pin jatuh
    public bool IsFallen()
    {
        // Logika: Jika kemiringan (sudut X atau Z) lebih dari 45 derajat, dianggap jatuh
        Vector3 rotation = transform.rotation.eulerAngles;
        
        // Normalisasi sudut agar terbaca -180 sampai 180
        float tiltX = Mathf.Abs(transform.up.y); 
        
        // Jika sumbu Y (atas) tidak lagi menunjuk ke atas (kurang dari 0.7), berarti miring
        return tiltX < 0.7f;
    }

    public void ResetPin()
    {
        // Kembalikan ke posisi awal dan berdirikan
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        transform.position = startPosition;
        transform.rotation = startRotation;
        
        // Aktifkan kembali (jika sempat dimatikan)
        gameObject.SetActive(true);
    }
}