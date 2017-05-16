using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    List<Library> libraries;

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

    void ConfigureCells() {
      // Remove
      for (int i = 1; i < container.transform.childCount; i++) {
        Destroy(container.transform.GetChild(i).gameObject);
      }

      // Add
      for (int i = 0; i < libraries.Count; i++) {
        LibraryCell cell = Instantiate(libraryCellPrefab, container.transform);
        cell.SetLibrary(libraries[i]);
      }
    }
  }
}