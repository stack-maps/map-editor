using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;

namespace StackMaps {
  /// <summary>
  /// This manages the aisle area sub-editor of the property editor.
  /// </summary>
  public class AisleAreaEditor : SidebarElement {
    public InputField rowsInputField;
    public MaterialDropdown orientationDropdown;
    public Aisle aislePrefab;

    AisleArea editingObject;

    /// <summary>
    /// Sets up the script according to the given object. If we can edit it,
    /// then we update our values. Otherwise hide this panel.
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SetEditingObject(GameObject obj) {
      if (obj == null) {
        editingObject = null;
      } else {
        editingObject = obj.GetComponent<AisleArea>();
      }

      PopulateObject();
    }

    /// <summary>
    /// Returns the currently editing game object.
    /// </summary>
    /// <returns>The editing object.</returns>
    public GameObject GetEditingObject() {
      return editingObject == null ? null : editingObject.gameObject;
    }

    /// <summary>
    /// Populates the editor with editing object's values.
    /// </summary>
    void PopulateObject() {
      if (editingObject == null) {
        return;
      }

      rowsInputField.text = editingObject.aisles.Count.ToString();
      orientationDropdown.currentlySelected = editingObject.IsHorizontal() ? 0 : 1;
    }

    /// <summary>
    /// Updates the object with editor values.
    /// </summary>
    public void UpdateObject() {
      bool newVal = orientationDropdown.currentlySelected == 0;
      bool changed = editingObject.IsHorizontal() != newVal;

      if (changed) {
        editingObject.Flip();
        ActionManager.shared.Push();
        TransformEditor.shared.UpdateTransform();
      }
    }

    /// <summary>
    /// Raises the generate button press event.
    /// </summary>
    public void OnGenerateButtonPress() {
      if (editingObject == null) {
        return;
      }

      // Create a bunch of stacks in this area, I guess.
      int desired = int.Parse(rowsInputField.text);
      int current = editingObject.aisles.Count;

      // If desired is more than current, add as many as we should, rearranging
      // the gap between two as needed.
      for (int i = 0; desired > current + i; i++) {
        Aisle obj = Instantiate(aislePrefab, editingObject.container);
        editingObject.aisles.Add(obj);
        obj.GetComponent<Rectangle>().disableEditing = true;
      }
 
      // Otherwise we remove them.
      for (int i = 0; current - i > desired; i++) {
        Aisle obj = editingObject.aisles[desired];
        editingObject.aisles.RemoveAt(desired);
        Destroy(obj.gameObject);
      }

      if (desired != current) {
        ActionManager.shared.Push();
      }
    }
  }
}
