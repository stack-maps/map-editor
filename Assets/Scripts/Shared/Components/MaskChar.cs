using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple script used to replace the default InputField password character
/// from asterisk to whatever we want.
/// </summary>
public class MaskChar : MonoBehaviour {
  public char customMaskChar = '●';

  void Start() {
    GetComponent<InputField>().asteriskChar = customMaskChar;
  }
}
