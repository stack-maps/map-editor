using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This class controls the cursor image swaps.
  ///
  /// This acts as a stack, with each new change of cursor image pushed to the
  /// top of the stack. If a change not on the top wants to be popped, then
  /// anything on top will also be pushed off.
  /// </summary>
  public static class CursorController {
    static readonly List<Texture2D> cursorImages = new List<Texture2D>();
    static readonly List<Vector2> cursorHotspots = new List<Vector2>();

    /// <summary>
    /// Pushs a new cursor image to the stack, returning a stack ID used to
    /// pop the cursor off the stack after the cursor change should be reverted.
    /// </summary>
    /// <returns>The cursor.</returns>
    /// <param name="cursorImage">Cursor image.</param>
    /// <param name="cursorHotspot">Cursor hotspot.</param>
    public static int PushCursor(Texture2D cursorImage, Vector2 cursorHotspot) {
      cursorImages.Add(cursorImage);
      cursorHotspots.Add(cursorHotspot);
      Cursor.SetCursor(cursorImage, cursorHotspot, CursorMode.Auto);

      return cursorImages.Count;
    }

    /// <summary>
    /// This should be called when the cursor image change should be reverted.
    /// </summary>
    /// <param name="id">Identifier as given by PushCursor.</param>
    public static void PopCursor(int id) {
      if (cursorImages.Count >= id) {
        cursorImages.RemoveRange(id - 1, cursorImages.Count - id + 1);
      } else {
        return;
      }

      if (id == 1) {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
      } else {
        Cursor.SetCursor(cursorImages[cursorImages.Count - 1], cursorHotspots[cursorHotspots.Count - 1], CursorMode.Auto);
      }
    }

    /// <summary>
    /// Returns the cursor currently being displayed.
    /// </summary>
    public static int GetCurrentId() {
      return cursorImages.Count;
    }
  }
}