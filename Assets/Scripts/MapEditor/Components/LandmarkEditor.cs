using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;

namespace StackMaps {
  /// <summary>
  /// This manages the landmark sub-editor of the property editor.
  /// </summary>
  public class LandmarkEditor : SidebarElement {
    public MaterialDropdown landmarkTypeDropdown;

    Landmark editingObject;

    /// <summary>
    /// Sets up the script according to the given object. If we can edit it,
    /// then we update our values. Otherwise hide this panel.
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SetEditingObject(GameObject obj) {
      if (obj == null) {
        editingObject = null;
      } else {
        editingObject = obj.GetComponent<Landmark>();
      }

      PopulateObject();
    }

    /// <summary>
    /// Returns the currently editing game object.
    /// </summary>
    /// <returns>The editing object.</returns>
    public GameObject GetEditingObject() {
      return editingObject == null? null : editingObject.gameObject;
    }

    /// <summary>
    /// Populates the editor with editing object's values.
    /// </summary>
    void PopulateObject() {
      if (editingObject == null) {
        return;
      }

      landmarkTypeDropdown.currentlySelected = (int)editingObject.landmarkType;
    }

    /// <summary>
    /// Updates the editing object with this editor's values.
    /// </summary>
    public void UpdateObject() {
      if (editingObject == null) {
        return;
      }

      LandmarkType newVal = (LandmarkType)landmarkTypeDropdown.currentlySelected;
      bool changed = editingObject.landmarkType != newVal;
      editingObject.landmarkType = newVal;

      if (changed) {
        ActionManager.shared.Push();
      }
    }
  }
}