using System;
using MaterialUI;
using System.Text.RegularExpressions;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// Regular expression validator of a Material Input Field.
  /// </summary>
  public class RegexValidator : MonoBehaviour, ITextValidator {
    /// <summary>
    /// The regular expression to test the input.
    /// </summary>
    public string regularExpression = "";

    /// <summary>
    /// The error string to display.
    /// </summary>
    public string errorString;

    MaterialInputField target;

    #region ITextValidator implementation

    public void Init(MaterialInputField materialInputField) {
      target = materialInputField;
    }

    public bool IsTextValid() {
      bool valid = Regex.IsMatch(target.inputField.text, regularExpression);

      if (!valid) {
        target.validationText.text = errorString;
      }

      return valid;
    }

    #endregion
  }
}

