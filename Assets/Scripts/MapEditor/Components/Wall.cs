using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a beautifully drawn line on the map.
/// </summary>
public class Wall : MonoBehaviour {
  Vector2 start, end;

  public Vector2 GetStart() {
    return start;
  }

  public Vector2 GetEnd() {
    return end;
  }

  public void SetStart(Vector2 pt) {
    start = pt;
    AdjustTransform();
  }

  public void SetEnd(Vector2 pt) {
    end = pt;
    AdjustTransform();
  }

  /// <summary>
  /// Adjusts the transform according to starting and ending position.
  /// </summary>
  void AdjustTransform() {
    Vector2 sizeDelta = ((RectTransform)transform).sizeDelta;
    sizeDelta.x = (start - end).magnitude;
    ((RectTransform)transform).sizeDelta = sizeDelta;
    ((RectTransform)transform).anchoredPosition = start;
    Vector3 angles = transform.localEulerAngles;
    angles.z = Mathf.Rad2Deg * Mathf.Atan2((end - start).y, (end - start).x);
    transform.localEulerAngles = angles;
  }
}
