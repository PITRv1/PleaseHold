using UnityEngine;
using UnityEngine.UI;

public class RadialGradientFillVisualUI : MonoBehaviour
{
    [SerializeField] private Image radialImage; // The UI Image (Radial Fill)
    [SerializeField] private Gradient fillGradient; // The color gradient

    private void Update()
    {
        if (radialImage != null)
        {
            // Get the fill amount (0 to 1) and apply gradient color
            radialImage.color = fillGradient.Evaluate(radialImage.fillAmount);
        }
    }
}
