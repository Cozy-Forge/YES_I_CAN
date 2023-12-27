using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPollutionDegree : MonoBehaviour
{
    private float _currentPollutionDegree = 0f;
    private float _maxPollutionDegree = 100f;

    [Header("Pollution UI")]

    [SerializeField]
    private Slider _infoSlider;
    [SerializeField]
    private Image _pollutionEffectUI;
    private float _maxPollutionEffectValue = 0.5f;
    private float _endPollutionEffectValue = 0.8f;

    [SerializeField]
    private GameObject _diePanel;

    public bool IsPolluted { get; private set; } = false;

    public void Polluted(float value)
    {
        if (IsPolluted) return;

        _currentPollutionDegree += value;
        float val = (_currentPollutionDegree / _maxPollutionDegree);
        if (_infoSlider != null)
            _infoSlider.value = val;
        if (_pollutionEffectUI != null)
            _pollutionEffectUI.color = new Color(0, 0, 0, Mathf.Lerp(0, _maxPollutionEffectValue, val));

        if (_currentPollutionDegree >= _maxPollutionDegree)
            Die();
    }

    private void Die()
    {
        IsPolluted = true;

        if(_pollutionEffectUI != null)
            _pollutionEffectUI.color = new Color(0, 0, 0, _endPollutionEffectValue);
        _diePanel?.SetActive(true);
    }
}
