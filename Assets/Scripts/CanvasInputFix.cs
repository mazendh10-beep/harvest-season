using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Attach this to your Canvas GameObject to ensure proper UI input after resolution changes
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public class CanvasInputFix : MonoBehaviour
{
    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private CanvasScaler scaler;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();
        scaler = GetComponent<CanvasScaler>();

        // Ensure proper Canvas settings
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // This is usually fine, but ensure it's set
            canvas.pixelPerfect = false;
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            // Make sure camera is assigned
            if (canvas.worldCamera == null)
            {
                canvas.worldCamera = Camera.main;
                Debug.LogWarning($"Canvas '{gameObject.name}' had no camera assigned. Assigned Main Camera.");
            }
        }

        // Ensure GraphicRaycaster is enabled and configured
        if (raycaster != null)
        {
            raycaster.enabled = true;
        }
    }

    void OnEnable()
    {
        // Force rebuild when canvas is enabled
        if (canvas != null)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }

    /// <summary>
    /// Call this method after resolution changes to fix input
    /// </summary>
    public void RefreshCanvasInput()
    {
        StartCoroutine(RefreshInputRoutine());
    }

    private System.Collections.IEnumerator RefreshInputRoutine()
    {
        // Disable raycaster
        if (raycaster != null)
        {
            raycaster.enabled = false;
        }

        // Force canvas rebuild
        canvas.enabled = false;
        yield return null;
        canvas.enabled = true;

        // Force layout rebuild
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        yield return null;

        // Re-enable raycaster
        if (raycaster != null)
        {
            raycaster.enabled = true;
        }

        // Reset EventSystem
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        Debug.Log($"Canvas input refreshed for: {gameObject.name}");
    }
}
