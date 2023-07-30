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
    [SerializeField] float intensity = 15;
    [SerializeField] float smooth = 15;
    [SerializeField] bool useSway = false;
    [SerializeField] GameObject flashLightObject;
    [SerializeField] Light flashLight => flashLightObject.GetComponent<Light>();
    private Quaternion origin_rotation;

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
        origin_rotation = flashLightObject.transform.localRotation;
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
        UpdateUI();

        if(useSway) { UpdateSway(); }
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
        float t_x_mouse = Input.GetAxis("Mouse X");
        float t_y_mouse = Input.GetAxis("Mouse Y");

        Quaternion t_x_adj = Quaternion.AngleAxis(-intensity * t_x_mouse, Vector3.up);
        Quaternion t_y_adj = Quaternion.AngleAxis(intensity * t_y_mouse, Vector3.right);
        Quaternion target_rotation = origin_rotation * t_x_adj * t_y_adj;

        flashLightObject.transform.localRotation = Quaternion.Lerp(flashLightObject.transform.localRotation, target_rotation, Time.deltaTime * smooth);
    }
    void UpdateUI()
    {
        PlayerUIManager.instance.UI_flashLightChargeSlider.maxValue = maxCharge;
        PlayerUIManager.instance.UI_flashLightChargeSlider.value = currentCharge;
    }
}