﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace StackMaps {
  /// <summary>
  /// This controls the transform widget attached to the currently selected
  /// object, allowing it to be resized, rotated and moved.
  ///
  /// The implementation relies on TransformDrag components attached to various
  /// handles. In this script we specify what each handle should do.
  /// </summary>
  public class TransformEditor : MonoBehaviour {
    /// <summary>
    /// There will be update transform calls and this makes things easier.
    /// </summary>
    public static TransformEditor shared;

    /// <summary>
    /// The list of handles to transform the editing object.
    /// </summary>
    public TransformDrag rotationHandle;
    public TransformDrag translationHandle;
    public TransformDrag resizeHandleTL;
    public TransformDrag resizeHandleT;
    public TransformDrag resizeHandleTR;
    public TransformDrag resizeHandleL;
    public TransformDrag resizeHandleR;
    public TransformDrag resizeHandleBL;
    public TransformDrag resizeHandleB;
    public TransformDrag resizeHandleBR;

    public Image translateHandleGraphics;

    /// <summary>
    /// The current canvas scale. This is required since the screen-coordinates
    /// are not necessarily to be the same as the canvas coordinates.
    /// </summary>
    public float canvasScale = 1;

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
    }

    /// <summary>
    /// Depending on which object we are editing, we switch visibility modes.
    /// </summary>
    void SwitchMode() {
      gameObject.SetActive(editingObject != null || editingWall != null);

      if (editingObject != null) {
        
        rotationHandle.gameObject.SetActive(true);
        resizeHandleTL.gameObject.SetActive(true);
        resizeHandleT.gameObject.SetActive(true);
        resizeHandleTR.gameObject.SetActive(true);
        resizeHandleBL.gameObject.SetActive(true);
        resizeHandleB.gameObject.SetActive(true);
        resizeHandleBR.gameObject.SetActive(true);

        translateHandleGraphics.color = new Color32(0, 0, 0, 0x40);

        ((RectTransform)transform).pivot = new Vector2(0.5f, 0.5f);

        // Set up the editor size for once.
        UpdateTransform();
      } else if (editingWall != null) {
        rotationHandle.gameObject.SetActive(false);
        resizeHandleTL.gameObject.SetActive(false);
        resizeHandleT.gameObject.SetActive(false);
        resizeHandleTR.gameObject.SetActive(false);
        resizeHandleBL.gameObject.SetActive(false);
        resizeHandleB.gameObject.SetActive(false);
        resizeHandleBR.gameObject.SetActive(false);

        // We want to disable the graphics of the translate.
        translateHandleGraphics.color = Color.clear;

        // We want to adjust pivot so this is the same as wall.
        ((RectTransform)transform).pivot = new Vector2(0, 0.5f);

        // Set up the editor size for once.
        UpdateTransform();
      }
    }

    /// <summary>
    /// Sets up the center, size and rotation according to the editing object.
    /// </summary>
    public void UpdateTransform() {
      if (editingObject != null) {
        transform.localPosition = editingObject.GetCenter();
        ((RectTransform)transform).sizeDelta = editingObject.GetSize();
        transform.localEulerAngles = new Vector3(0, 0, editingObject.GetRotation());
      } else if (editingWall != null) {
        RectTransform t = editingWall.transform as RectTransform;
        ((RectTransform)transform).anchoredPosition = t.anchoredPosition;
        ((RectTransform)transform).sizeDelta = t.sizeDelta;
        transform.localEulerAngles = t.localEulerAngles;
      }
    }

    /// <summary>
    /// Defines all handle actions.
    /// </summary>
    void Start() {
      shared = this;

      translationHandle.dragHandler = data => {
        Vector2 delta = data.delta / canvasScale / FindObjectOfType<Canvas>().scaleFactor;

        if (editingObject != null) {
          editingObject.SetCenter(editingObject.GetCenter() + delta);
        } else if (editingWall != null) {
          editingWall.SetStart(editingWall.GetStart() + delta);
          editingWall.SetEnd(editingWall.GetEnd() + delta);
        }

        UpdateTransform();
      };

      /*
        For all the resize handles, it is crucial that we can drag one over
        another (e.g. drag top handle below the bottom handle) and everything
        still works. Thanksfully Unity supports negative width and height. We
        thus need to abstract this transform editor's size out from the actual
        object's size, so that we can have negative width/height on this object
        but normal size on the actual object being edited.

        And we also need to take into account of rotations.
       */

      resizeHandleTL.dragHandler = data => {
        // Rotate
        Vector2 delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * data.delta / canvasScale;

        Rect r = ((RectTransform)transform).rect;
        r.center = transform.localPosition;
        Vector2 movingPt = new Vector2(r.xMin, r.yMax);
        Vector2 oppositePt = new Vector2(r.xMax, r.yMin);
        Resize(delta, movingPt, oppositePt, false, true);
      };

      resizeHandleTR.dragHandler = data => {
        Vector2 delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * data.delta / canvasScale;

        Rect r = ((RectTransform)transform).rect;
        r.center = transform.localPosition;
        Vector2 movingPt = new Vector2(r.xMax, r.yMax);
        Vector2 oppositePt = new Vector2(r.xMin, r.yMin);
        Resize(delta, movingPt, oppositePt, true, true);
      };

      resizeHandleBL.dragHandler = data => {
        Vector2 delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * data.delta / canvasScale;

        Rect r = ((RectTransform)transform).rect;
        r.center = transform.localPosition;
        Vector2 movingPt = new Vector2(r.xMin, r.yMin);
        Vector2 oppositePt = new Vector2(r.xMax, r.yMax);
        Resize(delta, movingPt, oppositePt, false, false);
      };

      resizeHandleBR.dragHandler = data => {
        Vector2 delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * data.delta / canvasScale;

        Rect r = ((RectTransform)transform).rect;
        r.center = transform.localPosition;
        Vector2 movingPt = new Vector2(r.xMax, r.yMin);
        Vector2 oppositePt = new Vector2(r.xMin, r.yMax);
        Resize(delta, movingPt, oppositePt, true, false);
      };

      resizeHandleL.dragHandler = data => {
        // Rotate and filter move amount.
        Vector2 delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * data.delta / canvasScale;
        delta.y = 0;

        Rect r = ((RectTransform)transform).rect;
        r.center = transform.localPosition;
        // For the single direction movement, this is fine, even though moving
        // point isn't exactly the handle.
        Vector2 movingPt = new Vector2(r.xMin, r.yMax);
        Vector2 oppositePt = new Vector2(r.xMax, r.yMin);
        Resize(delta, movingPt, oppositePt, false, true);
      };

      resizeHandleT.dragHandler = data => {
        Vector2 delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * data.delta / canvasScale;
        delta.x = 0;

        Rect r = ((RectTransform)transform).rect;
        r.center = transform.localPosition;
        Vector2 movingPt = new Vector2(r.xMax, r.yMax);
        Vector2 oppositePt = new Vector2(r.xMin, r.yMin);
        Resize(delta, movingPt, oppositePt, true, true);
      };

      resizeHandleB.dragHandler = data => {
        Vector2 delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * data.delta / canvasScale;
        delta.x = 0;

        Rect r = ((RectTransform)transform).rect;
        r.center = transform.localPosition;
        Vector2 movingPt = new Vector2(r.xMin, r.yMin);
        Vector2 oppositePt = new Vector2(r.xMax, r.yMax);
        Resize(delta, movingPt, oppositePt, false, false);
      };

      resizeHandleR.dragHandler = data => {
        Vector2 delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * data.delta / canvasScale;
        delta.y = 0;

        Rect r = ((RectTransform)transform).rect;
        r.center = transform.localPosition;
        Vector2 movingPt = new Vector2(r.xMax, r.yMin);
        Vector2 oppositePt = new Vector2(r.xMin, r.yMax);
        Resize(delta, movingPt, oppositePt, true, false);
      };
    }

    /// <summary>
    /// Handling resizing. Pass in the handle that is moving and its opposite
    /// point in the rectangle.
    ///
    /// The last two booleans are needed to handle negative width/height. They
    /// stand for moving point's x should be greater than opposite point's x,
    /// and similarly for y. For example, if moving point is top left, then
    /// mxGTox is false and myGToy is true.
    /// </summary>
    void Resize(Vector2 delta, Vector2 movingPt, Vector2 oppositePt, bool mxGTox, bool myGToy) {
      if (editingObject != null) {
        Rect curr = ((RectTransform)transform).rect;
        curr.center = transform.localPosition;

        // Form a new rectangle based on the offset. The offset has to be
        // rotated according to our current rotation.
        Vector2 p1 = movingPt + delta;
        Vector2 p2 = oppositePt;
        Vector2 s = new Vector2();
        s.x = mxGTox ? p1.x - p2.x : p2.x - p1.x;
        s.y = myGToy ? p1.y - p2.y : p2.y - p1.y;

        Rect rect = Rect.MinMaxRect(
                      Mathf.Min(p1.x, p2.x), Mathf.Min(p1.y, p2.y), 
                      Mathf.Max(p1.x, p2.x), Mathf.Max(p1.y, p2.y)
                    );

        editingObject.SetCenter(rect.center);
        editingObject.SetSize(rect.size);

        // We need to update in such a way we allow negative width/height.
        // We also need to do a rotation. How much the center moved locally is
        // different than how much the center moved in parent transform space.
        Vector2 localCenterDelta = ((p1 + p2) / 2 - (Vector2)transform.localPosition);
        Vector2 centerDelta = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * localCenterDelta;
        transform.localPosition += (Vector3)centerDelta;
        ((RectTransform)transform).sizeDelta = s;
      }
    }

    /// <summary>
    /// Returns whether user is dragging one of the transform controls. Used by
    /// EditAreaController to determine whether this is a map pan or a resize.
    /// </summary>
    public bool IsDragging() {
      return rotationHandle.IsDragging() || translationHandle.IsDragging() ||
      resizeHandleTL.IsDragging() || resizeHandleT.IsDragging() ||
      resizeHandleTR.IsDragging() || resizeHandleL.IsDragging() ||
      resizeHandleR.IsDragging() || resizeHandleBL.IsDragging() ||
      resizeHandleB.IsDragging() || resizeHandleBR.IsDragging();
    }
  }
}
