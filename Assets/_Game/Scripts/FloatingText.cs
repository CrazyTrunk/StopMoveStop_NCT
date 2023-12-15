using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 1f;
    public Vector3 offset  = new Vector3 (0, 5, 0);
    // Start is called before the first frame update
    void Start()
    {
        //Todo : Add Leanpool
        Destroy(gameObject, DestroyTime);
        transform.localPosition += offset;
    }
}
