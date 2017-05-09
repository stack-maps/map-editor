using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
  /// Creates a new landmark and adds it to the floor.
  /// </summary>
  /// <param name="rect">Dimensions of the landmark.</param>
  public void CreateLandmark(Rect rect) {
    Landmark obj = Instantiate(landmarkPrefab, landmarkLayer);
    ((RectTransform)obj.transform).sizeDelta = rect.size;
    ((RectTransform)obj.transform).anchoredPosition = rect.center;
    landmarks.Add(obj);
  }

  /// <summary>
  /// Creates a new aisle area and adds it to the floor.
  /// </summary>
  /// <param name="rect">Dimensions of the aisle area.</param>
  public void CreateAisleArea(Rect rect) {
    AisleArea obj = Instantiate(aisleAreaPrefab, aisleAreaLayer);
    ((RectTransform)obj.transform).sizeDelta = rect.size;
    ((RectTransform)obj.transform).anchoredPosition = rect.center;
    aisleAreas.Add(obj);
  }

  /// <summary>
  /// Creates a new aisle and adds it to the floor.
  /// </summary>
  /// <param name="rect">Dimensions of the aisle.</param>
  public void CreateAisle(Rect rect) {
    Aisle obj = Instantiate(aislePrefab, aisleLayer);
    ((RectTransform)obj.transform).sizeDelta = rect.size;
    ((RectTransform)obj.transform).anchoredPosition = rect.center;
    aisles.Add(obj);
  }
}
