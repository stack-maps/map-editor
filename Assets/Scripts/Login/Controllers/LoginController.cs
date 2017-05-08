using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MaterialUI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This class, together with ServiceController, handles all the logic behind
/// Login scene.
/// </summary>
public class LoginController : MonoBehaviour {

  /// <summary>
  /// The login button is pressed.
  /// </summary>
  public void OnLoginButtonTrigger() {
    // Placeholder

    DialogProgress dialog = DialogManager.ShowProgressLinear("Connecting to server...", "Loading", MaterialIconHelper.GetIcon(MaterialIconEnum.HOURGLASS_EMPTY));
    StartCoroutine(HideWindowAfterSeconds(dialog, 3.0f));
  }

  IEnumerator HideWindowAfterSeconds(MaterialDialog dialog, float duration) {
    yield return new WaitForSeconds(duration);
    dialog.Hide();
    SceneManager.LoadScene(1);
  }

  void Update() {
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
