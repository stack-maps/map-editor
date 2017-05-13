using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

namespace StackMaps {
  /// <summary>
  /// This class takes care of loading a floor (populating the canvas), editing a
  /// loaded floor, and exporting the floor to an appropriate format for upload.
  /// </summary>
  public class FloorController : MonoBehaviour {

    // Prefabs for instantiation
    public Aisle aislePrefab;
    public AisleArea aisleAreaPrefab;
    public Wall wallPrefab;
    public Landmark landmarkPrefab;

    // Layers to put the objects in
    public RectTransform aisleLayer;
    public RectTransform aisleAreaLayer;
    public RectTransform wallLayer;
    public RectTransform landmarkLayer;
    public RectTransform previewLayer;

    /// <summary>
    /// The data structure holding floor information, capable of serialization.
    /// </summary>
    Floor floor = new Floor();

    /// <summary>
    /// The preview object before creation.
    /// </summary>
    CanvasGroup previewObject;

    // TODO: these creation methods are kind of dumb with repetitive code. Think
    // about refactoring them at a later stage might be a good idea.


    /// <summary>
    /// Creates a new landmark and adds it to the floor.
    /// </summary>
    /// <param name="rect">Dimensions of the landmark.</param>
    /// <param name = "preview">Whether this is only a preview of the real object.</param>
    /// <param name = "isUndoRedo">Is this creation a result of undo/redo?</param>
    public Landmark CreateLandmark(Rect rect, bool preview, bool isUndoRedo = false) {
      if (preview) {
        // Destroys preview if it is of a different type
        if (previewObject != null && previewObject.GetComponent<Landmark>() == null) {
          ClearPreview();
        }

        // Create a preview object if it is not there already.
        if (previewObject == null) {
          previewObject = Instantiate(landmarkPrefab, previewLayer).GetComponent<CanvasGroup>();
          previewObject.alpha = 0.5f;
        }

        // Resize
        ((RectTransform)previewObject.transform).sizeDelta = rect.size;
        ((RectTransform)previewObject.transform).anchoredPosition = rect.center;

        return null;
      } else {
        // Clears the preview object.
        Landmark obj = Instantiate(landmarkPrefab, landmarkLayer);
        ((RectTransform)obj.transform).sizeDelta = rect.size;
        ((RectTransform)obj.transform).anchoredPosition = rect.center;
        floor.landmarks.Add(obj);
        ClearPreview();

        if (!isUndoRedo)
          ActionManager.shared.Push();

        return obj;
      }
    }

    /// <summary>
    /// Creates a new aisle area and adds it to the floor.
    /// </summary>
    /// <param name="rect">Dimensions of the aisle area.</param>
    /// <param name = "preview">Whether this is only a preview of the real object.</param>
    /// <param name = "isUndoRedo">Is this creation a result of undo/redo?</param>
    public AisleArea CreateAisleArea(Rect rect, bool preview, bool isUndoRedo = false) {
      if (preview) {
        // Destroys preview if it is of a different type
        if (previewObject != null && previewObject.GetComponent<AisleArea>() == null) {
          ClearPreview();
        }

        // Create a preview object if it is not there already.
        if (previewObject == null) {
          previewObject = Instantiate(aisleAreaPrefab, previewLayer).GetComponent<CanvasGroup>();
          previewObject.alpha = 0.5f;
        }

        // Resize
        ((RectTransform)previewObject.transform).sizeDelta = rect.size;
        ((RectTransform)previewObject.transform).anchoredPosition = rect.center;

        return null;
      } else {
        // Clears the preview object.
        AisleArea obj = Instantiate(aisleAreaPrefab, aisleAreaLayer);
        ((RectTransform)obj.transform).sizeDelta = rect.size;
        ((RectTransform)obj.transform).anchoredPosition = rect.center;
        floor.aisleAreas.Add(obj);
        ClearPreview();

        if (!isUndoRedo)
          ActionManager.shared.Push();

        return obj;
      }
    }

