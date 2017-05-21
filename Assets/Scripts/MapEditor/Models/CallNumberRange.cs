using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace StackMaps {
  /// <summary>
  /// A call number range consists of a beginning and an ending. We should check
  /// whether beginning is less than the ending.
  /// </summary>
  public class CallNumberRange {
    CallNumber begin = new CallNumber();
    CallNumber end = new CallNumber();

    /// <summary>
    /// This is the specific collection tag for this call number. E.g. Oversized
    /// See CallNumberCollection to see a list of supported collections in this
    /// editor.
    /// </summary>
    public string collection = CallNumberCollection.Regular;

    /// <summary>
    /// Gets the beginning call number of this range.
    /// </summary>
    public CallNumber GetBegin() {
      return begin;
    }

    /// <summary>
    /// Sets the beginning call number of this range. Returns false if b is
    /// larger than end.
    /// </summary>
    /// <returns><c>true</c>, if begin was set, <c>false</c> otherwise.</returns>
    public bool SetBegin(CallNumber b) {
      if (b == null || b.CompareTo(end) > 0) {
        return false;
      }

      begin = b;
      return true;
    }

    /// <summary>
    /// Gets the ending call number of this range.
    /// </summary>
    public CallNumber GetEnd() {
      return end;
    }

    /// <summary>
    /// Sets the ending call number of this range. Returns false if b is
    /// smaller than begin.
    /// </summary>
    /// <returns><c>true</c>, if end was set, <c>false</c> otherwise.</returns>
    public bool SetEnd(CallNumber b) {
      if (b == null || b.CompareTo(begin) < 0) {
        return false;
      }

      end = b;
      return true;
    }

    /// <summary>
    /// This is for bookkeeping for two-sided aisles.
    /// </summary>
    public bool isSideA;

    /// <summary>
    /// Returns whether the call range has one side missing.
    /// </summary>
    /// <returns><c>true</c>, if incomplete, <c>false</c> otherwise.</returns>
    public bool IsIncomplete() {
      return string.IsNullOrEmpty(begin.ToString()) || string.IsNullOrEmpty(end.ToString());
    }

    public JSONNode ToJSON() {
      JSONObject root = new JSONObject();
      root["collection"] = collection ?? "";
      root["call_start"] = begin.ToString();
      root["call_end"] = end.ToString();
      root["side"] = isSideA ? 0 : 1;

      return root;
    }

    public void FromJSON(FloorController api, JSONNode root) {
      collection = root["collection"] ?? "";
      begin = new CallNumber(root["call_start"]);
      end = new CallNumber(root["call_end"]);
      isSideA = root["side"].AsInt == 0;
    }
  }
}
