using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaps {
  /// <summary>
  /// A call numbe range consists of a beginning and an ending. We should check
  /// whether beginning is less than the ending.
  /// </summary>
  public class CallNumberRange {
    public CallNumber begin;
    public CallNumber end;

    /// <summary>
    /// This is for bookkeeping for two-sided aisles.
    /// </summary>
    public bool isSideA;
  }
}
