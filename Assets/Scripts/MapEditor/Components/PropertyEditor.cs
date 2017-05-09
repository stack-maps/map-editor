using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This manages the property sub-editors in the sidebar.
  /// </summary>
  public class PropertyEditor : MonoBehaviour {

    public ExpandButton expandButton;

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
    public SidebarElement emptyEditor;

    GameObject selectedObject;

    void Start() {
      SetSelectedObject(null, false);
    }

    public void OnExpandButtonPress() {
      SetSelectedObject(selectedObject, true);
    }

    /// <summary>
    /// Updates the UI and populates the fields.
    /// </summary>
    /// <param name="selected">Selected object. We look into its components and
    /// load appropriate sub-editors.</param>
    /// <param name = "animated">Whether this is animated</param>
    public void SetSelectedObject(GameObject selected, bool animated = true) {
      selectedObject = selected;

      emptyEditor.Show(expandButton.isExpanded && selected == null, animated);
      landmarkEditor.SetEditingObject(selected);
      landmarkEditor.Show(expandButton.isExpanded &&
      landmarkEditor.GetEditingObject() != null, animated);
      aisleEditor.SetEditingObject(selected);
      aisleEditor.Show(expandButton.isExpanded &&
      aisleEditor.GetEditingObject() != null, animated);
      aisleAreaEditor.SetEditingObject(selected);
      aisleAreaEditor.Show(expandButton.isExpanded &&
      aisleAreaEditor.GetEditingObject() != null, animated);
      wallEditor.SetEditingObject(selected);
      wallEditor.Show(expandButton.isExpanded &&
      wallEditor.GetEditingObject() != null, animated);
      rectangleEditor.SetEditingObject(selected);
      rectangleEditor.Show(expandButton.isExpanded &&
      rectangleEditor.GetEditingObject() != null, animated);
    }
  }
}