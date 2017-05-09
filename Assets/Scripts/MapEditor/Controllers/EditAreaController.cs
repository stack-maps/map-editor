using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditAreaController : MonoBehaviour {
  // The canvas where things are actually on the map.
  public RectTransform canvas;

  // The scroll rect of the edit area.
  public ScrollRect scrollRect;

  // The toolbar associated with the editing area.
  public ToolbarController toolbarController;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    ToggleScrolling(toolbarController.toolbar.GetActiveTool() == ToolType.SelectionTool);
	}

  // These levels of zoom are arbitrary and can be changed any time.
  readonly float[] zoomLevels = {0.25f, 0.33f, 0.5f, 0.75f, 1, 2, 4};

  /// <summary>
  /// Updates the zoom level of the canvas.
  /// </summary>
  /// <param name="zoomLevel">Zoom level.</param>
  public void UpdateZoom(int zoomLevel) {
    if (zoomLevel < 0) {
      // We didn't select anything. do nothing.
      return;
    } else if (zoomLevel < zoomLevels.Length) {
      canvas.localScale = new Vector3(zoomLevels[zoomLevel], zoomLevels[zoomLevel], 1);
    } else if (zoomLevel == zoomLevels.Length) {
      // We assume we want fit width here. Do some calculations.
      // We want to scale the view so the canvas sits at 90% of the view.
      float ratio = scrollRect.viewport.rect.width * 0.9f / canvas.rect.width;
      canvas.localScale = new Vector3(ratio, ratio, 1);
    } else {
      // We assume we want fit window here. Do some calculations.
      // We want to scale the view so the canvas sits at 90% of the view.
      float ratio1 = scrollRect.viewport.rect.width * 0.9f / canvas.rect.width;
      float ratio2 = scrollRect.viewport.rect.height * 0.9f / canvas.rect.height;
      float ratio = Mathf.Min(ratio1, ratio2);
      canvas.localScale = new Vector3(ratio, ratio, 1);
    }
  }

  /// <summary>
  /// Toggles the ability to scroll on the canvas.
  /// </summary>
  /// <param name="canScroll">If set to <c>true</c>, user can scroll.</param>
  public void ToggleScrolling(bool canScroll) {
    scrollRect.horizontal = canScroll;
    scrollRect.vertical = canScroll;
  }

}
