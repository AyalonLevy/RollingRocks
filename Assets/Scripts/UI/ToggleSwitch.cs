// Typwriter inspired by Christina Creates Games https://github.com/Maraakis/ChristinaCreatesGames/blob/main/Toggle%20Switch%20System/ToggleSwitch.cs

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider Setup")]
    [SerializeField, Range(0.0f, 1.0f)] protected float sliderValue;

    public bool CurrentValue { get; private set; }

    private Slider _slider;

    [Header("Animation")]
    [SerializeField, Range(0.0f, 1.0f)] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine _animationSliderCoroutine;

    [Header("Events")]
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;

    protected Action transidionEffect;

    protected void OnValidate()
    {
        SetupToggleComponents();

        _slider.value = sliderValue;
    }

    private void SetupToggleComponents()
    {
        if (_slider != null)
            return;

        SetupSliderComponent();
    }

    private void SetupSliderComponent()
    {
        _slider = GetComponent<Slider>();

        if (_slider == null)
        {
            Debug.Log("[ToggleSwitch] No slider found!");
            return;
        }

        _slider.interactable = false;
        var sliderColors = _slider.colors;
        sliderColors.disabledColor = Color.white;
        _slider.colors = sliderColors;
        _slider.transition = Selectable.Transition.None;
    }

    protected virtual void Awake()
    {
        SetupToggleComponents();

        CurrentValue = GameManager.Instance.GetGameData().useWASD;
        sliderValue = CurrentValue ? 1 : 0;
        _slider.value = sliderValue;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    private void Toggle()
    {
        SetStateAndStartAnimation(!CurrentValue);
    }

    private void SetStateAndStartAnimation(bool state)
    {
        CurrentValue = state;

        if (CurrentValue)
        {
            onToggleOn?.Invoke();
        }
        else
        {
            onToggleOff?.Invoke();
        }

        if (_animationSliderCoroutine != null)
        {
            StopCoroutine(_animationSliderCoroutine);
        }

        _animationSliderCoroutine = StartCoroutine(AnimateSlider());
    }

    private IEnumerator AnimateSlider()
    {
        float startValue = _slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float elapsedTime = 0;
        if (animationDuration > 0)
        {
            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;

                float lerpFactor = slideEase.Evaluate(elapsedTime / animationDuration);
                _slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                transidionEffect?.Invoke();

                yield return null;
            }
        }

        _slider.value = endValue;
    }
}
