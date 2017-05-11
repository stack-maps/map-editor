using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This class represents an area which will be filled by aisles. It keeps a
  /// list of aisles belonging to this area. If this area is scaled, moved or
  /// rotated, all its child aisles will follow suite.
  /// </summary>
  public class AisleArea : MonoBehaviour {
    // The list of aisles belonging to this area.
    public List<Aisle> aisles = new List<Aisle>();

    /// <summary>
    /// The container for aisles.
    /// </summary>
    public Transform container;

    bool horizontal;

    /// <summary>
    /// The orientation within this rectangle. Which way should we line up the
    /// stacks?
    /// </summary>
    public bool IsHorizontal() {
      return horizontal;
    }

    public void SetHorizontal(bool orientation) {
      if (orientation == horizontal) {
        return;
      }

      horizontal = orientation;
      container.GetComponent<Resizer>().swapWidthHeight = horizontal;
      container.localEulerAngles = new Vector3(0, 0, horizontal ? 90 : 0);
    }

  }
}
