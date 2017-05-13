using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace StackMaps {
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


    public JSONNode ToJSON() {
      JSONObject root = new JSONObject();
      root["x1"] = start.x;
      root["y1"] = start.y;
      root["x2"] = end.x;
      root["y2"] = end.y;

      return root;
    }

    public void FromJSON(FloorController api, JSONNode root) {
      SetStart(new Vector2(root["x1"].AsFloat, root["y1"].AsFloat));
      SetEnd(new Vector2(root["x2"].AsFloat, root["y2"].AsFloat));

      name = "(" + ActionManager.shared.index + ")" + GetHashCode();
    }

    public override int GetHashCode() {
      float i = 0;
      Vector2 pos = GetStart();
      i += pos.x * 1351;
      i += pos.y * 513;
      Vector2 s = GetEnd();
      i += s.x * 23;
      i += s.y * 7;
      i += transform.localEulerAngles.z * 0.25f;

      return (int)i;
    }
  }
}