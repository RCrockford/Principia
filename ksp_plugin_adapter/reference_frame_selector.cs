﻿#define HAS_SURFACE
#define HAS_BODY_CENTRED_ALIGNED_WITH_PARENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace principia {
namespace ksp_plugin_adapter {

static class CelestialExtensions {
  public static bool is_leaf(this CelestialBody celestial) {
    return celestial.orbitingBodies.Count == 0;
  }

  public static bool is_root(this CelestialBody celestial) {
    return celestial.orbit == null;
  }
}

class ReferenceFrameSelector {
  public ReferenceFrameSelector(
      ref PrincipiaPluginAdapter.WindowRenderer window_renderer) {
    window_renderer += RenderWindow;
    expanded_ = new Dictionary<CelestialBody, bool>();
    foreach (CelestialBody celestial in FlightGlobals.Bodies) {
      if (!celestial.is_leaf() && !celestial.is_root()) {
        expanded_.Add(celestial, false);
      }
    }
    selected_celestial_ =
        FlightGlobals.currentMainBody ?? Planetarium.fetch.Sun;
    for (CelestialBody celestial = selected_celestial_;
         celestial.orbit != null;
         celestial = celestial.orbit.referenceBody) {
      if (!celestial.is_leaf()) {
        expanded_[celestial] = true;
      }
    }
  }

  public void RenderButton() {
    var old_skin = UnityEngine.GUI.skin;
    UnityEngine.GUI.skin = null;
    if (UnityEngine.GUILayout.Button("Reference frame selection...")) {
      show_selector_ = !show_selector_;
    }
    UnityEngine.GUI.skin = old_skin;
  }

  public void RenderWindow() {
    var old_skin = UnityEngine.GUI.skin;
    UnityEngine.GUI.skin = null;
    if (show_selector_) {
      window_rectangle_ = UnityEngine.GUILayout.Window(
                              id         : this.GetHashCode(),
                              screenRect : window_rectangle_,
                              func       : RenderSelector,
                              text       : "Reference frame selection");
    }
    UnityEngine.GUI.skin = old_skin;
  }

  public void RenderSelector(int window_id) {
    var old_skin = UnityEngine.GUI.skin;
    UnityEngine.GUI.skin = null;

    UnityEngine.GUILayout.BeginHorizontal();

    // Left-hand side: tree view for celestial selection.
    UnityEngine.GUILayout.BeginVertical(UnityEngine.GUILayout.Width(200));
    RenderSubtree(celestial : Planetarium.fetch.Sun, depth : 0);
    UnityEngine.GUILayout.EndVertical();
    
    // Right-hand side: toggles for reference frame type selection.
    UnityEngine.GUILayout.BeginVertical();
    TypeSelector(FrameType.BODY_CENTRED_NON_ROTATING,
                 "Non-rotating reference frame fixing the centre of " +
                     selected_celestial_.theName);
#if HAS_SURFACE
    TypeSelector(FrameType.SURFACE,
                 "Reference frame of the surface of " +
                     selected_celestial_.theName);
#endif
    if (!selected_celestial_.is_root()) {
      CelestialBody parent = selected_celestial_.orbit.referenceBody;
      TypeSelector(FrameType.BARYCENTRIC_ROTATING,
                   "Reference frame fixing the barycentre of " +
                       selected_celestial_.theName + " and " + parent.theName +
                       " and the line between them");
#if HAS_BODY_CENTRED_ALIGNED_WITH_PARENT
      TypeSelector(FrameType.BODY_CENTRED_ALIGNED_WITH_PARENT,
                   "Reference frame fixing the centre of " +
                       selected_celestial_.theName + " and the line between " +
                       selected_celestial_.theName + " and " + parent.theName);
#endif
    }
    UnityEngine.GUILayout.EndVertical();

    UnityEngine.GUILayout.EndHorizontal();

    UnityEngine.GUI.DragWindow(
        position : new UnityEngine.Rect(left : 0f, top : 0f, width : 10000f,
                                        height : 10000f));

    UnityEngine.GUI.skin = old_skin;
  }

  private enum FrameType {
    BODY_CENTRED_NON_ROTATING,
#if HAS_SURFACE
    SURFACE,
#endif
    BARYCENTRIC_ROTATING,
#if HAS_BODY_CENTRED_ALIGNED_WITH_PARENT
    BODY_CENTRED_ALIGNED_WITH_PARENT
#endif
  }

  private void RenderSubtree(CelestialBody celestial, int depth) {
    // Horizontal offset between a node and its children.
    const int offset = 20;
    UnityEngine.GUILayout.BeginHorizontal();
    if (!celestial.is_root()) {
      UnityEngine.GUILayout.Label(
          "",
          UnityEngine.GUILayout.Width(offset * (depth - 1)));
      string button_text;
      if (celestial.is_leaf()) {
        button_text = "";
      } else if (expanded_[celestial]) {
        button_text = "-";
      } else {
        button_text = "+";
      }
      if (UnityEngine.GUILayout.Button(button_text,
                                       UnityEngine.GUILayout.Width(offset))) {
        Shrink();
        expanded_[celestial] = !expanded_[celestial];
      }
    }
    if (UnityEngine.GUILayout.Toggle(selected_celestial_ == celestial,
                                     celestial.name)) {
      selected_celestial_ = celestial;
    }
    UnityEngine.GUILayout.EndHorizontal();
    if (celestial.is_root() || (!celestial.is_leaf() && expanded_[celestial])) {
      foreach (CelestialBody child in celestial.orbitingBodies) {
        RenderSubtree(child, depth + 1);
      }
    }
  }

  private void TypeSelector(FrameType value, string text) {
   bool old_wrap = UnityEngine.GUI.skin.toggle.wordWrap;
   UnityEngine.GUI.skin.toggle.wordWrap = true;
   if (UnityEngine.GUILayout.Toggle(selected_type_ == value,
                                    text,
                                    UnityEngine.GUILayout.Width(150),
                                    UnityEngine.GUILayout.Height(75))) {
      selected_type_ = value;
    }
    UnityEngine.GUI.skin.toggle.wordWrap = old_wrap;
  }

  private void Shrink() {
    window_rectangle_.height = 0.0f;
    window_rectangle_.width = 0.0f;
  }

  private bool show_selector_;
  private UnityEngine.Rect window_rectangle_;
  private Dictionary<CelestialBody, bool> expanded_;
  private CelestialBody selected_celestial_;
  private FrameType selected_type_ = FrameType.BODY_CENTRED_NON_ROTATING;
}

}  // namespace ksp_plugin_adapter
}  // namespace principia
