using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace DayNight
{

    public class DayNightController : MonoBehaviour
    {
        [SerializeField] private DayNightData dayNightData;
        [SerializeField] private Light sunLight;
        [SerializeField] private Light moonLight;
        [SerializeField] private Texture2D[] moonTextures = new Texture2D[30];

        [SerializeField] [Tooltip ("Example: 2,2,3,3,4,4,5,5,6,6,7,7,8,9,8,7,7,6,6,5,5,4,4,3,2,2,1,1,1,2")]
        private int[] moonLux = new int[30];

        private HDAdditionalLightData moonLightData;
        private bool moonUpdated = true;
        private const float tiltForShadowStability = 20;

        void Start()
        {
            moonLightData = moonLight.GetComponent<HDAdditionalLightData>();
            moonLightData.surfaceTexture = moonTextures[dayNightData.DayOfMonth - 1];
            moonLightData.intensity = moonLux[dayNightData.DayOfMonth - 1];
        }


        void Update()
        {
            UpdatePositions();
        }


        void UpdatePositions()
        {
            float sunRotation = Mathf.Lerp(-90, 270, dayNightData.FractionOfDayForSunMoon);
            float moonRotation = sunRotation - 180;
            sunLight.transform.rotation = Quaternion.Euler(sunRotation, 0, tiltForShadowStability);
            moonLight.transform.rotation = Quaternion.Euler(moonRotation, 0, tiltForShadowStability);
            if (sunRotation > -5 && sunRotation < 185) 
            {
                ShadowCaster(true); 
            }
            else
            {
                ShadowCaster(false); 
            }
            UpdateMoonPhase();
        }


        void ShadowCaster(bool useSun)
        {
            if (useSun)
            {
                moonLight.shadows = LightShadows.None;
                sunLight.shadows = LightShadows.Soft;
            }
            else
            {
                sunLight.shadows = LightShadows.None;
                moonLight.shadows = LightShadows.Soft;
            }
        }

        void UpdateMoonPhase()
        {
            if (dayNightData.FractionOfDayForSunMoon < 0.5f) moonUpdated = false;

            else if (dayNightData.FractionOfDayForSunMoon > 0.5f && !moonUpdated)
            {
                moonLightData.surfaceTexture = moonTextures[dayNightData.DayOfMonth - 1];
                moonLightData.intensity = moonLux[dayNightData.DayOfMonth - 1];
                moonUpdated = true;
            }
        }
    }
}