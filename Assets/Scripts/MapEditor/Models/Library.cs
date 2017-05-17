using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace StackMaps {
  /// <summary>
  /// This class represents a library, containing floors.
  /// </summary>
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
      root["lid"] = libraryId;
      root["lname"] = libraryName;

      return root;
    }

    /// <summary>
    /// Overwrites this floor's content with JSON object root.
    /// </summary>
    /// <param name="root">Root.</param>
    public void FromJSON(JSONNode root) {
      libraryId = root["lid"];
      libraryName = root["lname"];

      if (root["Floor"] != null) {
        floors = new List<Floor>();

        // Parse the floors here
        foreach (JSONNode node in root["Floor"].AsArray) {
          Floor f = new Floor();
          f.FromJSON(null, node);
          floors.Add(f);
        }
      }
    }
  }
}