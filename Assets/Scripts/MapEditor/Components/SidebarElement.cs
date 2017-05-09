using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;
using UnityEngine.UI;

namespace StackMaps {
  /// <summary>
  /// An abstract editor, useful to define some basic methods here.
  /// </summary>
  public class SidebarElement : MonoBehaviour {
    public float animationDuration = 0.3f;

    // We either have a layout element ourselves, or we have a child that does!
    // This is a tree!
    LayoutElement heightConstraint;
    float defaultHeight;

    protected void Awake() {
      heightConstraint = GetComponent<LayoutElement>();

      if (heightConstraint != null)
        defaultHeight = heightConstraint.minHeight;
    }

    /// <summary>
    /// Sets the visibility of the editor with an optional animation.
    /// </summary>
    /// <param name="shouldShow">If set to <c>true</c> editor is visible.</param>
    /// <param name="animated">If set to <c>true</c> this is animated.</param>
    public void Show(bool shouldShow, bool animated) {
      if (heightConstraint != null) {
        if (animated) {
          TweenManager.TweenFloat(v => heightConstraint.minHeight = v, 
            heightConstraint.minHeight, shouldShow ? defaultHeight : 0, 
            animationDuration);
        } else {
          heightConstraint.minHeight = shouldShow ? defaultHeight : 0;
        }
      } else {
        // Our children is responsible for animating us!
        for (int i = 0; i < transform.childCount; i++) {
          Transform child = transform.GetChild(i);

          SidebarElement childEditor = child.GetComponent<SidebarElement>();

          if (childEditor) {
            childEditor.Show(shouldShow, animated);
          }
        }
      }
    }
  }
}