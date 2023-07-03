using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThrustersSlider : MonoBehaviour
{
    [SerializeField] Slider _thrustersSlider;
    [SerializeField] private float _maxThrusters;
    [SerializeField] private float _minThrusters;
    [SerializeField] private float _currentThrusters;
    Coroutine _corou = null;
    private bool _thrustersCharging = false;
    

    void Start()
    {
        _maxThrusters = 100.0f;
        _minThrusters = 0.0f;
    }

    public void SetThrusters(float maxThrusterValue)
    {
        _maxThrusters = maxThrusterValue;

        _currentThrusters = _maxThrusters;
    }

    public void BurnThrusters(float amount)
    {
        _thrustersCharging = false;
        _currentThrusters -= amount;// *Time.deltaTime;
        if (_currentThrusters < _minThrusters)
        {
            _currentThrusters = 0;
        }
    }

    public void RechargeThrusters(float amount)
    { 
        _corou = StartCoroutine(RechargeThrustersCo(amount));
    }

    IEnumerator RechargeThrustersCo(float amount)
    {
        _thrustersCharging = true;
        if (_currentThrusters <= 0.0f)
        {
            yield return new WaitForSeconds(5.0f);
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
        }

        while (_currentThrusters <= _maxThrusters)
        {
            _currentThrusters += amount;
            yield return new WaitForSeconds(Time.deltaTime);
            UpdateThrustersUI();
        }
        _thrustersCharging = false;
    }

    public void UpdateThrustersUI()
    {
        _thrustersSlider.value = _currentThrusters / _maxThrusters;
    }

    public bool AreThrustersCharging()
    {
        return _thrustersCharging;
    }
        
}
