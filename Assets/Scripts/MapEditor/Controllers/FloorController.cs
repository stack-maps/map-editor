using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// The list of aisles on this floor.
    /// </summary>
    List<Aisle> aisles = new List<Aisle>();

    /// <summary>
    /// The list of aisle areas on this floor.
    /// </summary>
    List<AisleArea> aisleAreas = new List<AisleArea>();

    /// <summary>
    /// The list of walls on this floor.
    /// </summary>
    List<Wall> walls = new List<Wall>();

    /// <summary>
    /// The list of landmarks on this floor.
    /// </summary>
    List<Landmark> landmarks = new List<Landmark>();

    /// <summary>
    /// The preview object before creation.
    /// </summary>
    CanvasGroup previewObject;

    /// <summary>
    /// Creates a new landmark and adds it to the floor.
    /// </summary>
    /// <param name="rect">Dimensions of the landmark.</param>
    /// <param name = "preview">Whether this is only a preview of the real object.</param>
    public void CreateLandmark(Rect rect, bool preview) {
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
      } else {
        // Clears the preview object.
        Landmark obj = Instantiate(landmarkPrefab, landmarkLayer);
        ((RectTransform)obj.transform).sizeDelta = rect.size;
        ((RectTransform)obj.transform).anchoredPosition = rect.center;
        landmarks.Add(obj);
        ClearPreview();
      }
    }

    /// <summary>
    /// Creates a new aisle area and adds it to the floor.
    /// </summary>
    /// <param name="rect">Dimensions of the aisle area.</param>
    /// <param name = "preview">Whether this is only a preview of the real object.</param>
    public void CreateAisleArea(Rect rect, bool preview) {
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
      } else {
        // Clears the preview object.
        AisleArea obj = Instantiate(aisleAreaPrefab, aisleAreaLayer);
        ((RectTransform)obj.transform).sizeDelta = rect.size;
        ((RectTransform)obj.transform).anchoredPosition = rect.center;
        aisleAreas.Add(obj);
        ClearPreview();
      }
    }

    /// <summary>
    /// Creates a new aisle and adds it to the floor.
    /// </summary>
    /// <param name="rect">Dimensions of the aisle.</param>
    /// <param name = "preview">Whether this is only a preview of the real object.</param>
    public void CreateAisle(Rect rect, bool preview) {
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

        // Resize
        ((RectTransform)previewObject.transform).sizeDelta = rect.size;
        ((RectTransform)previewObject.transform).anchoredPosition = rect.center;
      } else {
        // Clears the preview object.
        Aisle obj = Instantiate(aislePrefab, aisleLayer);
        ((RectTransform)obj.transform).sizeDelta = rect.size;
        ((RectTransform)obj.transform).anchoredPosition = rect.center;
        aisles.Add(obj);
        ClearPreview();
      }
    }

    /// <summary>
    /// Destroys the previewing object immediately.
    /// </summary>
    public void ClearPreview() {
      if (previewObject != null)
        Destroy(previewObject.gameObject);
    }
  }
}
