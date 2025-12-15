using UnityEngine;

public class EditorMouseLook : MonoBehaviour
{
    public float sensitivity = 200f;

    void Update()
    {
        // Script ini HANYA jalan di Unity Editor (Laptop)
        // Di HP script ini otomatis mati.
        if (!Application.isEditor) return;

        // Tahan Klik Kanan (Mouse 1) atau Alt untuk menengok
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftAlt))
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            // Putar player/kamera
            // Rotasi Y (Kiri Kanan)
            transform.Rotate(Vector3.up * mouseX);
            
            // Rotasi X (Atas Bawah) - hati-hati gimbal lock, tapi untuk tes simple ok
            Vector3 currentRotation = transform.localEulerAngles;
            currentRotation.x -= mouseY;
            transform.localEulerAngles = currentRotation;
        }
    }
}