using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StackMaps {
  /// <summary>
  /// A dragging component storing information on how much the object has been
  /// dragged.
  /// </summary>
  public class TransformDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    bool dragging;

    public delegate void DragHandler(PointerEventData eventData);

    public DragHandler dragHandler;

    public DragHandler beginDragHandler;

    public DragHandler endDragHandler;

    /// <summary>
    /// Used by transform editor to store information when dragging started.
    /// </summary>
    public Vector2 data;

    /// <summary>
    /// Determines whether this instance is being dragged.
    /// </summary>
    public bool IsDragging() {
      return dragging;
    }

    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData) {
      dragging = true;

      if (beginDragHandler != null)
        beginDragHandler(eventData);
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData) {
      if (dragHandler != null)
        dragHandler(eventData);
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData) {
      dragging = false;

      if (endDragHandler != null)
        endDragHandler(eventData);
    }

    #endregion
  }
}