using UnityEngine;

public class PlayerScaler : MonoBehaviour
{
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // Save the original scale
        AdjustPlayerWidth();
    }

    void Update()
    {
        AdjustPlayerWidth();
    }

    void AdjustPlayerWidth()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (screenHeight > screenWidth) // Portrait mode
        {
            float scaleFactor = 0.70f; // 75% of the original width
            transform.localScale = originalScale * scaleFactor; // Scale once from original
        }
        else
        {
            transform.localScale = originalScale; // Reset in landscape mode
        }
    }
}
