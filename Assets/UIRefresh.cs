using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIRefresh : MonoBehaviour
{
    private Vector2Int lastScreenSize;
    private bool isRefreshing = false;

    void Start()
    {
        lastScreenSize = new Vector2Int(Screen.width, Screen.height);
        RefreshUI();
    }

    void Update()
    {
        // Detect resolution changes (including window resizing)
        if (Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
        {
            lastScreenSize = new Vector2Int(Screen.width, Screen.height);
            OnResolutionChanged();
        }
    }

    public void OnResolutionChanged()
    {
        if (!isRefreshing)
        {
            StartCoroutine(RefreshUIRoutine());
        }
    }

    private IEnumerator RefreshUIRoutine()
    {
        isRefreshing = true;
        
        // Step 1: Disable EventSystem
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem != null)
        {
            eventSystem.enabled = false;
            eventSystem.SetSelectedGameObject(null);
        }

        // Step 2: Disable all raycasters
        GraphicRaycaster[] raycasters = FindObjectsByType<GraphicRaycaster>(FindObjectsSortMode.None);
        foreach (var raycaster in raycasters)
        {
            if (raycaster != null)
            {
                raycaster.enabled = false;
            }
        }

        // Step 3: Wait a frame
        yield return null;

        // Step 4: Force all canvases to rebuild
        Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas != null && canvas.isActiveAndEnabled)
            {
                // Toggle canvas to force rebuild
                canvas.enabled = false;
                canvas.enabled = true;
                
                // Force layout rebuild
                RectTransform rectTransform = canvas.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
                }
            }
        }

        Canvas.ForceUpdateCanvases();

        // Step 5: Wait another frame
        yield return null;

        // Step 6: Re-enable all raycasters
        foreach (var raycaster in raycasters)
        {
            if (raycaster != null)
            {
                raycaster.enabled = true;
            }
        }

        // Step 7: Re-enable EventSystem
        if (eventSystem != null)
        {
            eventSystem.enabled = true;
            eventSystem.UpdateModules();
        }

        // Step 8: Wait final frame
        yield return null;

        Debug.Log($"UI Refreshed at resolution: {Screen.width}x{Screen.height}");
        
        isRefreshing = false;
    }

    private void RefreshUI()
    {
        // Force all canvases to update
        Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas != null && canvas.isActiveAndEnabled)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(canvas.GetComponent<RectTransform>());
            }
        }

        Canvas.ForceUpdateCanvases();

        // Ensure EventSystem is functioning
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            
            // Re-enable all graphic raycasters
            GraphicRaycaster[] raycasters = FindObjectsByType<GraphicRaycaster>(FindObjectsSortMode.None);
            foreach (var raycaster in raycasters)
            {
                if (raycaster != null)
                {
                    raycaster.enabled = false;
                    raycaster.enabled = true;
                }
            }
        }

        Debug.Log($"UI Refreshed at resolution: {Screen.width}x{Screen.height}");
    }
}
