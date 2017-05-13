using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

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

    public JSONNode ToJSON() {
      JSONObject root = new JSONObject();
      Rectangle r = GetComponent<Rectangle>();
      root["center_x"] = r.GetCenter().x;
      root["center_y"] = r.GetCenter().y;
      root["length"] = horizontal? r.GetSize().y : r.GetSize().x;
      root["width"] = horizontal? r.GetSize().x : r.GetSize().y;
      root["rotation"] = transform.localEulerAngles.z + container.localEulerAngles.z;
      root["Aisle"] = new JSONArray();

      foreach (Aisle aisle in aisles) {
        root["Aisle"].Add(aisle.ToJSON());
      }

      return root;
    }

    public void FromJSON(FloorController api, JSONNode root) {
      Rectangle r = GetComponent<Rectangle>();
      r.SetCenter(new Vector2(root["center_x"].AsFloat, root["center_y"].AsFloat));
      r.SetSize(new Vector2(root["length"].AsFloat, root["width"].AsFloat));
      r.SetRotation(root["rotation"].AsFloat);

      foreach (JSONNode node in root["Aisle"].AsArray) {
        Aisle aisle = api.CreateAisle(Rect.zero, false, true);
        aisle.transform.SetParent(container, false);
        aisle.FromJSON(api, node);
      }

      name = "(" + ActionManager.shared.index + ")" + r.GetHashCode();
    }
  }
}
