using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResolutionUISync : MonoBehaviour
{
    private static ResolutionUISync instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static void ApplyResolution(int width, int height, bool fullscreen)
    {
        if (instance == null)
            instance = new GameObject("ResolutionUISync").AddComponent<ResolutionUISync>();
        
        instance.StartCoroutine(instance.ApplyResolutionRoutine(width, height, fullscreen));
    }

    public static void ApplyResolution(int width, int height, FullScreenMode mode)
    {
        if (instance == null)
            instance = new GameObject("ResolutionUISync").AddComponent<ResolutionUISync>();
        
        instance.StartCoroutine(instance.ApplyResolutionRoutine(width, height, mode));
    }

    private IEnumerator ApplyResolutionRoutine(int width, int height, bool fullscreen)
    {
        yield return ApplyResolutionRoutine(width, height, fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }

    private IEnumerator ApplyResolutionRoutine(int width, int height, FullScreenMode mode)
    {
        // Disable EventSystem
        EventSystem es = EventSystem.current;
        if (es != null)
        {
            es.enabled = false;
            es.SetSelectedGameObject(null);
        }

        // Disable raycasters
        GraphicRaycaster[] raycasters = FindObjectsByType<GraphicRaycaster>(FindObjectsSortMode.None);
        foreach (var r in raycasters)
            if (r != null) r.enabled = false;

        // Change resolution
        Screen.SetResolution(width, height, mode);

        // Wait
        yield return new WaitForEndOfFrame();
        yield return null;
        yield return null;

        // Rebuild canvases
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas c in canvases)
        {
            if (c != null)
            {
                c.enabled = false;
                c.enabled = true;
                LayoutRebuilder.ForceRebuildLayoutImmediate(c.GetComponent<RectTransform>());
            }
        }

        Canvas.ForceUpdateCanvases();
        yield return null;

        // Re-enable raycasters
        foreach (var r in raycasters)
            if (r != null) r.enabled = true;

        // Re-enable EventSystem
        if (es != null)
        {
            es.enabled = true;
            es.UpdateModules();
        }

        yield return null;
    }
}