    /// <summary>
    /// Creates a new aisle and adds it to the floor.
    /// </summary>
    /// <param name="rect">Dimensions of the aisle.</param>
    /// <param name = "preview">Whether this is only a preview of the real object.</param>
    /// <param name = "isUndoRedo">Is this creation a result of undo/redo?</param>
    public Aisle CreateAisle(Rect rect, bool preview, bool isUndoRedo = false) {
      Aisle obj = null;

      if (preview) {
        // Destroys preview if it is of a different type
        if (previewObject != null && previewObject.GetComponent<Aisle>() == null) {
          ClearPreview();
        }

        // Create a preview object if it is not there already.
        if (previewObject == null) {
          previewObject = Instantiate(aislePrefab, previewLayer).GetComponent<CanvasGroup>();
          previewObject.alpha = 0.5f;
        }

        Rectangle t = previewObject.GetComponent<Rectangle>();
        bool shouldRotate = rect.width > rect.height;
        t.SetRotation(shouldRotate ? 90 : 0);
        t.SetSize(shouldRotate ? new Vector2(rect.height, rect.width) : rect.size);
        t.SetCenter(rect.center);
      } else {
        // Clears the preview object.
        obj = Instantiate(aislePrefab, aisleLayer);
        Rectangle t = obj.GetComponent<Rectangle>();
        floor.aisles.Add(obj);
        ClearPreview();

        bool shouldRotate = rect.width > rect.height;
        t.SetRotation(shouldRotate ? 90 : 0);
        t.SetSize(shouldRotate ? new Vector2(rect.height, rect.width) : rect.size);
        t.SetCenter(rect.center);

        if (!isUndoRedo)
          ActionManager.shared.Push();
      }

      // Resize - we do something more here. We want to rotate the aisle
      // depending on whichever side is longer.
      return obj;
    }

    /// <summary>
    /// Creates a wall between the given two points.
    /// </summary>
    /// <param name="begin">Begin.</param>
    /// <param name="end">End.</param>
    /// <param name="preview">If set to <c>true</c> preview.</param>
    /// <param name = "isUndoRedo">Is this creation a result of undo/redo?</param>
    public Wall CreateWall(Vector2 begin, Vector2 end, bool preview, bool isUndoRedo = false) {
      if (preview) {
        // Destroys preview if it is of a different type
        if (previewObject != null && previewObject.GetComponent<Wall>() == null) {
          ClearPreview();
        }

        // Create a preview object if it is not there already.
        if (previewObject == null) {
          previewObject = Instantiate(wallPrefab, previewLayer).GetComponent<CanvasGroup>();
          previewObject.alpha = 0.5f;
        }

        previewObject.GetComponent<Wall>().SetStart(begin);
        previewObject.GetComponent<Wall>().SetEnd(end);

        return null;
      } else {
        // Clears the preview object.
        Wall obj = Instantiate(wallPrefab, wallLayer);
        floor.walls.Add(obj);
        ClearPreview();
        obj.SetStart(begin);
        obj.SetEnd(end);

        if (!isUndoRedo)
          ActionManager.shared.Push();

        return obj;
      }
    }

    /// <summary>
    /// Destroys the previewing object immediately.
    /// </summary>
    public void ClearPreview() {
      if (previewObject != null)
        Destroy(previewObject.gameObject);
    }

    /// <summary>
    /// Deletes the given object from the floor.
    /// </summary>
    /// <param name="obj">Object to delete.</param>
    public void DeleteObject(GameObject obj) {
      floor.aisles.Remove(obj.GetComponent<Aisle>());
      floor.aisleAreas.Remove(obj.GetComponent<AisleArea>());
      floor.walls.Remove(obj.GetComponent<Wall>());
      floor.landmarks.Remove(obj.GetComponent<Landmark>());

      Destroy(obj);
    }

    public void ImportFloor(string floorJSON) {
      Debug.Log("IMPORT: " + floorJSON);
      JSONNode root = JSONNode.Parse(floorJSON);
      Debug.Log("Parsed: " + root.ToString());

      // Delete everything.
      for (int i = 0; i < aisleLayer.childCount; i++) {
        Destroy(aisleLayer.GetChild(i).gameObject);
      }

      for (int i = 0; i < aisleAreaLayer.childCount; i++) {
        Destroy(aisleAreaLayer.GetChild(i).gameObject);
      }

      for (int i = 0; i < wallLayer.childCount; i++) {
        Destroy(wallLayer.GetChild(i).gameObject);
      }

      for (int i = 0; i < landmarkLayer.childCount; i++) {
        Destroy(landmarkLayer.GetChild(i).gameObject);
      }

      floor.FromJSON(this, root);
    }

    public string ExportFloor() {
      JSONNode node = floor.ToJSON();
      Debug.Log("EXPORT: " + node.ToString());

      return node.ToString();
    }
  }
}
