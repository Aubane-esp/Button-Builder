﻿using UnityEngine;
using System.Collections;
using Leap.Unity.UI.Interaction;
using Leap.Unity.GraphicalRenderer;

public class MainButtonController : MonoBehaviour {

  [Header("Color")]
  [SerializeField]
  private Color _defaultBaseColor;

  [SerializeField]
  private Color _defaultHoverColor;

  [SerializeField]
  private Color _defaultPressColor;

  [SerializeField]
  private Color _defaultToggleColor;

  [SerializeField]
  private LeapGraphic _svSwatchGraphic;

  [SerializeField]
  private InteractionToggle _baseColorToggle;

  [SerializeField]
  private InteractionToggle _hoverColorToggle;

  [SerializeField]
  private InteractionToggle _pressColorToggle;

  [SerializeField]
  private InteractionToggle _toggleColorToggle;

  [SerializeField]
  private InteractionSlider _saturationValueSlider;

  [SerializeField]
  private InteractionSlider _hueSlider;

  //Colors are stored in HSV
  private Vector3 _baseColor;
  private Vector3 _hoverColor;
  private Vector3 _pressColor;
  private Vector3 _toggleColor;

  private CustomVectorChannelData _svSwatchChannel;

  public Vector3 baseColor { get { return _baseColor; } }
  public Vector3 hoverColor { get { return _hoverColor; } }
  public Vector3 pressColor { get { return _pressColor; } }
  public Vector3 toggleColor { get { return _toggleColor; } }

  private void Awake() {
    _baseColor = _defaultBaseColor.ToHSV();
    _hoverColor = _defaultHoverColor.ToHSV();
    _pressColor = _defaultPressColor.ToHSV();
    _toggleColor = _defaultToggleColor.ToHSV();

    _svSwatchChannel = _svSwatchGraphic.GetFirstFeatureData<CustomVectorChannelData>();
  }

  private IEnumerator Start() {
    yield return null;
    OnChangeColorToggle();
  }

  private void Update() {
    if (Time.frameCount > 5) {
      applyColorDelegate((ref Vector3 hsv) => {
        hsv.x = _hueSlider.VerticalSliderValue;
        hsv.y = _saturationValueSlider.HorizontalSliderValue;
        hsv.z = _saturationValueSlider.VerticalSliderValue;

        _svSwatchChannel.value = new Vector4(hsv.x, 1, 1, 1);
      });
    }
  }

  /// <summary>
  /// Called whenever the user presses a color toggle.  Within this method we 
  /// make sure the hue, saturation, and value sliders are all in the correct
  /// positions to match the color the user selected.
  /// </summary>
  public void OnChangeColorToggle() {
    applyColorDelegate((ref Vector3 hsv) => {
      _hueSlider.VerticalSliderValue = hsv.x;
      _saturationValueSlider.HorizontalSliderValue = hsv.y;
      _saturationValueSlider.VerticalSliderValue = hsv.z;
    });
  }

  private void applyColorDelegate(ColorDelegate colorDelegate) {
    if (_baseColorToggle.toggled) {
      colorDelegate(ref _baseColor);
    } else if (_hoverColorToggle.toggled) {
      colorDelegate(ref _hoverColor);
    } else if (_pressColorToggle.toggled) {
      colorDelegate(ref _pressColor);
    } else if (_toggleColorToggle.toggled) {
      colorDelegate(ref _toggleColor);
    } else {
      Debug.LogWarning("No color toggle is currently toggled!");
    }
  }

  private delegate void ColorDelegate(ref Vector3 hsv);
}
