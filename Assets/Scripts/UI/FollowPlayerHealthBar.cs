using UnityEngine;

public class FollowPlayerHealthBar : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offset")]
    public Vector3 worldOffset = new Vector3(0f, 0.8f, 0f);

    void Start()
    {
        if (target == null && transform.parent != null)
        {
            target = transform.parent;
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = target.position + worldOffset;
        transform.rotation = Quaternion.identity;
    }
}