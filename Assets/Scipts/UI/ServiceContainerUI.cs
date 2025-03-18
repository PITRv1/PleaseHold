using UnityEngine;

public class ServiceContainerUI : MonoBehaviour
{
    [SerializeField] private ServiceInputUI serviceTemplate;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(serviceTemplate, this.transform);
        }
    }
}
