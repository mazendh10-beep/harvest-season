using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResolutionHandler : MonoBehaviour
{
    private static ResolutionHandler instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void ChangeResolution(int width, int height, bool fullscreen)
    {
        if (instance == null)
        {
            GameObject obj = new GameObject("ResolutionHandler");
            instance = obj.AddComponent<ResolutionHandler>();
        }
        instance.StartCoroutine(instance.ApplyResolutionRoutineMode(width, height, fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed));
    }

    private IEnumerator ApplyResolutionRoutineMode(int width, int height, FullScreenMode fullScreenMode)
    {
        Debug.Log($"Changing resolution to: {width}x{height}");

        EventSystem eventSystem = EventSystem.current;
        if (eventSystem != null)
        {
            eventSystem.enabled = false;
            eventSystem.SetSelectedGameObject(null);
        }

        GraphicRaycaster[] raycasters = FindObjectsByType<GraphicRaycaster>(FindObjectsSortMode.None);
        foreach (var raycaster in raycasters)
            if (raycaster != null) raycaster.enabled = false;

        Screen.SetResolution(width, height, fullScreenMode);

        yield return new WaitForEndOfFrame();
        yield return null;
        yield return null;

        Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas != null && canvas.gameObject.activeInHierarchy)
            {
                canvas.enabled = false;
                canvas.enabled = true;
                RectTransform rectTransform = canvas.GetComponent<RectTransform>();
                if (rectTransform != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
        }

        Canvas.ForceUpdateCanvases();
        yield return null;

        foreach (var raycaster in raycasters)
            if (raycaster != null) raycaster.enabled = true;

        if (eventSystem != null)
        {
            eventSystem.enabled = true;
            eventSystem.UpdateModules();
        }

        Debug.Log("Resolution change complete");
    }
}
