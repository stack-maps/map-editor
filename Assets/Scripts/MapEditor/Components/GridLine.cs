using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StackMaps {
  /// <summary>
  /// This class controls the individual grid lines.
  /// </summary>
  public class GridLine : MonoBehaviour {
    public Image background;
    public RectTransform rect;

    /// <summary>
    /// Setups the grid line's appearance.
    /// </summary>
    public void SetupGridLine(Color c, float thickness, float height) {
      background.color = c;
      rect.sizeDelta = new Vector2(thickness, height);
    }
  }
}
