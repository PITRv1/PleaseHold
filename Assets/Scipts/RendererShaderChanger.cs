using UnityEngine;

public class RendererShaderChanger : MonoBehaviour
{
    [SerializeField] private bool outlineShaderState;
    [SerializeField] private FullScreenPassRendererFeature outlineShaderPass;

    private void Start()
    {
        ToggleShaderPass(outlineShaderState);
    }

    public void ToggleShaderPass(bool state)
    {
        if (outlineShaderPass != null)
        {
            outlineShaderPass.SetActive(state);
        }
    }
}
