using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// Updates the transform such that the width is parent's height and vice versa.
  /// </summary>
  public class Resizer : MonoBehaviour {
    /// <summary>
    /// A margin from the edge.
    /// </summary>
    public Vector2 margin;

    /// <summary>
    /// Should we swap the height and width?
    /// </summary>
    public bool swapWidthHeight = false;

    // Update is called once per frame
    void Update() {
      if (transform.parent != null) {
        Rect r = ((RectTransform)transform.parent).rect;

        if (swapWidthHeight)
          ((RectTransform)transform).sizeDelta = new Vector2(Mathf.Abs(r.height), Mathf.Abs(r.width)) - margin * 2;
        else
          ((RectTransform)transform).sizeDelta = new Vector2(Mathf.Abs(r.width), Mathf.Abs(r.height)) - margin * 2;
      }
    }
  }
}