using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace StackMaps {
  /// <summary>
  /// This takes care of coordinating the editing area, handling object CRUD
  /// operations and updating the toolbar and the sidebar.
  /// </summary>
  public class EditAreaController : MonoBehaviour {
    // The snapping grid size
    public float snapGridSize = 20.0f;
    public float snapAngleSize = 15.0f;

    // The canvas where things are actually on the map.
    public RectTransform canvas;

    // The scroll rect of the edit area.
    public ScrollRect scrollRect;

    // The toolbar associated with the editing area.
    public ToolbarController toolbarController;

    // The sidebar associated with the editing area.
    public SidebarController sidebarController;

    // The transform editor.
    public TransformEditor transformEditor;

    // The grid line controller
    public GridController gridController;

    // The floor controller, a data-heavy class dealing with representation and
    // serialization of the floor.
    public FloorController floorController;

    public Texture2D cursorCrosshair;
    public Texture2D cursorPan;

    // The point where user pressed the mouse.
    Vector3 mouseDownPos;
    bool dragInitiated;

    // The cursor id.
    int crosshairId = 99;

    // Selection tool
    public GameObject _selectedObject;

    GameObject GetSelectedObject() {
      return _selectedObject;
    }

    void SetSelectedObject(GameObject obj) {
      _selectedObject = obj;
    }

    // Use this for initialization
    void Start() {
      Input.multiTouchEnabled = false;
      Selectable.delegates.Add(ProcessSelection);
      transformEditor.snapGridSize = snapGridSize;
      transformEditor.snapAngleSize = snapAngleSize;
      gridController.gridSize = snapGridSize;
      gridController.UpdateGrid(canvas.sizeDelta);
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
      toolbarController.toolbar.deleteButton.interactable = GetSelectedObject() != null;

      if (toolbarController.toolbar.GetActiveTool() != ToolType.SelectionTool) {
        // Unselect if we changed tool.
        ProcessSelection(null);
      }

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

      transformEditor.canvasScale = canvas.localScale.x;
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
        CursorController.PopCursor(crosshairId);
        return;
      }

      // Transform editor is responsible for setting the cursor. Return.
      if (transformEditor.IsDragging()) {
        return;
      }

      ToolType current = toolbarController.toolbar.GetActiveTool();

      if (current != ToolType.SelectionTool) {
        if (CursorController.GetCurrentId() < crosshairId)
          crosshairId = CursorController.PushCursor(cursorCrosshair, new Vector2(17.5f, 17.5f));
      } else {
        CursorController.PopCursor(crosshairId);
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
      // When a selectable object is clicked we need to be informed. If a click
      // happened when nothing is selected, we dismiss.
    }

    /// <summary>
    /// Processes the input for the aisle tool.
    /// </summary>
    void ProcessInputForAisleTool() {
      if (dragInitiated) {
        Rect r;
        bool completed = ProcessInputForRectangleCreation(out r);
        floorController.CreateAisle(r, !completed);
      }
    }

    /// <summary>
    /// Processes the input for the aisle area tool.
    /// </summary>
    void ProcessInputForAisleAreaTool() {
      if (dragInitiated) {
        Rect r;
        bool completed = ProcessInputForRectangleCreation(out r);
        floorController.CreateAisleArea(r, !completed);
      }
    }

    /// <summary>
    /// Processes the input for the wall tool.
    /// </summary>
    void ProcessInputForWallTool() {
      if (dragInitiated) {
        bool completed = Input.GetMouseButtonUp(0);
        Vector2 begin = SnapToGrid(canvas.InverseTransformPoint(mouseDownPos));
        Vector2 end = SnapToGrid(canvas.InverseTransformPoint(Input.mousePosition));
        floorController.CreateWall(begin, end, !completed);
      }
    }

    /// <summary>
    /// Processes the input for the landmark tool.
    /// </summary>
    void ProcessInputForLandmarkTool() {
      if (dragInitiated) {
        Rect r;
        bool completed = ProcessInputForRectangleCreation(out r);
        floorController.CreateLandmark(r, !completed);
      }
    }

    /// <summary>
    /// Processes the input for creating a rectangle. Returns true if rectangle is
    /// completed. The rectangle is stored in the out parameter output.
    /// </summary>
    bool ProcessInputForRectangleCreation(out Rect rect) {
      rect = Rect.zero;

      if (!dragInitiated) {
        return false;
      }

      Vector2 p1 = SnapToGrid(canvas.InverseTransformPoint(Input.mousePosition));
      Vector2 p2 = SnapToGrid(canvas.InverseTransformPoint(mouseDownPos));

      rect = Rect.MinMaxRect(
        Mathf.Min(p1.x, p2.x), Mathf.Min(p1.y, p2.y), 
        Mathf.Max(p1.x, p2.x), Mathf.Max(p1.y, p2.y)
      );

      return Input.GetMouseButtonUp(0);
    }

    Vector2 SnapToGrid(Vector2 p) {
      return new Vector2(Mathf.Round(p.x / snapGridSize), Mathf.Round(p.y / snapGridSize)) * snapGridSize;
    }

    /// <summary>
    /// Processes a clicked game object.
    /// </summary>
    void ProcessSelection(GameObject clicked) {
      if (toolbarController.toolbar.GetActiveTool() != ToolType.SelectionTool) {
        clicked = null;
      }

      if (clicked != null && clicked == GetSelectedObject()) {
        return;
      }

      if (clicked == scrollRect.gameObject) {
        SetSelectedObject(null);
      } else {
        SetSelectedObject(clicked);
      }

      sidebarController.propertyEditor.SetSelectedObject(GetSelectedObject());
      transformEditor.SetEditingObject(GetSelectedObject());
    }

    /// <summary>
    /// Deletes the currently selected object.
    /// </summary>
    public void OnDeleteButtonPress() {
      if (GetSelectedObject() == null) {
        return;
      }

      floorController.DeleteObject(GetSelectedObject());
      ProcessSelection(null);

      ActionManager.shared.Push();
    }

    /// <summary>
    /// Undoes last action.
    /// </summary>
    public void OnUndoButtonPress() {
      ActionManager.shared.Undo();
      ProcessSelection(null);
    }

    /// <summary>
    /// Redoes last action.
    /// </summary>
    public void OnRedoButtonPress() {
      ActionManager.shared.Redo();
      ProcessSelection(null);
    }
  }
}
