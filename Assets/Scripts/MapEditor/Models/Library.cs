using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

namespace StackMaps {
  /// <summary>
  /// This class represents a library, containing floors.
  /// </summary>
  [Serializable]
  public class Library {
    // The name of the library.
    public string libraryName = "New Library";

    public int libraryId = -1;

    // The floors contained in this library. May not be loaded, in which case this
    // variable has value null.
    public List<Floor> floors;


    /// <summary>
    /// Serializes the whole floor to a single JSON object.
    /// </summary>
    /// <returns>The JSO.</returns>
    public JSONNode ToJSON() {
      JSONObject root = new JSONObject();
      root["library_id"] = libraryId;
      root["library_name"] = libraryName;

      return root;
    }

    /// <summary>
    /// Overwrites this floor's content with JSON object root.
    /// </summary>
    /// <param name="root">Root.</param>
    public void FromJSON(JSONNode root) {
      libraryId = root["library_id"];
      libraryName = root["library_name"];

      if (root["floors"] != null) {
        floors = new List<Floor>();

        // Parse the floors here
        foreach (JSONNode node in root["floors"].AsArray) {
          Floor f = new Floor();
          f.FromJSON(null, node);
          floors.Add(f);
        }
      }
    }
  }
}