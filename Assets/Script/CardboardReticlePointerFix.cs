using UnityEngine;

/// <summary>
/// Fix untuk bug CardboardReticlePointer null reference
/// Pasang script ini di GameObject yang sama dengan CardboardReticlePointer
/// </summary>
public class CardboardReticlePointerFix : MonoBehaviour
{
    private CardboardReticlePointer reticlePointer;

    void Awake()
    {
        reticlePointer = GetComponent<CardboardReticlePointer>();
        
        if (reticlePointer != null)
        {
            // Pastikan LayerMask tidak kosong
            if (reticlePointer.ReticleInteractionLayerMask == 0)
            {
                // Set ke layer Default (layer 0)
                reticlePointer.ReticleInteractionLayerMask = 1 << 0;
                Debug.Log("CardboardReticlePointerFix: LayerMask di-set ke Default layer");
            }
        }
    }
}
