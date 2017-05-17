using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// Updates the transform so its world angle is constant.
  /// </summary>
  public class Orientator : MonoBehaviour {
    public Vector3 worldAngle;

    // Update is called once per frame
    void Update() {
      transform.eulerAngles = worldAngle;
    }
  }
}