﻿using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace StackMaps {
  /// <summary>
  /// This class represents an entire floor of a library. A floor of a library
  /// consists of aisles, aisle areas, walls, and a reference image.
  /// </summary>
  public class Floor {
    /// <summary>
    /// The floor identifier. This is always returned from the database.
    /// </summary>
    public int floorId;

    /// <summary>
    /// The order of floor within the library. This is always returned from the
    /// database.
    /// </summary>
    public int floorOrder;

    /// <summary>
    /// The library identifier. This is always returned from the database.
    /// </summary>
    public int libraryId;

    // All aisles not in an aisle area on this floor.
    public List<Aisle> aisles = new List<Aisle>();

    // All aisle areas, each having a reference to its children aisles.
    public List<AisleArea> aisleAreas = new List<AisleArea>();

    // All walls on this floor.
    public List<Wall> walls = new List<Wall>();

    public List<Landmark> landmarks = new List<Landmark>();

    // The name of the floor, e.g. 1.
    public string floorName = "New Floor";

    /// <summary>
    /// Serializes the whole floor to a single JSON object.
    /// </summary>
    /// <returns>The JSO.</returns>
    public JSONNode ToJSON() {
      JSONObject root = new JSONObject();
      root["fid"] = floorId;
      root["fname"] = floorName;
      root["library"] = libraryId;
      root["Aisle"] = new JSONArray();
      root["AisleArea"] = new JSONArray();
      root["Wall"] = new JSONArray();
      root["Landmark"] = new JSONArray();

      foreach (Aisle aisle in aisles) {
        root["Aisle"].Add(aisle.ToJSON());
      }

      foreach (AisleArea area in aisleAreas) {
        root["AisleArea"].Add(area.ToJSON());
      }

      foreach (Wall wall in walls) {
        root["Wall"].Add(wall.ToJSON());
      }

      foreach (Landmark landmark in landmarks) {
        root["Landmark"].Add(landmark.ToJSON());
      }

      return root;
    }

    /// <summary>
    /// Overwrites this floor's content with JSON object root.
    /// </summary>
    /// <param name = "api">The FloorController.</param>
    /// <param name="root">Root.</param>
    public void FromJSON(FloorController api, JSONNode root) {
      floorId = root["fid"];
      floorName = root["fname"];
      floorOrder = root["fname"];
      libraryId = root["library"];

      aisles.Clear();
      aisleAreas.Clear();
      walls.Clear();
      landmarks.Clear();

      foreach (JSONObject obj in root["Aisle"].AsArray) {
        Aisle aisle = api.CreateAisle(Rect.zero, false, true);
        aisle.FromJSON(api, obj);
      }

      foreach (JSONObject obj in root["AisleArea"].AsArray) {
        AisleArea aisleArea = api.CreateAisleArea(Rect.zero, false, true);
        aisleArea.FromJSON(api, obj);
      }

      foreach (JSONObject obj in root["Wall"].AsArray) {
        Wall wall = api.CreateWall(Vector2.zero, Vector2.zero, false, true);
        wall.FromJSON(api, obj);
      }

      foreach (JSONObject obj in root["Landmark"].AsArray) {
        Landmark landmark = api.CreateLandmark(Rect.zero, false, true);
        landmark.FromJSON(api, obj);
      }
    }
  }
}
