using UnityEngine;

public class AutoResolutionManager : MonoBehaviour
{
    void Start()
    {
        // Ensure ResolutionHandler exists
        if (FindObjectsByType<ResolutionHandler>(FindObjectsSortMode.None).Length == 0)
        {
            GameObject obj = new GameObject("ResolutionHandler");
            obj.AddComponent<ResolutionHandler>();
        }
    }
}
