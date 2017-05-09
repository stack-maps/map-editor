using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    toolbar.cursorButton.interactable = isEnabled;
    toolbar.aisleButton.interactable = isEnabled;
    toolbar.aisleAreaButton.interactable = isEnabled;
    toolbar.wallButton.interactable = isEnabled;
    toolbar.landmarkButton.interactable = isEnabled;
    toolbar.undoButton.interactable = isEnabled;
    toolbar.redoButton.interactable = isEnabled;
    toolbar.referenceImageButton.interactable = isEnabled;
    toolbar.zoomButton.interactable = isEnabled;
  }
}
