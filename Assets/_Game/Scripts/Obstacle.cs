using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshMaterial;
    [SerializeField] private Material originMaterial;
    [SerializeField] private Material transparentMaterial;

    private void Start()
    {
        meshMaterial.material = originMaterial;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.DETECT_CIRCLE))
        {
            meshMaterial.material = transparentMaterial;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.DETECT_CIRCLE))
        {
            meshMaterial.material = originMaterial;
        }
    }
}
