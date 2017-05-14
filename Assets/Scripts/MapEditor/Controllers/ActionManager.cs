using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This class tracks actions performed by the user and provides undo and redo
  /// functionalities.
  ///
  /// How this class works: we snapshot the entire floor's state for each action
  /// and restore the state as needed.
  /// </summary>
  public class ActionManager : MonoBehaviour {
    readonly List<string> states = new List<string>();

    public int index = -1;

    public static ActionManager shared;

    public FloorController floorController;

    void Awake() {
      shared = this;
    }

    void Start() {
      Push();
    }

    /// <summary>
    /// Snapshots the current floor.
    /// </summary>
    public void Push() {
      index++;

      while (index < states.Count) {
        states.RemoveAt(index);
      }

      states.Add(floorController.ExportFloor());
      Debug.Log(states[index]);
    }

    /// <summary>
    /// Undoes the last action.
    /// </summary>
    public void Undo() {
      if (!CanUndo()) {
        return;
      }

      index--;
      floorController.ImportFloor(states[index]);
    }

    /// <summary>
    /// Redoes the last action.
    /// </summary>
    public void Redo() {
      if (!CanRedo()) {
        return;
      }

      index++;
      floorController.ImportFloor(states[index]);
    }

    /// <summary>
    /// Determines whether undo is available.
    /// </summary>
    public bool CanUndo() {
      return index > 0;
    }

    /// <summary>
    /// Determines whether redo is available.
    /// </summary>
    public bool CanRedo() {
      return index + 1 < states.Count;
    }
  }
}
