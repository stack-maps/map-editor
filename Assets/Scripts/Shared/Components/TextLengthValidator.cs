using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;

public class TextLengthValidator : MonoBehaviour, ITextValidator {

  MaterialInputField inputField;

  public int minLength;

  public void Init(MaterialInputField materialInputField) {
    inputField = materialInputField;
  }

  public bool IsTextValid() {
    if (inputField.inputField.text.Length > minLength) {
      return true;
    } else {
      inputField.validationText.text = "Can't be empty!";
      return false;
    }
  }
}
