using UnityEngine;
using UnityEngine.UI;

public class TileBackgroundUI : MonoBehaviour
{
    public RawImage rawImage;
    public float tilingFactor = 1.0f; // Adjust this to control tiling density

    private void Start()
    {
        if (rawImage == null) return;

        // Set the texture tiling
        rawImage.uvRect = new Rect(0, 0, rawImage.rectTransform.rect.width / rawImage.texture.width * tilingFactor,
                                            rawImage.rectTransform.rect.height / rawImage.texture.height * tilingFactor);
    }
}
