using System.Collections;
using UnityEngine;

public class FlashLight : MonoBehaviour
{

    [Space] [Header("CHARGE SETTING")]
    [SerializeField] float maxCharge = 50f;
    [SerializeField] float minimumCharge = 20f;
    [SerializeField] float chargeReductionRate = 5f;
    [SerializeField] float chargeRechargeRate = 5f;

    [Space] [Header("LIGHT SETTING")]
    [SerializeField] float minimumeIntensity = 15f;

    [Space] [Header("FLICKER")]
    [SerializeField] float minTime;
    [SerializeField] float maxTime;

    [Space] [Header("INPUT")]
    [SerializeField] KeyCode ToggleKey = KeyCode.F;
    [SerializeField] KeyCode RechargeKey = KeyCode.R;


    [SerializeField] private float currentCharge;

    [Space] [Header("SWAY SETTING")]

    [SerializeField] GameObject flashLightObject;
    [SerializeField] Light flashLight => flashLightObject.GetComponent<Light>();

    // Script Variables
    private bool canUseFlashlight;
    private bool isToggleOn;
    private bool isFlashlightOn;
    private float originalIntensity;
    private float flashlightIntensity;
    private float Timer;
    private float flickerTime => flashLightObject.GetComponent<Light>().intensity;

    void Start()
    {
        currentCharge = maxCharge;
        canUseFlashlight = true;
        originalIntensity = flashLight.intensity;
        Timer = Random.Range(minTime, maxTime);
    }

    void Update()
    {   
        if(Input.GetKeyDown(ToggleKey)) { isToggleOn =! isToggleOn; }

        if (Input.GetKeyDown(RechargeKey))
        {
            StartCoroutine(Recharge());
        }

        flashLight.enabled = isToggleOn;

        if(flashLight.enabled)
        {
            currentCharge -= chargeReductionRate * Time.deltaTime;
            currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
            
            if(currentCharge < minimumCharge) 
            {
                UpdateFlashlightIntensity(); 
            }
        }

    }

    private bool HasCharge()
    {
        if(currentCharge < minimumCharge) 
        {
            return false;
        }
        return true;
    }
    private void UpdateFlashlightIntensity()
    {
        flashlightIntensity = flashLight.intensity;

        // Check if currentCharge is lower or equal to flashlightIntensity
        if(Mathf.RoundToInt(currentCharge) <= Mathf.RoundToInt(flashlightIntensity))
        {   
            // Check if flashlightIntensity is above the minimum intensity threshold
            if(flashlightIntensity > minimumeIntensity) 
            {
                flashLight.intensity = currentCharge; // Set flashlight intensity to currentCharge
            }
            // Check if flashlightIntensity is below or equal to the minimum intensity threshold
            if(flashlightIntensity <= minimumeIntensity)
            {
                while(flickerTime > 0)
                {
                    Flicker(); // Perform flickering effect

                    flashLight.intensity -= chargeReductionRate / 2 * Time.deltaTime;

                    return;
                }
                return;
            }
        }
        void Flicker()
        {
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
            }
            if (Timer <= 0)
            {
                flashLight.enabled = !flashLight.enabled;
                Timer = Random.Range(minTime, maxTime);
            }
        }
    }
   

    public IEnumerator Recharge()
    {
        currentCharge = maxCharge;
        //isToggleOn = false;

        while (flashLight.intensity < originalIntensity)
        {
            Debug.Log("Increasing");

            // Increase flashlight intensity gradually
            flashLight.intensity += chargeRechargeRate * Time.deltaTime;
            flashLight.intensity = Mathf.Clamp(flashLight.intensity, 0, originalIntensity);

            // Check if flashlight intensity has reached the original intensity
            if(flashLight.intensity >= originalIntensity) 
            {
                // Set flashlight intensity to the original intensity
                flashLight.intensity = originalIntensity;
                flashlightIntensity = flashLight.intensity;
                // Exit the loop
                break;
            }
            yield return null;
        }
    }
    void UpdateSway()
	{
      
    }
}