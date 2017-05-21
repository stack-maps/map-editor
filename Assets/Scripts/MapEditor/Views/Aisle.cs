using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

namespace StackMaps {
  /// <summary>
  /// This class represents an aisle object on the map.
  /// </summary>
  [ExecuteInEditMode]
  public class Aisle : MonoBehaviour {
    // Whether this aisle is single sided.
    public bool singleSided;

    public List<CallNumberRange> callNumberRanges;

    void Awake() {
      // Initializes a stack to be two sided with two call number ranges.
      callNumberRanges = new List<CallNumberRange> {
        new CallNumberRange(), new CallNumberRange()
      };

      callNumberRanges[0].isSideA = true;
      callNumberRanges[1].isSideA = false;
    }

    // Used to toggle sides.
    public Text sideAText;
    public GameObject sideB;

    void Update() {
      // Update sprite depending on single/double sided-ness.
      if (sideAText != null)
        sideAText.gameObject.SetActive(!singleSided);

      if (sideB != null)
        sideB.SetActive(!singleSided);
    }

    public JSONNode ToJSON() {
      JSONObject root = new JSONObject();
      Rectangle r = GetComponent<Rectangle>();
      root["center_x"] = r.GetCenter().x;
      root["center_y"] = r.GetCenter().y;
      root["height"] = r.GetSize().y;
      root["width"] = r.GetSize().x;
      root["rotation"] = r.GetRotation();
      root["is_double_sided"] = !singleSided;
      root["call_ranges"] = new JSONArray();

      foreach (CallNumberRange range in callNumberRanges) {
        if (range.IsIncomplete()) {
          continue;
        }

        JSONNode rangeNode = range.ToJSON();
        root["call_ranges"].Add(rangeNode);
      }

      return root;
    }

    public void FromJSON(FloorController api, JSONNode root) {
      Rectangle r = GetComponent<Rectangle>();
      r.SetCenter(new Vector2(root["center_x"].AsFloat, root["center_y"].AsFloat));
      r.SetSize(new Vector2(root["width"].AsFloat, root["height"].AsFloat));
      r.SetRotation(root["rotation"].AsFloat);
      singleSided = root["is_double_sided"].AsInt == 0;
      callNumberRanges = new List<CallNumberRange>();

      foreach (JSONNode node in root["call_ranges"].AsArray) {
        CallNumberRange range = new CallNumberRange();
        range.FromJSON(api, node);
        callNumberRanges.Add(range);
      }
    }
  }
}
