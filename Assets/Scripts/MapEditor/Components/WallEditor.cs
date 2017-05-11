using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StackMaps {
  /// <summary>
  /// This manages the wall sub-editor of the property editor.
  /// </summary>
  public class WallEditor : SidebarElement {
    Wall editingObject;

    public InputField x1InputField;
    public InputField x2InputField;
    public InputField y1InputField;
    public InputField y2InputField;

    /// <summary>
    /// Sets up the script according to the given object. If we can edit it,
    /// then we update our values. Otherwise hide this panel.
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SetEditingObject(GameObject obj) {
      if (obj == null) {
        editingObject = null;
      } else {
        editingObject = obj.GetComponent<Wall>();
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

      x1InputField.text = editingObject.GetStart().x.ToString();
      y1InputField.text = editingObject.GetStart().y.ToString();
      x2InputField.text = editingObject.GetEnd().x.ToString();
      y2InputField.text = editingObject.GetEnd().y.ToString();
    }


    /// <summary>
    /// Updates the editing object with this editor's values.
    /// </summary>
    public void UpdateObject() {
      if (editingObject == null) {
        return;
      }

      editingObject.SetStart(new Vector2(float.Parse(x1InputField.text),
        float.Parse(y1InputField.text)));
      editingObject.SetEnd(new Vector2(float.Parse(x2InputField.text),
        float.Parse(y2InputField.text)));
    }

  }
}
