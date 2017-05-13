using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StackMaps {
  /// <summary>
  /// This manages the rectangle sub-editor of the property editor.
  /// </summary>
  public class RectangleEditor : SidebarElement {
    Rectangle editingObject;

    public InputField xInputField;
    public InputField yInputField;
    public InputField widthInputField;
    public InputField heightInputField;
    public InputField rotationInputField;

    /// <summary>
    /// Sets up the script according to the given object. If we can edit it,
    /// then we update our values. Otherwise hide this panel.
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SetEditingObject(GameObject obj) {
      if (obj == null) {
        editingObject = null;
      } else {
        editingObject = obj.GetComponent<Rectangle>();

        if (editingObject != null && editingObject.disableEditing) {
          editingObject = null;
        }
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

      if (!xInputField.isFocused)
        xInputField.text = editingObject.GetCenter().x.ToString("F");

      if (!yInputField.isFocused)
        yInputField.text = editingObject.GetCenter().y.ToString("F");

      if (!widthInputField.isFocused)
        widthInputField.text = editingObject.GetSize().x.ToString("F");

      if (!heightInputField.isFocused)
        heightInputField.text = editingObject.GetSize().y.ToString("F");

      if (!rotationInputField.isFocused)
        rotationInputField.text = editingObject.GetRotation().ToString("F");
    }

    void Update() {
      PopulateObject();
    }

    /// <summary>
    /// Updates the editing object with this editor's values.
    /// </summary>
    public void UpdateObject() {
      if (editingObject == null) {
        return;
      }

      Vector2 newCenter = new Vector2(float.Parse(xInputField.text), float.Parse(yInputField.text));
      Vector2 newSize = new Vector2(float.Parse(widthInputField.text), float.Parse(heightInputField.text));
      float newRotation = float.Parse(rotationInputField.text);
      bool changed = !((newCenter - editingObject.GetCenter()).magnitude < 0.01f &&
                     (newSize - editingObject.GetSize()).magnitude < 0.01f &&
                     Mathf.Abs(newRotation - editingObject.GetRotation()) < 0.01f);
      editingObject.SetCenter(newCenter);
      editingObject.SetSize(newSize);
      editingObject.SetRotation(newRotation);

      if (changed) {
        ActionManager.shared.Push();
      }

      // Also tell transform to update too
      TransformEditor.shared.UpdateTransform();
    }
  }
}
