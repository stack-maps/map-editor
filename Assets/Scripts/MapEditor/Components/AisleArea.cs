using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This class represents an area which will be filled by aisles. It keeps a
  /// list of aisles belonging to this area. If this area is scaled, moved or
  /// rotated, all its child aisles will follow suite.
  /// </summary>
  public class AisleArea : MonoBehaviour {
    // The list of aisles belonging to this area.
    List<Aisle> aisles = new List<Aisle>();
  }
}
