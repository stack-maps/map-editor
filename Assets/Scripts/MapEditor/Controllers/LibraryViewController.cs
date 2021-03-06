﻿using System.Collections;
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
    public Transform previewEndCell;

    public Text libraryNameText;
    public Text libraryFloorCountText;

    public FloorPreviewCell floorPreviewCellPrefab;

    public EditAreaController editAreaController;

    Library displayingLibrary;

    /// <summary>
    /// Displays the library view controller, populating the view with the given
    /// library's information. Assume the library is loaded.
    /// </summary>
    /// <param name="library">Library.</param>
    public void Show(Library library) {
    displayingLibrary = library;
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
        int cap = i;
        FloorPreviewCell cell = Instantiate(floorPreviewCellPrefab, previewCellContainer);
        Canvas.ForceUpdateCanvases();
        cell.SetFloorName(library.floors[i].floorName);
        cell.SetEditButtonCallback(() => EditFloor(library.floors[cap]));
        // Also add in dropdown button callbacks!
        // cell.SetCanMoveUp(i > 0);
        // cell.SetCanMoveDown(i < library.floors.Count);
        floorPreviewController.DrawFloorPreview(cell.previewContainer, library.floors[i].floorJSONCache);
      }

      // Move the last cell last.
      previewEndCell.SetAsLastSibling();
    }

    void EditFloor(Floor f) {
      view.SetActive(false);
      editAreaController.BeginEdit(f);
    }

    public void OnAddFloorButtonPress() {
      DialogManager.ShowPrompt("Floor Name", CreateFloor, "CREATE", 
        "Create Floor", MaterialIconHelper.GetIcon(MaterialIconEnum.CREATE), null, "CANCEL");
    }

    void CreateFloor(string floorName) {
      int libId = displayingLibrary.libraryId;
      int order = displayingLibrary.floors.Count;

      DialogComplexProgress d = (DialogComplexProgress)DialogManager.CreateComplexProgressLinear();
      d.Initialize("Creating new floor...", "Loading", MaterialIconHelper.GetIcon(MaterialIconEnum.HOURGLASS_EMPTY));
      d.InitializeCancelButton("CANCEL", ServiceController.shared.CancelCreateFloor);
      d.Show();

      ServiceController.shared.CreateFloor(libId, floorName, order, (suc1, suc2, id) => {
        d.Hide();

        if (!suc1) {
          DialogManager.ShowAlert("Unable to communicate with server!", 
            "Connection Error", MaterialIconHelper.GetIcon(MaterialIconEnum.ERROR));
        } else if (!suc2) {
          DialogManager.ShowAlert("Login has expired! Please relogin and try again.",  
            "Login Expired", MaterialIconHelper.GetIcon(MaterialIconEnum.ERROR));
        } else {
          DialogManager.ShowAlert("Floor has been created.", "Success", MaterialIconHelper.GetIcon(MaterialIconEnum.CHECK));

          // Add the newly created floor here.
          Floor f = new Floor();
          f.floorId = id;
          f.floorName = floorName;
          f.floorOrder = order;
          displayingLibrary.floors.Add(f);
          UpdateFloorPreviews(displayingLibrary);
        }
      });
    }
  }
}
