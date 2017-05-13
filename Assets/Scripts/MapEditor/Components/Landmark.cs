using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialUI;
using System.Linq;
using UnityEngine.EventSystems;
using SimpleJSON;
using System;

namespace StackMaps {
  /// <summary>
  /// Represents a landmark on the screen. This is a bordered box with an icon on 
  /// top.
  /// </summary>
  public class Landmark : MonoBehaviour {
    public VectorImage icon;

    public LandmarkType landmarkType;

    Dictionary<LandmarkType, ImageData> landmarkIcons;

    void Awake() {
      landmarkIcons = new Dictionary<LandmarkType, ImageData>
      {
        { LandmarkType.Stairs, GetIconFromIconFont("CommunityMD", "stairs") },
        {
          LandmarkType.Elevator,
          GetIconFromIconFont("CommunityMD", "elevator")
        },
        {
          LandmarkType.Restroom,
          GetIconFromIconFont("CommunityMD", "human_male_female")
        }
      };
    }

    void Update() {
      if (icon != null && landmarkIcons[landmarkType] != null)
        icon.SetImage(landmarkIcons[landmarkType]);
    }

    static ImageData GetIconFromIconFont(string fontName, string iconName) {
      VectorImageSet iconSet = VectorImageManager.GetIconSet(fontName);
      Glyph glyph = iconSet.iconGlyphList.FirstOrDefault(x => x.name.ToLower().Equals(iconName.ToLower()));

      if (glyph == null) {
        Debug.LogError("Could not find an icon with the name: " + iconName + " inside the " + fontName + " icon font");
        return null;
      }

      Font font = VectorImageManager.GetIconFont(fontName);

      return new ImageData(new VectorImageData(glyph, font));
    }

    public JSONNode ToJSON() {
      JSONObject root = new JSONObject();
      Rectangle r = GetComponent<Rectangle>();
      root["center_x"] = r.GetCenter().x;
      root["center_y"] = r.GetCenter().y;
      root["length"] = r.GetSize().x;
      root["width"] = r.GetSize().y;
      root["rotation"] = r.GetRotation();
      root["lname"] = landmarkType.ToString();

      return root;
    }

    public void FromJSON(FloorController api, JSONNode root) {
      Rectangle r = GetComponent<Rectangle>();
      r.SetCenter(new Vector2(root["center_x"].AsFloat, root["center_y"].AsFloat));
      r.SetSize(new Vector2(root["length"].AsFloat, root["width"].AsFloat));
      r.SetRotation(root["rotation"].AsFloat);
      LandmarkType t;

      try {
        t = (LandmarkType)Enum.Parse(typeof(LandmarkType), root["lname"]);
      } catch (ArgumentException ex) {
        t = LandmarkType.Restroom;
      }
    }
  }
}