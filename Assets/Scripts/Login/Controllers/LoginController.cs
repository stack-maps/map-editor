using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MaterialUI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace StackMaps {
  /// <summary>
  /// This class, together with ServiceController, handles all the logic behind
  /// Login scene.
  /// </summary>
  public class LoginController : MonoBehaviour {
    public MaterialInputField usernameInputField;
    public MaterialInputField passwordInputField;
    public MaterialInputField addressInputField;
    public Image screenTransitionMask;

    void Start() {
      usernameInputField.inputField.text = PlayerPrefs.GetString(PlayerPrefsKey.SavedUsername);
      addressInputField.inputField.text = PlayerPrefs.GetString(PlayerPrefsKey.SavedAddress);
    }

    /// <summary>
    /// The login button is pressed.
    /// </summary>
    public void OnLoginButtonPress() {
      // Placeholder
      string username = usernameInputField.inputField.text;
      string password = passwordInputField.inputField.text;
      string api = addressInputField.inputField.text;

      DialogComplexProgress d = (DialogComplexProgress)DialogManager.CreateComplexProgressLinear();
      d.Initialize("Connecting to server", "Loading", MaterialIconHelper.GetIcon(MaterialIconEnum.HOURGLASS_EMPTY));
      d.InitializeCancelButton("Cancel", () => {
        ServiceController.shared.CancelLogin();
        d.Hide();
      });

      DontDestroyOnLoad(d.gameObject);
      d.Show();

      ServiceController.shared.Login(api, username, password, (success, authenticated) => {
        if (success && authenticated) {
          PlayerPrefs.SetString(PlayerPrefsKey.SavedAddress, api);
          PlayerPrefs.SetString(PlayerPrefsKey.SavedUsername, username);
          AsyncOperation o = SceneManager.LoadSceneAsync(1);
          o.allowSceneActivation = false;

          TweenManager.TweenFloat(v => {
            Color c = screenTransitionMask.color;
            c.a = v;
            screenTransitionMask.color = c;
          }, 0, 1, 0.6f, 0, () => o.allowSceneActivation = true);

        } else {
          if (!success)
            DialogManager.ShowAlert("Unable to connect to the given address.", 
              "Connection Error", MaterialIconHelper.GetIcon(MaterialIconEnum.ERROR));
          else
            DialogManager.ShowAlert("Incorrect username or password.", 
              "Login Failed", MaterialIconHelper.GetIcon(MaterialIconEnum.ERROR));
        }

        d.Hide();
      });

    }


    void Update() {
      if (Input.GetKeyDown(KeyCode.Tab)) {
        UnityEngine.UI.Selectable next = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Selectable>().FindSelectableOnDown();

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
}