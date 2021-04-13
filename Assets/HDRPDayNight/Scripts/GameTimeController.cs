using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DayNight
{

    public class GameTimeController : MonoBehaviour
    {
        [SerializeField] private DayNightData dayNightData;
        [Tooltip("How long is a game day in real time seconds")]
        [SerializeField] private int realTimeSecondsPerDay = 3600;
        [Tooltip("Scale factor for how much faster time at night passes compared to day.")]
        [SerializeField] private float nightTimeScale = 4;
        [Space][Header("Sunrise Using 24 Hour Clock")]
        [SerializeField] private int sunriseHour = 5;
        [SerializeField] private int sunriseMinute = 0;
        [Space][Header("Sunset Using 24 Hour Clock")]
        [SerializeField] private int sunsetHour = 19;
        [SerializeField] private int sunsetMinute = 0;
        [Space][Header("Start Date")]
        [SerializeField] private int startDay = 1;
        [SerializeField] private int startMonth = 1;
        [SerializeField] private int startYear = 1;
        [Space]
        [Header("Start Time Using 24 Hour Clock")]
        [SerializeField] private int startHour = 16;
        [SerializeField] private int startMinute = 0;

        private const float minutesInHour = 60;
        private const int hoursInDay = 24;
        private const int daysInMonth = 30;
        private const int monthsInYear = 12;
        private const float dawnFraction = 0.25f;
        private const float duskFraction = 0.75f;
        private const float dayFraction = 0.5f;
        private const float eveningFraction = 0.25f;

        private float realTime;
        private float dawnHours, duskHoursUntilEnd, duskHours, dayLightHours;
        private float dawnRealTime, duskRealTime, dayLightRealTime, duskRealTimeUntilEnd;


        void Awake()
        {
            dayNightData.DayOfMonth = startDay;
            dayNightData.Month = startMonth;
            dayNightData.Year = startYear;

            dawnHours = sunriseHour + (sunriseMinute / minutesInHour);
            duskHours = sunsetHour + (sunsetMinute / minutesInHour);
            duskHoursUntilEnd = hoursInDay - duskHours;
            dawnRealTime = dawnHours / hoursInDay / nightTimeScale * realTimeSecondsPerDay;
            duskRealTime = realTimeSecondsPerDay - (duskHoursUntilEnd / hoursInDay / nightTimeScale * realTimeSecondsPerDay);
            dayLightHours = duskHours - dawnHours;
            dayLightRealTime = duskRealTime - dawnRealTime;
            duskRealTimeUntilEnd = realTimeSecondsPerDay - duskRealTime;

            dayNightData.TimeOfDayHours = startHour + startMinute / minutesInHour;
            realTime = GetRealTimeFromHours(dayNightData.TimeOfDayHours);
        }


        void Update()
        {
            SetDayNightData();
        }


        void SetDayNightData()
        {
            realTime += Time.deltaTime;
            if (realTime > realTimeSecondsPerDay)
            {
                realTime = 0;
                dayNightData.DayOfMonth++;
                ValidateDayNightData();
            }

            dayNightData.FractionOfDayForSunMoon = GetFractionOfDayFromRealTime(realTime);
            dayNightData.TimeOfDayHours = GetHoursFromRealTime(realTime);
            dayNightData.TimeOfDayRealTime = realTime;
        }


        float GetFractionOfDayFromRealTime(float time)
        {
            if (time <= dawnRealTime)
            {
                return (time / dawnRealTime) * dawnFraction;
            }
            else if (time <= duskRealTime)
            {
                return dawnFraction + (time - dawnRealTime) / dayLightRealTime * dayFraction;
            }
            else
            {
                return duskFraction + (time - duskRealTime) / duskRealTimeUntilEnd * eveningFraction;
            }
        }


        float GetRealTimeFromHours(float hours)
        {
            if(hours <= dawnHours)
            {
                return (hours / dawnHours) * dawnRealTime;
            }
            else if(hours <= duskHours)
            {
                return dawnRealTime + (hours - dawnHours) / dayLightHours * dayLightRealTime; 
            }
            else
            {
                return duskRealTime + (hours - duskHours) / duskHoursUntilEnd * duskRealTimeUntilEnd;
            }
        }


        float GetHoursFromRealTime(float time)
        {
            if (time <= dawnRealTime)
            {
                return (time / dawnRealTime) * dawnHours;
            }
            else if (time <= duskRealTime)
            {
                return dawnHours + (time - dawnRealTime) / dayLightRealTime * dayLightHours;
            }
            else
            {
                return duskRealTime + (time - duskRealTime) / duskRealTimeUntilEnd * duskHoursUntilEnd;
            }
        }


        void ValidateDayNightData()
        {
                if (dayNightData.DayOfMonth > daysInMonth)
                {
                    dayNightData.DayOfMonth = 1;
                    dayNightData.Month++;
                    if (dayNightData.Month > monthsInYear)
                    {
                        dayNightData.Month = 1;
                        dayNightData.Year++;
                    }
                }
        }

    }
}