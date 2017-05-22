using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;

namespace StackMaps {
  /// <summary>
  /// This manages the display of libraries and floors on the sidebar. It loads
  /// the libraries and floors. When the user wants to edit a library,
  /// information should be copied from this class to the EditAreaController.
  /// </summary>
  public class LibraryExplorer : MonoBehaviour {
    public ExpandButton expandButton;
    public SidebarElement container;
    public SidebarElement defaultCell;
    public GameObject loadingPanel;
    public GameObject loadFailedPanel;
    public GameObject emptyPanel;
    public LibraryCell libraryCellPrefab;

    public LibraryViewController libraryViewController;

    [SerializeField]
    public List<Library> libraries;

    public enum DisplayMode {
      Loading,
      LoadFailed,
      Loaded
    }

    void Start() {
      UpdateLibraries();
    }

    public void UpdateLibraries() {
      // We want to reset all the libraries. Does not change the current editing
      // floor. 
      libraries = null;
      defaultCell.Show(true, true);
      SetMode(DisplayMode.Loading);

      ServiceController.shared.GetLibraryList((success, list) => {
        if (success) {
          libraries = list;
          SetMode(DisplayMode.Loaded);
          defaultCell.Show(libraries.Count == 0, true);
          ConfigureCells();
        } else {
          // Failed to load the library. 
          SetMode(DisplayMode.LoadFailed);
        }
      });
    }

    public void SetMode(DisplayMode mode) {
      loadingPanel.SetActive(mode == DisplayMode.Loading);
      loadFailedPanel.SetActive(mode == DisplayMode.LoadFailed);
      emptyPanel.SetActive(mode == DisplayMode.Loaded && libraries != null && libraries.Count == 0);
    }

    public void OnExpandButtonPress() {
      // We want to show or hide everything in the panel.
      container.Show(expandButton.isExpanded, true);
    }

    public void ConfigureCells() {
      // Remove
      for (int i = 1; i < container.transform.childCount; i++) {
        Destroy(container.transform.GetChild(i).gameObject);
      }

      // Add
      for (int i = 0; i < libraries.Count; i++) {
        int capture = i;
        LibraryCell cell = Instantiate(libraryCellPrefab, container.transform);
        cell.SetLibrary(libraries[i]);
        cell.selectButton.buttonObject.onClick.AddListener(() => OnLibraryCellPress(capture));
      }
    }

    void OnLibraryCellPress(int index) {
      // Configure library view controller.
      if (libraries[index].floors == null) {
        // We need to load this library from server first! That means a loading
        // dialog.
        DialogComplexProgress d = (DialogComplexProgress)DialogManager.CreateComplexProgressLinear();
        d.Initialize("Getting library floor plans...", "Loading", MaterialIconHelper.GetIcon(MaterialIconEnum.HOURGLASS_EMPTY));
        d.InitializeCancelButton("Cancel", () => {
          ServiceController.shared.CancelGetLibrary();
          d.Hide();
        });

        d.Show();

        ServiceController.shared.GetLibrary(libraries[index].libraryId, (success, lib) => {
          d.Hide();

          if (success) {
            // Load the library.
            libraries[index] = lib;
            libraryViewController.Show(libraries[index]);
          } else {
            // Display error.
            DialogManager.ShowAlert("Unable to connect to server!", 
              "Connection Error", MaterialIconHelper.GetIcon(MaterialIconEnum.ERROR));
          }
        });
      } else {
        libraryViewController.Show(libraries[index]);
      }
    }
  }
}