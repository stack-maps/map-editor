﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This manages the aisle area sub-editor of the property editor.
  /// </summary>
  public class AisleAreaEditor : MonoBehaviour {
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

      gameObject.SetActive(editingObject != null);
    }
  }
}
