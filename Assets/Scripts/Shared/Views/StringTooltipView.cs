using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Util {
  public class StringTooltipView : MonoBehaviour, ITooltipDisplayer {
    public Text tooltipLabel;

    #region ITooltipDisplayer implementation

    public void SetupTooltip(Dictionary<object, object> userInfo) {
      tooltipLabel.text = userInfo["tooltip"] as string;
    }

    #endregion
  }
}
