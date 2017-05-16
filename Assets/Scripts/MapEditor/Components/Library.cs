using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
  }
}