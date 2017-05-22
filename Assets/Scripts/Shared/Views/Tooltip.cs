using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Util {
  public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    /// <summary>
    /// The amount of time before we show the tooltip.
    /// </summary>
    public float hoverTime = 0.25f;

    /// <summary>
    /// The tooltip to display about this component.
    /// </summary>
    [TextArea]
    public string tooltip = "Nothing interesting happens.";

    /// <summary>
    /// The offset applied to tooltips.
    /// </summary>
    public Vector2 offset;

    /// <summary>
    /// Whether the tooltip will move with mouse movement.
    /// </summary>
    public bool moveWithMouse = false;

    /// <summary>
    /// Extra info for tooltip display.
    /// </summary>
    public Dictionary<object, object> info = new Dictionary<object, object>();

    /// <summary>
    /// The place to attach the tooltip.
    /// </summary>
    Transform tooltipViewObject;

    /// <summary>
    /// The scale of input mouse position.
    /// </summary>
    float scale;

    /// <summary>
    /// The formatted text box that will take care of displaying the text.
    /// </summary>
    public GameObject tooltipPrefab;

    GameObject tooltipInstance;

    float passedTime = 0;

    bool isHovering = false;

    void Start() {
      tooltipViewObject = GameObject.Find("TooltipView").transform;
    }

    void Update() {
      if (isHovering) {
        passedTime += Time.deltaTime;
      } else if (tooltipInstance) {
        Destroy(tooltipInstance);
      }

      if (passedTime > hoverTime) {
        if (!tooltipInstance) {
          // We instantiate a text prefab.
          tooltipInstance = Instantiate(tooltipPrefab);

          // Add it under our transform, and set up text.
          tooltipInstance.transform.localScale = Vector3.one;
          tooltipInstance.transform.SetParent(tooltipViewObject, false);

          scale = GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;

          info["tooltip"] = tooltip;

          tooltipInstance.GetComponent<ITooltipDisplayer>().SetupTooltip(info);

          UpdateTooltipPosition();
        }

        if (moveWithMouse)
          UpdateTooltipPosition();
      }
    }

    void UpdateTooltipPosition() {
      // Now we move the object based on mouse position. - We can prevent off
      // screen here!
      Vector2 pos = (Vector2)Input.mousePosition / scale + offset;
      Vector2 size = ((RectTransform)tooltipInstance.transform).sizeDelta;

      Vector2 upRight = (pos + size) * scale;
      Vector2 upLeft = (new Vector2(pos.x - size.x, pos.y + size.y)) * scale;
      Vector2 downLeft = (pos - size) * scale;
      if (upRight.x <= Screen.width && upRight.y <= Screen.height) {
        ((RectTransform)(tooltipInstance.transform)).anchoredPosition = pos;
      } else if (upLeft.x >= 0 && upRight.y <= Screen.height) {
        pos.x -= size.x;
        ((RectTransform)(tooltipInstance.transform)).anchoredPosition = pos;
      } else if (downLeft.x >= 0 && downLeft.y >= 0) {
        ((RectTransform)(tooltipInstance.transform)).anchoredPosition = pos - size;
      } else {
        pos.y -= size.y;
        ((RectTransform)(tooltipInstance.transform)).anchoredPosition = pos;
      }
    }

    #region IPointerEnterHandler implementation

    public void OnPointerEnter(PointerEventData eventData) {
      isHovering = true;
    }

    #endregion

    #region IPointerExitHandler implementation

    public void OnPointerExit(PointerEventData eventData) {
      isHovering = false;
      passedTime = 0;
    }

    #endregion

  }
}
