using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;

namespace StackMaps {
  /// <summary>
  /// This manages how a library cell would look like in the library explorer.
  /// </summary>
  public class LibraryCell : SidebarElement {
    /// <summary>
    /// The button that will select us from the rest.
    /// </summary>
    public MaterialButton selectButton;

    /// <summary>
    /// Sets up this cell with the given library info.
    /// </summary>
    /// <param name="lib">Lib.</param>
    public void SetLibrary(Library lib) {
      selectButton.textText = lib.libraryName;
    }
  }
}