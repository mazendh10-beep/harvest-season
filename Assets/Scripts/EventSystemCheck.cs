using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemCheck : MonoBehaviour
{
    void Awake()
    {
        EventSystem[] systems = Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        Debug.Log("EventSystem count: " + systems.Length);

        if (systems.Length == 0)
        {
            Debug.LogError("NO EventSystem found in scene!");
        }
        else if (systems.Length > 1)
        {
            Debug.LogError("MULTIPLE EventSystems found in scene!");
        }
    }
}
