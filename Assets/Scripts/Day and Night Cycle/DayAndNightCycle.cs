using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using Object = UnityEngine.Object;


public class DayAndNightCycle : MonoBehaviour
{
    [System.Serializable]
    public struct DayAndNightMark
    {
        public float timeRatio; //Entre 0 et 1
        public Color color;
        public float intensity;
    }

    [SerializeField] private DayAndNightMark[] _marks;
    [SerializeField] private float _cycleLenght = 24; //In Second
    [SerializeField] private Light2D _light;

    private float _currentCycleTime;
    private int _currentMarkIndex, _nextMarkIndex;
    private float _currentMarkTime, _nextMarkTime;

    // Start is called before the first frame update
    void Start()
    {
        _currentMarkIndex = -1;
        CycleMarks();
    }

    private void CycleMarks()
    {
        _currentMarkIndex = (_currentMarkIndex + 1) % _marks.Length;
        _nextMarkIndex = (_currentMarkIndex + 1) % _marks.Length;
        _currentMarkTime = _marks[_currentMarkIndex].timeRatio * _cycleLenght;
        _nextMarkTime = _marks[_nextMarkIndex].timeRatio * _cycleLenght;
    }

    // Update is called once per frame
    void Update()
    {
        _currentCycleTime = (_currentCycleTime + Time.deltaTime) % _cycleLenght;
        
        //Est-ce qu'un marque a été passer
        if (Math.Abs(_nextMarkTime - _currentCycleTime) < 0.1f)
        {
            DayAndNightMark next = _marks[_nextMarkIndex];
            _light.color = next.color;
            _light.intensity = next.intensity;
            
            CycleMarks();
        }
    }
}
