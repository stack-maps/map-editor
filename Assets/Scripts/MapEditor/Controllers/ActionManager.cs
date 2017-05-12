using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This class tracks actions performed by the user and provides undo and redo
  /// functionalities.
  ///
  /// How this class works: for each undoable action in the code, we push
  /// function stubs that specify both how to execute it, and how to rewind it.
  /// Undo and redo come naturally from the stack of actions we keep.
  /// </summary>
  public class ActionManager : MonoBehaviour {
    readonly List<Action> actions = new List<Action>();

    int index = -1;

    public static ActionManager shared;

    void Awake() {
      shared = this;
    }

    /// <summary>
    /// Pushes the action to the stack. Also performs it immediately.
    /// </summary>
    /// <param name="action">Action to do.</param>
    public void PushAction(Action action) {
      index++;

      while (index < actions.Count) {
        actions.RemoveAt(index);
      }

      actions.Add(action);
      action.currentAction();
    }

    /// <summary>
    /// Undoes the last action.
    /// </summary>
    public void Undo() {
      if (!CanUndo()) {
        return;
      }

      actions[index].restoreAction();
      index--;
    }

    /// <summary>
    /// Redoes the last action.
    /// </summary>
    public void Redo() {
      if (!CanRedo()) {
        return;
      }

      index++;
      actions[index].currentAction();
    }

    /// <summary>
    /// Determines whether undo is available.
    /// </summary>
    public bool CanUndo() {
      return index >= 0;
    }

    /// <summary>
    /// Determines whether redo is available.
    /// </summary>
    public bool CanRedo() {
      return index + 1 < actions.Count;
    }
  }
}
