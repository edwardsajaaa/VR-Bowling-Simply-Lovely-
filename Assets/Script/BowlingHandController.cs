using UnityEngine;

public class BowlingHandController : MonoBehaviour
{
    [Header("Referensi")]
    public Transform ballHolder;      // Tarik objek 'BallHolder' (anak kamera) ke sini
    public Transform cameraTransform; // Tarik 'Main Camera' ke sini

    [Header("Settings Lemparan")]
    // Karena pakai ForceMode.Impulse, angkanya kecil saja (10-20) sudah sangat kuat
    public float throwForce = 15f;    
    public float pickupRange = 5f;    // Jarak pandang untuk ambil bola
    public float spinAmount = 10f;    // Putaran bola agar menggelinding cantik

    private Rigidbody rb;
    private bool isHolding = false;   // Status sedang pegang bola atau tidak

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Pastikan gravitasi nyala di awal
        rb.useGravity = true;

        // PENTING: Agar bola tidak tembus tembok saat dilempar kencang
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Note: Saya HAPUS baris 'rb.mass = 6f' agar Anda bisa atur bebas di Inspector.
        
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // LOGIKA 1: Jika SEDANG memegang bola
        if (isHolding)
        {
            // Cek Input Klik (Sentuh Layar/Tombol Cardboard/Klik Kiri Mouse)
            if (Input.GetButtonDown("Fire1")) 
            {
                ThrowBall();
            }
        }
        // LOGIKA 2: Jika BELUM pegang bola (Cari bola untuk diambil)
        else
        {
            CheckForPickup();
        }
    }

    // Fungsi Mendeteksi Tatapan (Raycast)
    void CheckForPickup()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.transform == this.transform)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    PickupBall();
                }
            }
        }
    }

    void PickupBall()
    {
        isHolding = true;

        // Matikan Fisika agar bola menempel di tangan (tidak jatuh)
        rb.useGravity = false;
        rb.isKinematic = true; 

        // Tempelkan bola ke BallHolder
        transform.SetParent(ballHolder);
        
        // Reset posisi & rotasi bola ke titik 0 BallHolder
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void ThrowBall()
    {
        isHolding = false;

        // Lepaskan bola dari tangan
        transform.SetParent(null);

        // Nyalakan Fisika lagi
        rb.useGravity = true;
        rb.isKinematic = false;

        // --- LOGIKA LEMPARAN REALISTIS ---

        // 1. Ambil arah pandangan mata
        Vector3 throwDirection = cameraTransform.forward;

        // 2. Ratakan arah Y jadi 0 (Supaya bola melempar LURUS MENDATAR, tidak nyusruk tanah)
        throwDirection.y = 0; 
        throwDirection.Normalize();

        // 3. Tambahkan gaya dorong tipe IMPULSE (Ledakan Instan)
        // Ini bikin bola langsung ngebut di awal (rekomendasi untuk benda berat)
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        // 4. Tambahkan gaya putar (Torque) agar bola menggelinding visualnya
        rb.AddTorque(transform.right * spinAmount, ForceMode.Impulse);
    }
}