using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditAreaController : MonoBehaviour {
  public RectTransform canvas;

  public ScrollRect scrollRect;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  // These levels of zoom are arbitrary and can be changed any time.
  readonly float[] zoomLevels = {0.25f, 0.33f, 0.5f, 0.75f, 1, 2, 4};

  /// <summary>
  /// Updates the zoom level of the canvas.
  /// </summary>
  /// <param name="zoomLevel">Zoom level.</param>
  public void UpdateZoom(int zoomLevel) {
    if (zoomLevel < zoomLevels.Length) {
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

}
