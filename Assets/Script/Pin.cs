using UnityEngine;

public class Pin : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public bool IsFallen()
    {
        // CARA BARU (LEBIH AKURAT):
        // Kita hitung sudut kemiringan antara "Atas Dunia" (Langit) dan "Atas Pin"
        float angle = Vector3.Angle(Vector3.up, transform.up);

        // Debugging: Cek di Console kalau ada pin yang aneh
        if (angle > 20f)
        {
            Debug.Log(gameObject.name + " - Sudut miring: " + angle.ToString("F1") + "° (" + (angle > 45f ? "JATUH" : "MASIH BERDIRI") + ")");
        }

        // ATURAN DIPERBAIKI: 
        // Jika kemiringan lebih dari 45 derajat -> Dianggap JATUH
        // (Pin yang goyang sedikit masih dianggap berdiri)
        if (angle > 45f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetPin()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        // Matikan velocity biar tidak meluncur pas direset
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        // Berdirikan lagi
        transform.position = startPosition;
        transform.rotation = startRotation;
        
        gameObject.SetActive(true);
    }

    // Sembunyikan pin yang sudah jatuh (untuk lemparan ke-2)
    public void HidePin()
    {
        gameObject.SetActive(false);
    }
}