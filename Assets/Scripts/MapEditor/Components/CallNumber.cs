using System;
using System.Text.RegularExpressions;

namespace StackMaps {
  /// <summary>
  /// Represents a call number. In our case, call number is divided into four
  /// sections, CLASS, SUBCLASS, CUTTER1 and CUTTER2.
  /// </summary>
  public class CallNumber : IComparable {
    /// <summary>
    /// The class part. This consists of letters only.
    /// </summary>
    string cnClass = "";

    /// <summary>
    /// The subclass part. This consists of a number.
    /// </summary>
    string cnSubclass = "";

    /// <summary>
    /// Cutter numbers consist of ".LETTERSNUMBERS".
    /// </summary>
    string cnCutter1 = "";

    /// <summary>
    /// Just in case we need another one.
    /// </summary>
    string cnCutter2 = "";

    public string GetClass() {
      return cnClass;
    }

    public string GetSubclass() {
      return cnSubclass;
    }

    public string GetCutter1() {
      return cnCutter1;
    }

    public string GetCutter2() {
      return cnCutter2;
    }

    /// <summary>
    /// Sets the class. If the given input is invalid, does nothing.
    /// </summary>
    /// <returns><c>true</c>, if class was set, <c>false</c> otherwise.</returns>
    public bool SetClass(string c) {
      if (Regex.IsMatch(c, @"^[a-zA-Z]+$")) {
        cnClass = c;
        return true;
      }

      return false;
    }

    /// <summary>
    /// Sets the subclass. If the given input is invalid, does nothing.
    /// </summary>
    /// <returns><c>true</c>, if subclass was set, <c>false</c> otherwise.</returns>
    public bool SetSubclass(string c) {
      float f;

      if (float.TryParse(c, out f)) {
        cnSubclass = c;
        return true;
      }

      return false;
    }

    /// <summary>
    /// Sets the first cutter. If the given input is invalid, does nothing.
    /// </summary>
    /// <returns><c>true</c>, if cutter1 was set, <c>false</c> otherwise.</returns>
    public bool SetCutter1(string c) {
      if (Regex.IsMatch(c, @"^\.[a-zA-Z]+[0-9]+$")) {
        cnCutter1 = c;
        return true;
      }

      return false;
    }

    /// <summary>
    /// Sets the second cutter. If the given input is invalid, does nothing.
    /// </summary>
    /// <returns><c>true</c>, if cutter2 was set, <c>false</c> otherwise.</returns>
    public bool SetCutter2(string c) {
      if (Regex.IsMatch(c, @"^\.[a-zA-Z]+[0-9]+$")) {
        cnCutter2 = c;
        return true;
      }

      return false;
    }

    #region IComparable implementation

    public int CompareTo(object obj) {
      if (obj == null)
        return 1;

      CallNumber other = obj as CallNumber;

      if (other != null) {
        // We first compare class.
        int classComp = cnClass.CompareTo(other.cnClass);

        if (classComp != 0) {
          return classComp;
        }

        // Then we compare subclass.

        if ((HasSubclass() && !other.HasSubclass())) {
          return 1;
        } else if ((!HasSubclass() && other.HasSubclass())) {
          return -1;
        } else if (!HasSubclass() && !other.HasSubclass()) {
          return 0;
        } else {
          int result = cnSubclass.CompareTo(other.cnSubclass);

          if (result != 0) {
            return result;
          }
        }

        // Then we compare the cutters.
        if ((HasCutter1() && !other.HasCutter1())) {
          return 1;
        } else if ((!HasSubclass() && other.HasSubclass())) {
          return -1;
        } else if (!HasCutter1() && !other.HasCutter1()) {
          return 0;
        } else {
          int result = LetterPart(cnCutter1).CompareTo(LetterPart(other.cnCutter1));

          if (result != 0) {
            return result;
          }

          if (NumberPart(cnCutter1) > NumberPart(other.cnCutter1)) {
            return 1;
          } else if (NumberPart(cnCutter1) < NumberPart(other.cnCutter1)) {
            return -1;
          }
        }

        if ((HasCutter2() && !other.HasCutter2())) {
          return 1;
        } else if ((!HasSubclass() && other.HasSubclass())) {
          return -1;
        } else if (!HasCutter2() && !other.HasCutter2()) {
          return 0;
        } else {
          int result = LetterPart(cnCutter2).CompareTo(LetterPart(other.cnCutter2));

          if (result != 0) {
            return result;
          }

          if (NumberPart(cnCutter2) > NumberPart(other.cnCutter2)) {
            return 1;
          } else if (NumberPart(cnCutter2) < NumberPart(other.cnCutter2)) {
            return -1;
          }
        }

        return 0;
      } else {
        throw new ArgumentException("Object is not a call number");
      }
    }

    /// <summary>
    /// Returns the letter part of a cutter number. Assumed valid.
    /// </summary>
    string LetterPart(string cutter) {
      Match m = Regex.Match(cutter, @"[a-zA-Z]+");
      return m.Value;
    }

    /// <summary>
    /// Returns the number part of a cutter number. Assumed valid.
    /// </summary>
    float NumberPart(string cutter) {
      Match m = Regex.Match(cutter, @"[0-9]+");
      return float.Parse("0." + m.Value);
    }

    #endregion

    /// <summary>
    /// Whether this call number has a class defined.
    /// </summary>
    public bool HasClass() {
      return cnClass.Length > 0;
    }

    /// <summary>
    /// Whether this call number has a subclass defined.
    /// </summary>
    public bool HasSubclass() {
      return cnSubclass.Length > 0;
    }

    /// <summary>
    /// Whether this call number has the first cutter defined.
    /// </summary>
    public bool HasCutter1() {
      return cnCutter1.Length > 0;
    }

    /// <summary>
    /// Whether this call number has the second cutter defined.
    /// </summary>
    public bool HasCutter2() {
      return cnCutter2.Length > 0;
    }
  }
}
