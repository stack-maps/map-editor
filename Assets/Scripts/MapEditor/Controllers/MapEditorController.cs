using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MaterialUI;
using UnityEngine.SceneManagement;

namespace StackMaps {
  /// <summary>
  /// This class is the top controller for the map editor scene. It is the
  /// coordinator of all the areas of the editor.
  /// </summary>
  public class MapEditorController : MonoBehaviour {
    public Image screenTransitionMask;

    EditAreaController editAreaController;

    public LibraryExplorer libraryExplorer;

    void Start() {
      TweenManager.TweenFloat(v => {
        Color c = screenTransitionMask.color;
        c.a = v;
        screenTransitionMask.color = c;
      }, 100, 0, 1f);


      editAreaController = GetComponentInChildren<EditAreaController>(true);
    }

    void Update() {
      // Implement tab to cycle.
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

    public void OnFileDropdownItemSelect(int item) {
      if (item == 0) {
        editAreaController.SaveFloor();
      } else if (item == 1) {
      } else if (item == 2) {
      }
    }

    public void OnCreateLibraryButtonPress() {
      DialogManager.ShowPrompt("Library Name", CreateLibrary, "CREATE", 
        "Create Library", MaterialIconHelper.GetIcon(MaterialIconEnum.CREATE), null, "CANCEL");
    }

    void CreateLibrary(string libName) {
      DialogComplexProgress d = (DialogComplexProgress)DialogManager.CreateComplexProgressLinear();
      d.Initialize("Creating new library...", "Loading", MaterialIconHelper.GetIcon(MaterialIconEnum.HOURGLASS_EMPTY));
      d.InitializeCancelButton("CANCEL", ServiceController.shared.CancelCreateLibrary);
      d.Show();

      ServiceController.shared.CreateLibrary(libName, (suc1, suc2, id) => {
        d.Hide();

        if (!suc1) {
          DialogManager.ShowAlert("Unable to communicate with server!", 
            "Connection Error", MaterialIconHelper.GetIcon(MaterialIconEnum.ERROR));
        } else if (!suc2) {
          DialogManager.ShowAlert("Login has expired! Please relogin and try again.",  
            "Login Expired", MaterialIconHelper.GetIcon(MaterialIconEnum.ERROR));
        } else {
          DialogManager.ShowAlert("Library has been created.", "Success", MaterialIconHelper.GetIcon(MaterialIconEnum.CHECK));

          // Add the newly created library here.
          Library lib = new Library();
          lib.libraryId = id;
          lib.libraryName = libName;
          libraryExplorer.libraries.Add(lib);
          libraryExplorer.ConfigureCells();
        }
      });
    }

    public void OnLogoutButtonPressed() {
      ServiceController.shared.Logout();
      SceneManager.LoadScene(0);
    }
  }
}