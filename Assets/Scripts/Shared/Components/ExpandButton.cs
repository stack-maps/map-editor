using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;
using UnityEngine.UI;

/// <summary>
/// This script is used to toggle chevron button expansion.
/// </summary>

[RequireComponent(typeof(Button))]
public class ExpandButton : MonoBehaviour {
  public RectTransform targetGraphic;

  public bool isExpanded;

  public float duration = 0.15f;

  int animId = -1;

  public void Toggle() {
    isExpanded = !isExpanded;

    // The goal should be the closest -90 deg. or closest 0 deg. based on
    // current rotation.
    float currZ = targetGraphic.localEulerAngles.z;
    float noExpZ = Mathf.Round(currZ / 360) * 360;
    float expZ = Mathf.Round((currZ + 90) / 360) * 360 - 90;


    Vector3 targetRotation = isExpanded ? new Vector3(0, 0, expZ) : new Vector3(0, 0, noExpZ);

    if (animId > -1)
      TweenManager.EndTween(animId);

    animId = TweenManager.TweenVector3(v => targetGraphic.localEulerAngles = v, 
      targetGraphic.localEulerAngles, targetRotation, duration);

  }

  void Start() {
    GetComponent<Button>().onClick.AddListener(Toggle);
  }
  
}
