using System.Collections.Generic;

namespace Util {
  /// <summary>
  /// An interface to set up information to be displayed.
  /// </summary>
  public interface ITooltipDisplayer {
    /// <summary>
    /// Setups the tooltip view using the given string.
    /// </summary>
    /// <param name="userInfo">User info containing data to set up tooltip.</param>
    void SetupTooltip(Dictionary<object, object> userInfo);
  }
}
