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

    /// <summary>
    /// Flips the area by 90 degrees while exchanging width with height.
    /// Effectively turning all the stacks.
    /// </summary>
    public void Flip() {
      Rectangle r = GetComponent<Rectangle>();
      r.SetRotation(r.GetRotation() + (horizontal ? 90 : -90));
      r.SetSize(new Vector2(r.GetSize().y, r.GetSize().x));
      horizontal = !horizontal;
    }

    public JSONNode ToJSON() {
      JSONObject root = new JSONObject();
      Rectangle r = GetComponent<Rectangle>();
      root["center_x"] = r.GetCenter().x;
      root["center_y"] = r.GetCenter().y;
      root["height"] = r.GetSize().y;
      root["width"] = r.GetSize().x;
      root["rotation"] = r.GetRotation();
      root["aisles"] = new JSONArray();

      foreach (Aisle aisle in aisles) {
        root["aisles"].Add(aisle.ToJSON());
      }

      return root;
    }

    public void FromJSON(FloorController api, JSONNode root) {
      Rectangle r = GetComponent<Rectangle>();
      r.SetCenter(new Vector2(root["center_x"].AsFloat, root["center_y"].AsFloat));
      r.SetSize(new Vector2(root["width"].AsFloat, root["height"].AsFloat));
      r.SetRotation(root["rotation"].AsFloat);

      foreach (JSONNode node in root["aisles"].AsArray) {
        Aisle aisle = api.CreateAisle(Rect.zero, false, true, true);
        aisle.transform.SetParent(container, false);
        aisle.FromJSON(api, node);
      }
    }
  }
}
