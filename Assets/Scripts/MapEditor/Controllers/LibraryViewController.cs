using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;
using UnityEngine.UI;

namespace StackMaps {
  /// <summary>
  /// Manages the main view area to display a list of maps in selected library.
  /// </summary>
  public class LibraryViewController : MonoBehaviour {
    public GameObject view;

    public Text libraryNameText;
    public Text libraryFloorCountText;

    /// <summary>
    /// Displays the library view controller, populating the view with the given
    /// library's information.
    /// </summary>
    /// <param name="library">Library.</param>
    public void Show(Library library) {
      libraryNameText.text = library.libraryName;
      int count = library.floors.Count;

      if (count == 0) {
        libraryFloorCountText.text = "Empty Library";
      } else if (count == 1) {
        libraryFloorCountText.text = "Single Floor";
      } else {
        libraryFloorCountText.text = count + " Floors";
      }

      view.SetActive(true);
    }

    public void Hide() {
      view.SetActive(false);
    }
  }
}