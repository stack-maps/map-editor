using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This class represents an entire floor of a library. A floor of a library
  /// consists of aisles, aisle areas, walls, and a reference image.
  /// </summary>
  public class Floor : MonoBehaviour {
    // All aisles not in an aisle area on this floor.
    public List<Aisle> aisles = new List<Aisle>();

    // All aisle areas, each having a reference to its children aisles.
    public List<AisleArea> aisleAreas = new List<AisleArea>();

    // All walls on this floor.
    public List<Wall> walls = new List<Wall>();

    public List<Landmark> landmarks = new List<Landmark>();

    // The name of the floor, e.g. 1.
    public string floorName = "New Floor";
  }
}
