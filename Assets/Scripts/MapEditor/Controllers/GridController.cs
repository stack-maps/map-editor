using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This class manages the grids seen on the editor page.
  /// </summary>
  public class GridController : MonoBehaviour {
    public int minorGridsPerMajorGrid = 5;
    public float gridSize = 1.0f;
    public Color majorGridLineColor;
    public Color minorGridLineColor;
    public float majorGridLineThickness = 2;
    public float minorGridLineThickness = 1;
    public GridLine gridPrefab;
    public Transform horizontalContainer;
    public Transform verticalContainer;

    /// <summary>
    /// Reconfigures the grid according to the given canvas size and current grid
    /// settings.
    /// </summary>
    /// <param name="canvasSize">Canvas size.</param>
    public void UpdateGrid(Vector2 canvasSize) {
      // We want an odd number of grids enough to cover up the entire canvas.
      int countX = ((int)(canvasSize.x / gridSize) / 2) * 2 + 1;
      int countY = ((int)(canvasSize.y / gridSize) / 2) * 2 + 1;

      // Clear out current grids
      for (int i = 0; i < horizontalContainer.childCount; i++) {
        Destroy(horizontalContainer.GetChild(0));
      }

      for (int i = 0; i < verticalContainer.childCount; i++) {
        Destroy(verticalContainer.GetChild(0));
      }

      // Setup new grids.
      int middleX = countX / 2;
      int middleY = countY / 2;

      for (int i = 0; i < countX; i++) {
        GridLine gl = Instantiate(gridPrefab, horizontalContainer);

        if ((middleX - i) % minorGridsPerMajorGrid == 0) {
          gl.SetupGridLine(majorGridLineColor, majorGridLineThickness, canvasSize.y);
        } else {
          gl.SetupGridLine(minorGridLineColor, minorGridLineThickness, canvasSize.y);
        }
      }

      for (int i = 0; i < countY; i++) {
        GridLine gl = Instantiate(gridPrefab, verticalContainer);

        if ((middleY - i) % minorGridsPerMajorGrid == 0) {
          gl.SetupGridLine(majorGridLineColor, majorGridLineThickness, canvasSize.x);
        } else {
          gl.SetupGridLine(minorGridLineColor, minorGridLineThickness, canvasSize.x);
        }
      }
    }
  }
}