using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;
using UnityEngine.UI;

namespace StackMaps {
  /// <summary>
  /// Manages a floor preview cell.
  /// </summary>
  public class FloorPreviewCell : MonoBehaviour {
    /// <summary>
    /// The edit menu dropdown.
    /// </summary>
    public MaterialDropdown editMenuDropdown;

    /// <summary>
    /// The floor name text on top left.
    /// </summary>
    public Text floorNameText;

    /// <summary>
    /// The container for floor preview generation.
    /// </summary>
    public RectTransform previewContainer;

    /// <summary>
    /// Clears out floor preview.
    /// </summary>
    public void ClearContainer() {
      for (int i = 0; i < previewContainer.childCount; i++) {
        for (int j = 0; j < previewContainer.GetChild(i).childCount; j++) {
          Destroy(previewContainer.GetChild(i).GetChild(j).gameObject);
        }
      }
    }

    /// <summary>
    /// Sets the name of the floor, shown in the upper left corner.
    /// </summary>
    /// <param name="floorName">Floor name.</param>
    public void SetFloorName(string floorName) {
      floorNameText.text = floorName;
    }

    /// <summary>
    /// Sets whether the move up button in the dropdown is enabled.
    /// </summary>
    public void SetCanMoveUp(bool isNotFirst) {
      CanvasGroup g = editMenuDropdown.listItems[1].canvasGroup;
      g.alpha = isNotFirst ? 1 : 0.5f;
      g.interactable = isNotFirst;
    }

    /// <summary>
    /// Sets whether the move down button in the dropdown is enabled.
    /// </summary>
    public void SetCanMoveDown(bool isNotLast) {
      CanvasGroup g = editMenuDropdown.listItems[2].canvasGroup;
      g.alpha = isNotLast ? 1 : 0.5f;
      g.interactable = isNotLast;
    }
  }
}