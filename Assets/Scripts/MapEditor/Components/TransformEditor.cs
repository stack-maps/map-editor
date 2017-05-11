using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StackMaps {
  /// <summary>
  /// This controls the transform widget attached to the currently selected
  /// object, allowing it to be resized, rotated and moved.
  /// </summary>
  public class TransformEditor : MonoBehaviour {
    /// <summary>
    /// The list of handles to transform the editing object.
    /// </summary>
    public GameObject rotationHandle;
    public GameObject resizeHandleTL;
    public GameObject resizeHandleT;
    public GameObject resizeHandleTR;
    public GameObject resizeHandleL;
    public GameObject resizeHandleR;
    public GameObject resizeHandleBL;
    public GameObject resizeHandleB;
    public GameObject resizeHandleBR;

    public Image translateHandleGraphics;


    Rectangle editingObject;

    Wall editingWall;

    /// <summary>
    /// Sets up the script according to the given object. We switch modes
    /// automatically between a rectangle editor and a wall editor.
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SetEditingObject(GameObject obj) {
      if (obj == null) {
        editingObject = null;
      } else {
        editingObject = obj.GetComponent<Rectangle>();
        editingWall = obj.GetComponent<Wall>();
      }

      SwitchMode();
      Update();
    }

    /// <summary>
    /// Depending on which object we are editing, we switch visibility modes.
    /// </summary>
    void SwitchMode() {
      gameObject.SetActive(editingObject != null || editingWall != null);

      if (editingObject != null) {
        
        rotationHandle.SetActive(true);
        resizeHandleTL.SetActive(true);
        resizeHandleT.SetActive(true);
        resizeHandleTR.SetActive(true);
        resizeHandleBL.SetActive(true);
        resizeHandleB.SetActive(true);
        resizeHandleBR.SetActive(true);

        translateHandleGraphics.color = new Color32(0, 0, 0, 0x40);

        ((RectTransform)transform).pivot = new Vector2(0.5f, 0.5f);
      } else if (editingWall != null) {
        rotationHandle.SetActive(false);
        resizeHandleTL.SetActive(false);
        resizeHandleT.SetActive(false);
        resizeHandleTR.SetActive(false);
        resizeHandleBL.SetActive(false);
        resizeHandleB.SetActive(false);
        resizeHandleBR.SetActive(false);

        // We want to disable the graphics of the translate.
        translateHandleGraphics.color = Color.clear;

        // We want to adjust pivot so this is the same as wall.
        ((RectTransform)transform).pivot = new Vector2(0, 0.5f);
      }
    }

    /// <summary>
    /// Update this instance, we simply conform to the editing object's size.
    /// </summary>
    void Update() {
      if (editingObject != null) {
        ((RectTransform)transform).anchoredPosition = editingObject.GetCenter();
        ((RectTransform)transform).sizeDelta = editingObject.GetSize();
        transform.localEulerAngles = new Vector3(0, 0, editingObject.GetRotation());
      } else if (editingWall != null) {
        RectTransform t = editingWall.transform as RectTransform;
        Rect r = t.rect;
        ((RectTransform)transform).anchoredPosition = t.anchoredPosition;
        ((RectTransform)transform).sizeDelta = t.sizeDelta;
        transform.localEulerAngles = t.localEulerAngles;
      }
    }
  }
}