using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// This is an action as defined in ActionManager.
  /// </summary>
  public class Action : MonoBehaviour {
    public delegate void ActionBlock();

    /// <summary>
    /// The action we want to execute now.
    /// </summary>
    public ActionBlock currentAction;

    /// <summary>
    /// The action to restore the state to before we executed currentAction.
    /// </summary>
    public ActionBlock restoreAction;
  }
}
