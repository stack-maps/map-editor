using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// Displays a preview for a given JSON floor representation string.
  /// </summary>
  public class FloorPreviewController : FloorController {
    /// <summary>
    /// Draws the floor preview in the given container. Creates everything into
    /// the container then scale and position the finished preview to be at the
    /// center of the container.
    /// </summary>
    /// <param name="container">Container.</param>
    /// <param name="floorJson">Floor json.</param>
    public void DrawFloorPreview(RectTransform container, string floorJson) {
      aisleAreaLayer = container.GetChild(0) as RectTransform;
      wallLayer = container.GetChild(1) as RectTransform;
      landmarkLayer = container.GetChild(2) as RectTransform;
      aisleLayer = container.GetChild(3) as RectTransform;

      ImportFloor(floorJson);

      // Do scaling and repositioning here.
      // Calculate a rectangular bound from iterating through every object.
      Rect boundingRect = floor.GetBoundingRect();

      if (boundingRect.size.magnitude > 0) {
        // Then reposition and rescale the container. Only fill up 80% of the area
        float ratioX = container.rect.width / boundingRect.width * 0.8f;
        float ratioY = container.rect.height / boundingRect.height * 0.8f;
        float ratio = Mathf.Max(ratioX, ratioY);
        container.localScale = new Vector3(ratio, ratio, 1);
        // container.localPosition = -boundingRect.center * ratio;
      }
    }
  }
}
