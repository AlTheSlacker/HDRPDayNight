using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DayNight
{
    [CreateAssetMenu(fileName = "DayNightData", menuName = "DayNightData")]
    public class DayNightData : ScriptableObject
    {
        [SerializeField] private float fractionOfDayForSunMoon;
        [SerializeField] private float timeOfDayHours;
        [SerializeField] private float timeOfDayRealTime;
        [SerializeField] private int dayOfMonth;
        [SerializeField] private int month;
        [SerializeField] private int year;

        public float FractionOfDayForSunMoon { get => fractionOfDayForSunMoon; set => fractionOfDayForSunMoon = value; }
        public float TimeOfDayHours { get => timeOfDayHours; set => timeOfDayHours = value; }
        public float TimeOfDayRealTime { get => timeOfDayRealTime; set => timeOfDayRealTime = value; }
        public int DayOfMonth { get => dayOfMonth; set => dayOfMonth = value; }
        public int Month { get => month; set => month = value; }
        public int Year { get => year; set => year = value; }
    }
}