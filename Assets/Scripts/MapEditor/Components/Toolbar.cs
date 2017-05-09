﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;
using UnityEngine.UI;
using Util;

/// <summary>
/// Provides access to the toolbar and ensures only one tool is selected at a
/// time.
/// </summary>
public class Toolbar : MonoBehaviour {
  public MaterialButton cursorButton;
  public MaterialButton aisleButton;
  public MaterialButton aisleAreaButton;
  public MaterialButton wallButton;
  public MaterialButton landmarkButton;
  public MaterialButton undoButton;
  public MaterialButton redoButton;
  public MaterialButton referenceImageButton;
  public MaterialButton zoomButton;

  // A tooltip object, to show user which tool is selected in text.
  public Text activeToolText;


  // The currently active tool.
  ToolType activeTool = ToolType.SelectionTool;

  // Convenience variable.
  readonly Dictionary<ToolType, MaterialButton> tool2Button = new Dictionary<ToolType, MaterialButton>();

  // The default normal color of the tools.
  public Color normalColor = new Color32(0x00, 0x00, 0x00, 0x8A);

  // The default highlight color of the tools.
  public Color highlightColor = new Color32(0x00, 0x96, 0x88, 0xFF);

  // Duration of tool switching animation.
  public float animationDuration = 0.3f;

  void Start() {
    // Injecting a function on all tools so only one is active at a time.
    MaterialButton[] tools =
    {
      cursorButton, aisleButton, aisleAreaButton, wallButton, landmarkButton
    };

    for (int i = 0; i < tools.Length; i++) {
      ToolType t = (ToolType)i;
      tool2Button[t] = tools[i];
      tools[i].GetComponent<Button>().onClick.AddListener(() => CheckTool(t));
    }

    CheckTool(ToolType.SelectionTool);
  }

  /// <summary>
  /// Ensures only one button is active.
  /// </summary>
  void CheckTool(ToolType t) {
    // Restore active tool color
    ToolType local = activeTool;

    TweenManager.TweenColor(v => tool2Button[local].iconColor = v, 
      tool2Button[local].iconColor, normalColor, animationDuration);

    // Highlight new tool
    TweenManager.TweenColor(v => tool2Button[t].iconColor = v, 
      tool2Button[t].iconColor, highlightColor, animationDuration);

    activeTool = t;

    // This is a bit of a hack, since I know each button has a Tooltip component
    activeToolText.text = tool2Button[t].GetComponent<Tooltip>().tooltip;
  }
}
