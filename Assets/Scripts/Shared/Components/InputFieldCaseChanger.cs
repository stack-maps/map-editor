using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

namespace StackMaps {
  /// <summary>
  /// This class changes casing for all input letters.
  /// </summary>
  public class InputFieldCaseChanger : MonoBehaviour {
    public bool toUpperCase = true;

    void Start() {
      GetComponent<InputField>().onValidateInput += (input, charIndex, addedChar) => MyValidate(addedChar);
    }

    char MyValidate(char charToValidate) {
      if (toUpperCase) {
        return charToValidate.ToString().ToUpper()[0];
      } else {
        return charToValidate.ToString().ToLower()[0];
      }
    }
  }
}