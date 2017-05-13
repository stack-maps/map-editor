using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;
using System;

namespace StackMaps {
  /// <summary>
  /// Handles custom editing for a call number range object.
  /// </summary>
  public class CallNumberRangeEditor : SidebarElement {
    CallNumberRange editingObject;

    public MaterialInputField startClassInputField;
    public MaterialInputField endClassInputField;
    public MaterialInputField startSubclassInputField;
    public MaterialInputField endSubclassInputField;
    public MaterialInputField startCutter1InputField;
    public MaterialInputField endCutter1InputField;
    public MaterialInputField startCutter2InputField;
    public MaterialInputField endCutter2InputField;

    public VectorImage warningIcon;

    public MaterialDropdown collectionDropdown;
    public MaterialDropdown sideDropdown;

    /// <summary>
    /// Defining a function delegate to be called when the delete button is
    /// pressed.
    /// </summary>
    public delegate void DeleteDelegate();

    /// <summary>
    /// The function to call on delete.
    /// </summary>
    public DeleteDelegate onDelete;

    /// <summary>
    /// Sets up the script according to the given object. If we can edit it,
    /// then we update our values.
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SetEditingObject(GameObject obj) {
      if (obj == null) {
        editingObject = null;
      } else {
        editingObject = obj.GetComponent<CallNumberRange>();
      }

      PopulateObject();
    }

    /// <summary>
    /// Sets up the script according to the given editable object.
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SetEditingObject(CallNumberRange obj) {
      editingObject = obj;
      PopulateObject();
    }

    /// <summary>
    /// Returns the currently editing game object.
    /// </summary>
    /// <returns>The editing object.</returns>
    public CallNumberRange GetEditingObject() {
      return editingObject;
    }

    /// <summary>
    /// Populates the editor with editing object's values.
    /// </summary>
    void PopulateObject() {
      if (editingObject == null) {
        return;
      }

      startClassInputField.inputField.text = editingObject.GetBegin().GetClass();
      startSubclassInputField.inputField.text = editingObject.GetBegin().GetSubclass();
      startCutter1InputField.inputField.text = editingObject.GetBegin().GetCutter1();
      startCutter2InputField.inputField.text = editingObject.GetBegin().GetCutter2();
      endClassInputField.inputField.text = editingObject.GetEnd().GetClass();
      endSubclassInputField.inputField.text = editingObject.GetEnd().GetSubclass();
      endCutter1InputField.inputField.text = editingObject.GetEnd().GetCutter1();
      endCutter2InputField.inputField.text = editingObject.GetEnd().GetCutter2();

      // TODO: this part seems a bit ugly. Can we do better here?
      int i = -1;

      if (CallNumberCollection.Regular.Equals(editingObject.collection)) {
        i = 0;
      } else if (CallNumberCollection.OversizedPlus.Equals(editingObject.collection)) {
        i = 1;
      } else if (CallNumberCollection.OversizedPlusPlus.Equals(editingObject.collection)) {
        i = 2;
      }

      collectionDropdown.currentlySelected = i;

      sideDropdown.currentlySelected = editingObject.isSideA ? 0 : 1;
    }

    void Update() {
      UpdateObject();
    }

    /// <summary>
    /// Updates the editing object with this editor's values.
    /// </summary>
    public void UpdateObject() {
      if (editingObject == null) {
        return;
      }

      // TODO: this part seems a bit ugly. Can we do better here?
      string collection = "";

      if (collectionDropdown.currentlySelected == 0) {
        collection = CallNumberCollection.Regular;
      } else if (collectionDropdown.currentlySelected == 1) {
        collection = CallNumberCollection.OversizedPlus;
      } else if (collectionDropdown.currentlySelected == 2) {
        collection = CallNumberCollection.OversizedPlusPlus;
      }

      editingObject.collection = collection;
      editingObject.isSideA = sideDropdown.currentlySelected == 0;

      // We check if all fields are properly set up. If not we don't update the
      // ranges.
      bool allValid = true;
      allValid &= !startClassInputField.validationText.isActiveAndEnabled;
      allValid &= !endClassInputField.validationText.isActiveAndEnabled;
      allValid &= !startSubclassInputField.validationText.isActiveAndEnabled;
      allValid &= !endSubclassInputField.validationText.isActiveAndEnabled;
      allValid &= !startCutter1InputField.validationText.isActiveAndEnabled;
      allValid &= !endCutter1InputField.validationText.isActiveAndEnabled;
      allValid &= !startCutter2InputField.validationText.isActiveAndEnabled;
      allValid &= !endCutter2InputField.validationText.isActiveAndEnabled;

      if (!allValid) {
        return;
      }

      // This is called every single time we edit the field.
      // We want to create new call numbers each time, to validate the input.
      CallNumber begin = new CallNumber();
      begin.SetClass(startClassInputField.inputField.text);
      begin.SetSubclass(startSubclassInputField.inputField.text);
      begin.SetCutter1(startCutter1InputField.inputField.text);
      begin.SetCutter2(startCutter2InputField.inputField.text);

      CallNumber end = new CallNumber();
      end.SetClass(endClassInputField.inputField.text);
      end.SetSubclass(endSubclassInputField.inputField.text);
      end.SetCutter1(endCutter1InputField.inputField.text);
      end.SetCutter2(endCutter2InputField.inputField.text);

      // Check if there is any change
      bool changed = !(begin.ToString().Equals(editingObject.GetBegin().ToString()) &&
                     end.ToString().Equals(editingObject.GetEnd().ToString()));

      allValid &= editingObject.SetBegin(begin);
      allValid &= editingObject.SetEnd(end);

      if (changed) {
        ActionManager.shared.Push();
      }

      warningIcon.gameObject.SetActive(!allValid);
    }

    /// <summary>
    /// We pressed the delete button!
    /// </summary>
    public void OnDeleteButtonPress() {
      if (onDelete != null)
        onDelete();
    }
  }
}
