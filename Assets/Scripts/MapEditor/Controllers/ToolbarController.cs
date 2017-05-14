using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This controls the toolbar of the editor.
  /// </summary>
  public class ToolbarController : MonoBehaviour {
    // The toolbar we are controlling.
    public Toolbar toolbar;

    /// <summary>
    /// Sets whether user can press anything on the toolbar.
    /// </summary>
    /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
    public void SetEnabled(bool isEnabled) {
      transform.GetChild(0).gameObject.SetActive(isEnabled);
    }
  }
}