using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public Image HealthImage;

    public int MinHp;
    public int MaxHp;
    private int _currentValue;
    private float _currentPercent;
    
    public void SetHealth(int health)
    {
        
            _currentValue = health;
            _currentPercent = (float)_currentValue / (float)(MaxHp - MinHp);
        
        HealthImage.fillAmount = _currentPercent;
    }

    public float CurrentPercent
    {
        get { return _currentPercent; }
    }
    public float CurrentValue
    {
        get { return _currentValue; }
    }

    void Awake()
    {
        Instance = this;
    }
}
