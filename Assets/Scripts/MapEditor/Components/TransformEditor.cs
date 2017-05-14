using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;
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
    // This is set by the edit area controller. Change it there!
    public float snapGridSize = 20;
    public float snapAngleSize = 15;

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

    CanvasGroup canvasGroup;

    // Used to find the canvas for scale.
    Canvas canvas;

    // Cached information for snapping.
    Vector2 startMousePos;

    /// <summary>
    /// The current canvas scale. This is required since the screen-coordinates
    /// are not necessarily to be the same as the canvas coordinates.
    /// </summary>
    public float canvasScale = 1;

    Rectangle editingRect;

    Wall editingWall;

    // These are used to cache transform size before dragging.
    Vector2 cachedSize;
    Vector2 cachedCenter;
    Vector2 cachedWallStart;
    Vector2 cachedWallEnd;
    float cachedRotation;
    float cachedUserRotation;
    Vector2 cachedSelfPosition;

    /// <summary>
    /// Sets up the script according to the given object. We switch modes
    /// automatically between a rectangle editor and a wall editor.
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SetEditingObject(GameObject obj) {
      if (obj == null) {
        editingRect = null;
        editingWall = null;
      } else {
        editingRect = obj.GetComponent<Rectangle>();
        editingWall = obj.GetComponent<Wall>();
      }

      SwitchMode();
    }

    /// <summary>
    /// Depending on which object we are editing, we switch visibility modes.
    /// </summary>
    void SwitchMode() {
      if (editingRect != null) {
        
        rotationHandle.gameObject.SetActive(!editingRect.disableEditing);
        resizeHandleTL.gameObject.SetActive(!editingRect.disableEditing);
        resizeHandleT.gameObject.SetActive(!editingRect.disableEditing);
        resizeHandleL.gameObject.SetActive(!editingRect.disableEditing);
        resizeHandleR.gameObject.SetActive(!editingRect.disableEditing);
        resizeHandleTR.gameObject.SetActive(!editingRect.disableEditing);
        resizeHandleBL.gameObject.SetActive(!editingRect.disableEditing);
        resizeHandleB.gameObject.SetActive(!editingRect.disableEditing);
        resizeHandleBR.gameObject.SetActive(!editingRect.disableEditing);

        translateHandleGraphics.color = new Color32(0, 0, 0, 0x40);

        ((RectTransform)transform).pivot = new Vector2(0.5f, 0.5f);

        // Set up the editor size for once.
        UpdateTransform();
      } else if (editingWall != null) {
        rotationHandle.gameObject.SetActive(false);
        resizeHandleTL.gameObject.SetActive(false);
        resizeHandleT.gameObject.SetActive(false);
        resizeHandleL.gameObject.SetActive(true);
        resizeHandleR.gameObject.SetActive(true);
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

      if (canvasGroup == null) {
        canvasGroup = GetComponent<CanvasGroup>();
      }

      canvasGroup.interactable = editingRect != null || editingWall != null;
      float goal = canvasGroup.interactable ? 1 : 0;
      canvasGroup.blocksRaycasts = canvasGroup.interactable;
      TweenManager.TweenFloat(v => canvasGroup.alpha = v, canvasGroup.alpha, goal, 0.15f);
    }

    /// <summary>
    /// Sets up the center, size and rotation according to the editing object.
    /// </summary>
    public void UpdateTransform() {
      if (editingRect != null) {
        transform.position = editingRect.transform.position;
        ((RectTransform)transform).sizeDelta = editingRect.GetSize();
        transform.eulerAngles = editingRect.transform.eulerAngles;
      } else if (editingWall != null) {
        RectTransform t = editingWall.transform as RectTransform;
        ((RectTransform)transform).anchoredPosition = t.anchoredPosition;
        ((RectTransform)transform).sizeDelta = t.sizeDelta;
        transform.localEulerAngles = t.localEulerAngles;
      }
    }

    void Start() {
      canvas = FindObjectOfType<Canvas>();
    }


    /// <summary>
    /// Defines all handle actions.
    /// </summary>
    void Awake() {
      shared = this;

      translationHandle.dragHandler = data => {
        Vector2 delta = SnapToGrid((data.position - startMousePos) / canvasScale / canvas.scaleFactor);

        if (editingRect != null) {
          editingRect.SetCenter(cachedCenter + delta);
        } else if (editingWall != null) {
          editingWall.SetStart(cachedWallStart + delta);
          editingWall.SetEnd(cachedWallEnd + delta);
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
        Vector2 delta = SnapToGrid((data.position - startMousePos) / canvasScale / canvas.scaleFactor);
        delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * (Vector3)delta;

        Rect r = new Rect(Vector2.zero, cachedSize);
        r.center = cachedCenter;
        Vector2 movingPt = new Vector2(r.xMin, r.yMax);
        Vector2 oppositePt = new Vector2(r.xMax, r.yMin);
        Resize(delta, movingPt, oppositePt, false, true);
      };

      resizeHandleTR.dragHandler = data => {
        Vector2 delta = SnapToGrid((data.position - startMousePos) / canvasScale / canvas.scaleFactor);
        delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * (Vector3)delta;

        Rect r = new Rect(Vector2.zero, cachedSize);
        r.center = cachedCenter;
        Vector2 movingPt = new Vector2(r.xMax, r.yMax);
        Vector2 oppositePt = new Vector2(r.xMin, r.yMin);
        Resize(delta, movingPt, oppositePt, true, true);
      };

      resizeHandleBL.dragHandler = data => {
        Vector2 delta = SnapToGrid((data.position - startMousePos) / canvasScale / canvas.scaleFactor);
        delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * (Vector3)delta;

        Rect r = new Rect(Vector2.zero, cachedSize);
        r.center = cachedCenter;
        Vector2 movingPt = new Vector2(r.xMin, r.yMin);
        Vector2 oppositePt = new Vector2(r.xMax, r.yMax);
        Resize(delta, movingPt, oppositePt, false, false);
      };

      resizeHandleBR.dragHandler = data => {
        Vector2 delta = SnapToGrid((data.position - startMousePos) / canvasScale / canvas.scaleFactor);
        delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * (Vector3)delta;

        Rect r = new Rect(Vector2.zero, cachedSize);
        r.center = cachedCenter;
        Vector2 movingPt = new Vector2(r.xMax, r.yMin);
        Vector2 oppositePt = new Vector2(r.xMin, r.yMax);
        Resize(delta, movingPt, oppositePt, true, false);
      };

      resizeHandleL.dragHandler = data => {
        // Now two things. This handle (and R) is active for both wall and rect.
        // They are treated differently, though.
        Vector2 delta = SnapToGrid((data.position - startMousePos) / canvasScale / canvas.scaleFactor);

        if (editingRect != null) {
          // Rotate and filter movement.
          delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * (Vector3)delta;
          delta.y = 0;

          Rect r = new Rect(Vector2.zero, cachedSize);
          r.center = cachedCenter;
          // For the single direction movement, this is fine, even though moving
          // point isn't exactly the handle.
          Vector2 movingPt = new Vector2(r.xMin, r.yMax);
          Vector2 oppositePt = new Vector2(r.xMax, r.yMin);
          Resize(delta, movingPt, oppositePt, false, true);
        } else if (editingWall != null) {
          // We just call set method. And we are done!
          editingWall.SetStart(editingWall.GetStart() + delta);
          UpdateTransform();
        }
      };

      resizeHandleR.dragHandler = data => {
        Vector2 delta = SnapToGrid((data.position - startMousePos) / canvasScale / canvas.scaleFactor);

        if (editingRect != null) {
          // Rotate and filter movement.
          delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * (Vector3)delta;
          delta.y = 0;

          Rect r = new Rect(Vector2.zero, cachedSize);
          r.center = cachedCenter;
          Vector2 movingPt = new Vector2(r.xMax, r.yMin);
          Vector2 oppositePt = new Vector2(r.xMin, r.yMax);
          Resize(delta, movingPt, oppositePt, true, false);
        } else if (editingWall != null) {
          // We just call set method. And we are done!
          editingWall.SetEnd(editingWall.GetEnd() + delta);
          UpdateTransform();
        }
      };

      resizeHandleT.dragHandler = data => {
        Vector2 delta = SnapToGrid((data.position - startMousePos) / canvasScale / canvas.scaleFactor);
        delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * (Vector3)delta;
        delta.x = 0;

        Rect r = new Rect(Vector2.zero, cachedSize);
        r.center = cachedCenter;
        Vector2 movingPt = new Vector2(r.xMax, r.yMax);
        Vector2 oppositePt = new Vector2(r.xMin, r.yMin);
        Resize(delta, movingPt, oppositePt, true, true);
      };

      resizeHandleB.dragHandler = data => {
        Vector2 delta = SnapToGrid((data.position - startMousePos) / canvasScale / canvas.scaleFactor);
        delta = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * (Vector3)delta;
        delta.x = 0;

        Rect r = new Rect(Vector2.zero, cachedSize);
        r.center = cachedCenter;
        Vector2 movingPt = new Vector2(r.xMin, r.yMin);
        Vector2 oppositePt = new Vector2(r.xMax, r.yMax);
        Resize(delta, movingPt, oppositePt, false, false);
      };


      // Rotation shouldn't be too hard. We just want to rotate depending on a
      // delta angle as seen from the center of the object.
      rotationHandle.dragHandler = data => {
        // Use sine rule to figure out delta angle. Go draw a diagram!
        Vector2 ptA = transform.position;
        Vector2 ptB = data.position;
        Vector2 ptC = data.position - data.delta;

        float sideA = (ptB - ptC).magnitude;
        float sideB = (ptA - ptC).magnitude;

        float angleB = Vector2.Angle(ptC - ptB, ptA - ptB);

        float angleA = Mathf.Asin(sideA * Mathf.Sin(angleB * Mathf.Deg2Rad) / sideB) * Mathf.Rad2Deg;

        // Still needs to determine sign of this rotation!
        float a1 = Mathf.Atan2((ptB - ptA).x, (ptB - ptA).y);
        float a2 = Mathf.Atan2((ptC - ptA).x, (ptC - ptA).y);

        float sign = Mathf.Sign(a2 - a1);

        cachedUserRotation += sign * angleA;

        // Round
        float roundedRotation = Mathf.Round(cachedUserRotation / snapAngleSize) * snapAngleSize;

        editingRect.SetRotation(cachedRotation + roundedRotation);

        UpdateTransform();
      };

      // Below are functions that compare before and after drags. If they differ
      // then user modified the rectangle and we should inform ActionManager.
      RegisterHandleComparison(rotationHandle);
      RegisterHandleComparison(translationHandle);
      RegisterHandleComparison(resizeHandleTL);
      RegisterHandleComparison(resizeHandleB);
      RegisterHandleComparison(resizeHandleBL);
      RegisterHandleComparison(resizeHandleBR);
      RegisterHandleComparison(resizeHandleR);
      RegisterHandleComparison(resizeHandleL);
      RegisterHandleComparison(resizeHandleT);
      RegisterHandleComparison(resizeHandleTR);
    }

    /// <summary>
    /// Registers each handle to be able to push undo/redo stack if there is a
    /// modification at the end of the drag.
    /// </summary>
    /// <param name="handle">Handle.</param>
    void RegisterHandleComparison(TransformDrag handle) {
      handle.beginDragHandler = data => {
        startMousePos = data.position;
        cachedUserRotation = 0;
        cachedSelfPosition = transform.localPosition;

        if (editingRect != null) {
          cachedCenter = editingRect.GetCenter();
          cachedSize = editingRect.GetSize();
          cachedRotation = editingRect.GetRotation();
        } else if (editingWall != null) {
          cachedWallStart = editingWall.GetStart();
          cachedWallEnd = editingWall.GetEnd();
        }
      };

      handle.endDragHandler = data => {
        if (editingRect != null) {
          Vector2 currCenter = editingRect.GetCenter();
          Vector2 currSize = editingRect.GetSize();

          if (!((currCenter - cachedCenter).magnitude < 0.01f &&
              (currSize - cachedSize).magnitude < 0.01f &&
              Mathf.Abs(cachedRotation - editingRect.GetRotation()) < 0.01f)) {
            ActionManager.shared.Push();
          }
        } else if (editingWall != null) {
          Vector2 currStart = editingWall.GetStart();
          Vector2 currEnd = editingWall.GetEnd();

          if (!((currStart - cachedWallStart).magnitude < 0.01f &&
              (currEnd - cachedWallEnd).magnitude < 0.01f)) {
            ActionManager.shared.Push();
          }
        }
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
      if (editingRect != null) {
        Rect curr = new Rect(Vector2.zero, cachedSize);
        curr.center = cachedCenter;
        Quaternion q = Quaternion.Euler(0, 0, transform.localEulerAngles.z);

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


        Vector2 newCenter = q * (Vector3)(rect.center - curr.center) + (Vector3)curr.center;
        editingRect.SetCenter(newCenter);
        editingRect.SetSize(rect.size);

        // We need to update in such a way we allow negative width/height.
        // We also need to do a rotation. How much the center moved locally is
        // different than how much the center moved in parent transform space.
        Vector2 localCenterDelta = (p1 + p2) / 2 - cachedCenter;
        Vector2 centerDelta = q * localCenterDelta;
        transform.localPosition = cachedSelfPosition + centerDelta;
        ((RectTransform)transform).sizeDelta = s;
      }
    }

    Vector2 SnapToGrid(Vector2 p) {
      return new Vector2(Mathf.Round(p.x / snapGridSize), Mathf.Round(p.y / snapGridSize)) * snapGridSize;
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
