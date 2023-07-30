using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScreenFader : MonoBehaviour 
{
    public static ScreenFader instance;

    [SerializeField] Image img;
    [SerializeField] float _fadeDuration;
    [SerializeField] float _fadeSpeed;
    [SerializeField] Color _fadeFromColor;
    [SerializeField] Color _fadeToColor;
    [SerializeField] bool fadeImage;

    [SerializeField] UnityEvent CompleteEvent;
    private float currentDuration;
    private Color currentColor;
    private void Awake()
    {
        CreateInstance();
        currentDuration = _fadeDuration;
    }

    private void CreateInstance()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }

    public void FadeIn()
    {
        fadeImage = true;
    }
    public void FadeOut()
    {
        fadeImage = false;
    }

    private void Update()
    {
        img.color = fadeImage ? SmoothColorTransition(img.color, _fadeToColor, _fadeSpeed / 2 * Time.deltaTime) :
                                SmoothColorTransition(img.color, _fadeFromColor, _fadeSpeed * Time.deltaTime);

        if (img.color == _fadeToColor)
        {
            CompleteEvent?.Invoke();
        }    
    }

    private Color SmoothColorTransition(Color currentColor, Color targetColor, float transitionSpeed)
    {
        float r = Mathf.MoveTowards(currentColor.r, targetColor.r, transitionSpeed);
        float g = Mathf.MoveTowards(currentColor.g, targetColor.g, transitionSpeed);
        float b = Mathf.MoveTowards(currentColor.b, targetColor.b, transitionSpeed);
        float a = Mathf.MoveTowards(currentColor.a, targetColor.a, transitionSpeed);
        return new Color(r, g, b, a);
    }
    
}