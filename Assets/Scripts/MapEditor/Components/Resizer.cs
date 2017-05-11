using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// Updates the transform such that the width is parent's height and vice versa.
  /// </summary>
  public class Resizer : MonoBehaviour {
    public bool swapWidthHeight = false;

    // Update is called once per frame
    void Update() {
      if (transform.parent != null) {
        Rect r = ((RectTransform)transform.parent).rect;

        if (swapWidthHeight)
          ((RectTransform)transform).sizeDelta = new Vector2(r.height, r.width);
        else
          ((RectTransform)transform).sizeDelta = r.size;
      }
    }
  }
}