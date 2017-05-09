using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This manages the property sub-editors in the sidebar.
  /// </summary>
  public class PropertyEditor : MonoBehaviour {
    /// <summary>
    /// The landmark editor.
    /// </summary>
    public LandmarkEditor landmarkEditor;

    /// <summary>
    /// The aisle editor.
    /// </summary>
    public AisleEditor aisleEditor;

    /// <summary>
    /// The aisle area editor.
    /// </summary>
    public AisleAreaEditor aisleAreaEditor;

    /// <summary>
    /// The rectangle editor.
    /// </summary>
    public RectangleEditor rectangleEditor;

    /// <summary>
    /// The wall editor.
    /// </summary>
    public WallEditor wallEditor;

    /// <summary>
    /// The default empty selection.
    /// </summary>
    public GameObject emptyEditor;

    void Start() {
      SetSelectedObject(null);
    }

    /// <summary>
    /// Updates the UI and populates the fields.
    /// </summary>
    /// <param name="selected">Selected object. We look into its components and
    /// load appropriate sub-editors.</param>
    public void SetSelectedObject(GameObject selected) {
      emptyEditor.SetActive(selected == null);
      landmarkEditor.SetEditingObject(selected);
      aisleEditor.SetEditingObject(selected);
      aisleAreaEditor.SetEditingObject(selected);
      wallEditor.SetEditingObject(selected);
      rectangleEditor.SetEditingObject(selected);
    }
  }
}