﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an entire floor of a library. A floor of a library
/// consists of aisles, aisle areas, walls, and a reference image.
/// </summary>
public class Floor : MonoBehaviour {
  // All aisles not in an aisle area on this floor.
  List<Aisle> aisles = new List<Aisle>();

  // All aisle areas, each having a reference to its children aisles.
  List<AisleArea> aisleAreas = new List<AisleArea>();

  // All walls on this floor.
  List<Wall> walls = new List<Wall>();

  // The name of the floor, e.g. 1.
  public string floorName = "New Floor";
}