using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StackMaps {
  /// <summary>
  /// This manages the rectangle sub-editor of the property editor.
  /// </summary>
  public class RectangleEditor : MonoBehaviour {
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
      }

      gameObject.SetActive(editingObject != null);
      PopulateObject();
    }

    /// <summary>
    /// Populates the editor with editing object's values.
    /// </summary>
    void PopulateObject() {
      if (editingObject == null) {
        return;
      }

      xInputField.text = editingObject.GetCenter().x.ToString("F");
      yInputField.text = editingObject.GetCenter().y.ToString("F");
      widthInputField.text = editingObject.GetSize().x.ToString("F");
      heightInputField.text = editingObject.GetSize().y.ToString("F");
      rotationInputField.text = editingObject.GetRotation().ToString("F");
    }

    /// <summary>
    /// Updates the editing object with this editor's values.
    /// </summary>
    public void UpdateObject() {
      if (editingObject == null) {
        return;
      }

      editingObject.SetCenter(new Vector2(float.Parse(xInputField.text), float.Parse(yInputField.text)));
      editingObject.SetSize(new Vector2(float.Parse(widthInputField.text), float.Parse(heightInputField.text)));
      editingObject.SetRotation(float.Parse(rotationInputField.text));
    }
  }
}
