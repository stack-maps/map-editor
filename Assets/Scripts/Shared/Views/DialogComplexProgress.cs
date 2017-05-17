using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;
using UnityEngine.Events;

namespace StackMaps {
  /// <summary>
  /// Shows a dialog progress with a cancel button.
  /// </summary>
  public class DialogComplexProgress : DialogProgress {
    public MaterialButton cancelButton;

    public void InitializeCancelButton(string titleText, UnityAction callback) {
      cancelButton.textText = titleText;
      cancelButton.buttonObject.onClick.AddListener(callback);
    }

  }
}