using UnityEngine;

public class GiveXP : MonoBehaviour
{
    [Header("XP Drop")]
    public GameObject xpBlobPrefab;
    public int xpReward = 1;

    public void DropXP()
    {
        if (xpBlobPrefab == null)
        {
            Debug.LogWarning(gameObject.name + ": XP Blob Prefab is missing.");
            return;
        }

        GameObject xpBlobObject = Instantiate(
            xpBlobPrefab,
            transform.position,
            Quaternion.identity
        );

        XPBlob xpBlob = xpBlobObject.GetComponent<XPBlob>();

        if (xpBlob != null)
        {
            xpBlob.xpAmount = xpReward;
        }
        else
        {
            Debug.LogWarning(gameObject.name + ": XP Blob Prefab does not have an XPBlob component.");
        }
    }
}