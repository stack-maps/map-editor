using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;

namespace StackMaps {
  /// <summary>
  /// This manages the aisle sub-editor of the property editor.
  /// </summary>
  public class AisleEditor : SidebarElement {
    /// <summary>
    /// The switch determining whether stack is single or double-sided.
    /// </summary>
    public MaterialSwitch sidednessSwitch;

    /// <summary>
    /// Prefab for creating a call number range editor.
    /// </summary>
    public CallNumberRangeEditor callNumberRangeEditorPrefab;

    /// <summary>
    /// Currently editing component.
    /// </summary>
    Aisle editingObject;

    /// <summary>
    /// List of editors, one for each call number range.
    /// </summary>
    List<CallNumberRangeEditor> rangeEditors = new List<CallNumberRangeEditor>();

    /// <summary>
    /// Sets up the script according to the given object. If we can edit it,
    /// then we update our values. Otherwise hide this panel.
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SetEditingObject(GameObject obj) {
      if (obj == null) {
        editingObject = null;
      } else {
        editingObject = obj.GetComponent<Aisle>();
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

      sidednessSwitch.toggle.isOn = !editingObject.singleSided;

      DestroyEditors();

      for (int i = 0; i < editingObject.callNumberRanges.Count; i++) {
        CallNumberRange range = editingObject.callNumberRanges[i];
        CallNumberRangeEditor editor = Instantiate(callNumberRangeEditorPrefab, transform);

        editor.onDelete = () => {
          editingObject.callNumberRanges.Remove(range);
          editor.Delete();
        };

        rangeEditors.Add(editor);
        editor.SetEditingObject(range);
      }
    }

    /// <summary>
    /// Removes all call number range editors.
    /// </summary>
    void DestroyEditors() {
      while (rangeEditors.Count > 0) {
        CallNumberRangeEditor r = rangeEditors[0];
        rangeEditors.RemoveAt(0);
        Destroy(r.gameObject);
      }
    }

    /// <summary>
    /// Updates the editing object with this editor's values.
    /// </summary>
    public void UpdateObject() {
      if (editingObject == null) {
        return;
      }

      bool changed = editingObject.singleSided != !sidednessSwitch.toggle.isOn;
      editingObject.singleSided = !sidednessSwitch.toggle.isOn;

      if (changed) {
        ActionManager.shared.Push();

        foreach (CallNumberRangeEditor editor in rangeEditors) {
          CanvasGroup g = editor.sideDropdown.GetComponent<CanvasGroup>();
          g.interactable = !editingObject.singleSided;
          //g.blocksRaycasts = editingObject.singleSided;
          TweenManager.TweenFloat(v => g.alpha = v, g.alpha, editingObject.singleSided ? 0 : 1, 0.3f);
        }
      }
    }

    /// <summary>
    /// Adds a new range to this object.
    /// </summary>
    public void AddCallNumberRange() {
      if (editingObject == null) {
        return;
      }

      CallNumberRange range = new CallNumberRange();
      editingObject.callNumberRanges.Add(range);
      CallNumberRangeEditor editor = Instantiate(callNumberRangeEditorPrefab, transform);
      rangeEditors.Add(editor);

      editor.onDelete = () => {
        editingObject.callNumberRanges.Remove(range);
        editor.Delete();
        ActionManager.shared.Push();
      };

      editor.Show(false, false);
      editor.Show(true, true);
      editor.SetEditingObject(range);

      ActionManager.shared.Push();
    }
  }
}
