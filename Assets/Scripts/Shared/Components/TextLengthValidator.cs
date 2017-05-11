using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;

public class TextLengthValidator : MonoBehaviour, ITextValidator {

  MaterialInputField target;

  public int minLength;

  public void Init(MaterialInputField materialInputField) {
    target = materialInputField;
  }

  public bool IsTextValid() {
    if (target.inputField.text.Length > minLength) {
      return true;
    } else {
      target.validationText.text = "Can't be empty!";
      return false;
    }
  }
}
