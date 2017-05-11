using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StackMaps {
  /// <summary>
  /// Marks this object as selectable by the editor.
  /// </summary>
  public class Selectable : MonoBehaviour, IPointerClickHandler {
    #region IPointerClickHandler implementation

    public void OnPointerClick(PointerEventData eventData) {
      // Relay this information to all delegates
      foreach (SelectableDelegate d in delegates) {
        d(gameObject);
      }
    }

    #endregion

    public delegate void SelectableDelegate(GameObject selected);

    /// <summary>
    /// A list of delegates that will receive calls when any object is clicked.
    /// </summary>
    public static List<SelectableDelegate> delegates = new List<SelectableDelegate>();
  }
}
