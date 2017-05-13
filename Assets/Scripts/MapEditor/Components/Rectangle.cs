using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// Represents a rotatable rectangle.
  /// </summary>
  public class Rectangle : MonoBehaviour {

    /// <summary>
    /// If true, rectangle will not show up in the editor.
    /// </summary>
    public bool disableEditing;

    /// <summary>
    /// Gets the center in relative position to the canvas.
    /// </summary>
    public Vector2 GetCenter() {
      return ((RectTransform)transform).anchoredPosition;
    }

    public void SetCenter(Vector2 center) {
      ((RectTransform)transform).anchoredPosition = center;
    }

    public Vector2 GetSize() {
      return ((RectTransform)transform).sizeDelta;
    }

    public void SetSize(Vector2 size) {
      ((RectTransform)transform).sizeDelta = size;
    }

    public float GetRotation() {
      return transform.localEulerAngles.z;
    }

    public void SetRotation(float degrees) {
      Vector3 angles = transform.localEulerAngles;
      angles.z = degrees;
      transform.localEulerAngles = angles;
    }

    public Rect GetRect() {
      Vector2 c = GetCenter();
      Vector2 s = GetSize();

      return new Rect(c - s / 2, s);
    }

    public override int GetHashCode() {
      float i = 0;
      i += disableEditing? 1 : 0;
      Vector2 pos = ((RectTransform)transform).anchoredPosition;
      i += pos.x * 1717;
      i += pos.y * 112;
      Vector2 s = ((RectTransform)transform).sizeDelta;
      i += s.x * 11;
      i += s.y * 3;
      i += transform.localEulerAngles.z * 0.25f;

      return (int)i;
    }
  }
}
