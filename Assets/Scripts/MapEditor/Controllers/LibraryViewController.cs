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
    public FloorPreviewController floorPreviewController;

    public GameObject view;
    public RectTransform previewCellContainer;

    public Text libraryNameText;
    public Text libraryFloorCountText;

    public FloorPreviewCell floorPreviewCellPrefab;

    /// <summary>
    /// Displays the library view controller, populating the view with the given
    /// library's information. Assume the library is loaded.
    /// </summary>
    /// <param name="library">Library.</param>
    public void Show(Library library) {
      UpdateMetaInfo(library);
      UpdateFloorPreviews(library);
      view.SetActive(true);
    }

    public void Hide() {
      view.SetActive(false);
    }

    /// <summary>
    /// Updates the meta information of the library.
    /// </summary>
    /// <param name="library">Library.</param>
    void UpdateMetaInfo(Library library) {
      libraryNameText.text = library.libraryName;
      int count = library.floors.Count;

      if (count == 0) {
        libraryFloorCountText.text = "Empty Library";
      } else if (count == 1) {
        libraryFloorCountText.text = "Single Floor";
      } else {
        libraryFloorCountText.text = count + " Floors";
      }
    }

    /// <summary>
    /// Look into library and create a preview for each floor.
    /// </summary>
    void UpdateFloorPreviews(Library library) {
      // Destroy the transforms
      for (int i = 0; i < previewCellContainer.childCount - 1; i++) {
        Destroy(previewCellContainer.GetChild(i).gameObject);
      }

      // Put new ones in.
      for (int i = 0; i < library.floors.Count; i++) {
        FloorPreviewCell cell = Instantiate(floorPreviewCellPrefab, previewCellContainer);
        cell.SetFloorName(library.floors[i].floorName);
        cell.SetCanMoveUp(i > 0);
        cell.SetCanMoveDown(i < library.floors.Count);

        floorPreviewController.DrawFloorPreview(cell.previewContainer, library.floors[i].floorJSONCache);
      }

      // Move the last cell last.
      previewCellContainer.GetChild(0).SetAsLastSibling();
    }
  }
}