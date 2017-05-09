using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StackMaps {
  /// <summary>
  /// This class represents an aisle object on the map.
  /// </summary>
  [ExecuteInEditMode]
  public class Aisle : MonoBehaviour {
    // Whether this aisle is single sided.
    public bool singleSided;

    public List<CallNumberRange> callNumberRanges = new List<CallNumberRange>();

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
  }
}