using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;
using UnityEngine.UI;

/// <summary>
/// This script is used to toggle chevron button expansion.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(Button))]
public class ExpandButton : MonoBehaviour {
  public RectTransform targetGraphic;

  public bool isExpanded;

  public float duration = 0.3f;

  int animId = -1;

  void Start() {
    if (targetGraphic != null) {
      targetGraphic.localEulerAngles = calculateTargetRotation();
    }
  }

  public void Toggle() {
    isExpanded = !isExpanded;
    Vector3 targetRotation = calculateTargetRotation();

    if (animId > -1)
      TweenManager.EndTween(animId);

    animId = TweenManager.TweenVector3(v => targetGraphic.localEulerAngles = v, 
      targetGraphic.localEulerAngles, targetRotation, duration);
  }

  Vector3 calculateTargetRotation() {
    // The goal should be the closest -90 deg. or closest 0 deg. based on
    // current rotation.
    float currZ = targetGraphic.localEulerAngles.z;
    float noExpZ = Mathf.Round(currZ / 360) * 360;
    float expZ = Mathf.Round((currZ + 90) / 360) * 360 - 90;

    return isExpanded ? new Vector3(0, 0, expZ) : new Vector3(0, 0, noExpZ);
  }
}
