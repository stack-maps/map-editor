using System.Collections;
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

  // The currently active tool.
  ToolType activeTool = ToolType.SelectionTool;

  // Convenience variable.
  Dictionary<ToolType, MaterialButton> tool2Button;

  // The default normal color of the tools.
  public Color normalColor = new Color32(0x00, 0x00, 0x00, 0x8A);

  // The default highlight color of the tools.
  public Color highlightColor = new Color32(0x00, 0x96, 0x88, 0xFF);

  // Duration of tool switching animation.
  public float animationDuration = 0.3f;

  // The tooltip for the active tool.
  public Text activeToolText;

  void Start() {
    // Injecting a function on all tools so only one is active at a time.
    MaterialButton[] tools =
    {
      cursorButton, aisleButton, aisleAreaButton, wallButton, landmarkButton
    };

    tool2Button = new Dictionary<ToolType, MaterialButton>();

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
    // Restore active tool
    ToolType local = activeTool;
    tool2Button[local].canvasGroup.interactable = true;
    tool2Button[local].materialRipple.enabled = true;

    TweenManager.TweenColor(v => tool2Button[local].iconColor = v, 
      tool2Button[local].iconColor, normalColor, animationDuration);

    // Highlight new tool
    TweenManager.TweenColor(v => tool2Button[t].iconColor = v, 
      tool2Button[t].iconColor, highlightColor, animationDuration);

    activeTool = t;
    activeToolText.text = tool2Button[t].GetComponent<Tooltip>().tooltip;
    tool2Button[activeTool].canvasGroup.interactable = false;
    tool2Button[activeTool].materialRipple.enabled = false;
  }

  /// <summary>
  /// Gets the active tool.
  /// </summary>
  /// <returns>The active tool.</returns>
  public ToolType GetActiveTool() {
    return activeTool;
  }
}
