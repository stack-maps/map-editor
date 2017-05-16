using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MaterialUI;

/// <summary>
/// This class is the top controller for the map editor scene. It is the
/// coordinator of all the areas of the editor.
/// </summary>
public class MapEditorController : MonoBehaviour {
  public Image screenTransitionMask;

  void Start() {
    TweenManager.TweenFloat(v => {
      Color c = screenTransitionMask.color;
      c.a = v;
      screenTransitionMask.color = c;
    }, 100, 0, 1f);
  }

  void Update() {
    // Implement tab to cycle.
    if (Input.GetKeyDown(KeyCode.Tab)) {
      Selectable next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

      if (next != null) {
        InputField inputfield = next.GetComponent<InputField>();

        if (inputfield != null) {
          inputfield.OnPointerClick(new PointerEventData(EventSystem.current));
          EventSystem.current.SetSelectedGameObject(next.gameObject, new BaseEventData(EventSystem.current));
        }
      }
    }
  }
}
