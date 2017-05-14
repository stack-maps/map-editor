using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This controls the sidebar of the application.
  /// </summary>
  public class SidebarController : MonoBehaviour {
    /// <summary>
    /// The library explorer on the toolbar.
    /// </summary>
    public LibraryExplorer libraryExplorer;

    /// <summary>
    /// The property editor below the library explorer.
    /// </summary>
    public PropertyEditor propertyEditor;
  }
}