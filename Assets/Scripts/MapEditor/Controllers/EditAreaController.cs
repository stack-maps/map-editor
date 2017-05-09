﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This takes care of coordinating the editing area, handling object CRUD
/// operations and updating the toolbar and the sidebar.
/// </summary>
public class EditAreaController : MonoBehaviour {
  // The canvas where things are actually on the map.
  public RectTransform canvas;

  // The scroll rect of the edit area.
  public ScrollRect scrollRect;

  // The toolbar associated with the editing area.
  public ToolbarController toolbarController;

  public Texture2D cursorCrosshair;
  public Texture2D cursorPan;

  // The point where user pressed the mouse.
  Vector3 mouseDownPos;
  bool dragInitiated;

  // Use this for initialization
  void Start() {
  }
	
  // Update is called once per frame
  void Update() {
    // Mouse-related updates
    // First some bookkeeping on mouse that is very useful
    if (Input.GetMouseButtonDown(0) && IsMouseInEditingArea()) {
      mouseDownPos = Input.mousePosition;
      dragInitiated = true;
    }

    ToggleScrolling(toolbarController.toolbar.GetActiveTool() == ToolType.SelectionTool);
    ToggleCursor();

    // Tool-related updates
    ProcessInputForTools();

    // Finally reset drag
    if (Input.GetMouseButtonUp(0)) {
      dragInitiated = false;
    }
  }

  // These levels of zoom are arbitrary and can be changed any time.
  readonly float[] zoomLevels = { 0.25f, 0.33f, 0.5f, 0.75f, 1, 2, 4 };

  /// <summary>
  /// Updates the zoom level of the canvas.
  /// </summary>
  /// <param name="zoomLevel">Zoom level.</param>
  public void UpdateZoom(int zoomLevel) {
    if (zoomLevel < 0) {
      // We didn't select anything. do nothing.
      return;
    } else if (zoomLevel < zoomLevels.Length) {
      canvas.localScale = new Vector3(zoomLevels[zoomLevel], zoomLevels[zoomLevel], 1);
    } else if (zoomLevel == zoomLevels.Length) {
      // We assume we want fit width here. Do some calculations.
      // We want to scale the view so the canvas sits at 90% of the view.
      float ratio = scrollRect.viewport.rect.width * 0.9f / canvas.rect.width;
      canvas.localScale = new Vector3(ratio, ratio, 1);
    } else {
      // We assume we want fit window here. Do some calculations.
      // We want to scale the view so the canvas sits at 90% of the view.
      float ratio1 = scrollRect.viewport.rect.width * 0.9f / canvas.rect.width;
      float ratio2 = scrollRect.viewport.rect.height * 0.9f / canvas.rect.height;
      float ratio = Mathf.Min(ratio1, ratio2);
      canvas.localScale = new Vector3(ratio, ratio, 1);
    }
  }

  /// <summary>
  /// Toggles the ability to scroll on the canvas.
  /// </summary>
  /// <param name="canScroll">If set to <c>true</c>, user can scroll.</param>
  public void ToggleScrolling(bool canScroll) {
    scrollRect.horizontal = canScroll;
    scrollRect.vertical = canScroll;
  }

  /// <summary>
  /// Determines whether user's mouse pointer is in the editing area.
  /// </summary>
  bool IsMouseInEditingArea() {
    Vector2 local = scrollRect.viewport.InverseTransformPoint(Input.mousePosition);
    return scrollRect.viewport.rect.Contains(local);
  }

  /// <summary>
  /// Replaces the default cursor image depending on the current editing state
  /// and tool.
  /// </summary>
  void ToggleCursor() {
    if (!IsMouseInEditingArea()) {
      Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
      return;
    }

    ToolType current = toolbarController.toolbar.GetActiveTool();
    float delta = (Input.mousePosition - mouseDownPos).magnitude;

    if (current == ToolType.SelectionTool && dragInitiated && delta > 0) {
      Cursor.SetCursor(cursorPan, new Vector2(17.5f, 17.5f), CursorMode.Auto);
    } else if (current != ToolType.SelectionTool) {
      Cursor.SetCursor(cursorCrosshair, new Vector2(17.5f, 17.5f), CursorMode.Auto);
    } else {
      Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
  }

  /// <summary>
  /// Look at all the input this frame and grab any useful ones for each tool.
  /// </summary>
  void ProcessInputForTools() {
    switch (toolbarController.toolbar.GetActiveTool()) {
      case ToolType.SelectionTool:
        ProcessInputForSelectionTool();
        break;
      case ToolType.AisleTool:
        ProcessInputForAisleTool();
        break;
      case ToolType.AisleAreaTool:
        ProcessInputForAisleAreaTool();
        break;
      case ToolType.WallTool:
        ProcessInputForWallTool();
        break;
      case ToolType.LandmarkTool:
        ProcessInputForLandmarkTool();
        break;
    }
  }

  /// <summary>
  /// Processes the input for the selection tool.
  /// </summary>
  void ProcessInputForSelectionTool() {
    
  }

  /// <summary>
  /// Processes the input for the aisle tool.
  /// </summary>
  void ProcessInputForAisleTool() {
    Rect r;

    if (ProcessInputForRectangleCreation(out r)) {
      // Create the object using r.
      Debug.Log(r);
    }
  }

  /// <summary>
  /// Processes the input for the aisle area tool.
  /// </summary>
  void ProcessInputForAisleAreaTool() {
    Rect r;

    if (ProcessInputForRectangleCreation(out r)) {
      // Create the object using r.
      Debug.Log(r);
    }
  }

  /// <summary>
  /// Processes the input for the wall tool.
  /// </summary>
  void ProcessInputForWallTool() {
    Rect r;

    if (ProcessInputForRectangleCreation(out r)) {
      // Create the object using r.
      Debug.Log(r);
    }
  }

  /// <summary>
  /// Processes the input for the landmark tool.
  /// </summary>
  void ProcessInputForLandmarkTool() {
    Rect r;

    if (ProcessInputForRectangleCreation(out r)) {
      // Create the object using r.
      Debug.Log(r);
    }
  }

  /// <summary>
  /// Processes the input for creating a rectangle. Returns true if rectangle is
  /// completed and the rectangle is stored in the out parameter output.
  /// </summary>
  bool ProcessInputForRectangleCreation(out Rect rect) {
    rect = new Rect();

    if (!dragInitiated) {
      return false;
    }

    if (Input.GetMouseButtonUp(0)) {
      // User released the mouse! We now know our rectangle.
      float x1 = Input.mousePosition.x;
      float y1 = Input.mousePosition.y;
      float x2 = mouseDownPos.x;
      float y2 = mouseDownPos.y;

      rect = Rect.MinMaxRect(
        Mathf.Min(x1, x2), Mathf.Min(y1, y2), 
        Mathf.Max(x1, x2), Mathf.Max(y1, y2)
      );
    }

    return Input.GetMouseButtonUp(0);
  }
}
