using UnityEngine;
using TetraCreations.Attributes;
using EL.Core.Player;

public class PlayerHeart : MonoBehaviour
{   
    [Title("PlayerHeart", TitleColor.Cyan, TitleColor.White, 2, 20)]
    [SerializeField] private float HeartRate = 30f;
    [SerializeField] private float CurrentHeartRate = 0f;

    [Header("Increase During "),Space(20)]
    [SerializeField] private float Walking;
    [SerializeField] private float Running;

    [Header("Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float smoothTime;


    [Header("Line Render")]
    public LineRenderer myLineRenderer;
    public int points;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;
    [Range(0, 2 * Mathf.PI)]
    public float radians;

    //,. Scirpt Variables
    private Player player;

    private float currentVelocity;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Start() 
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }
    void Draw()
    {
        float xStart = xLimits.x;
        float Tau = 2 * Mathf.PI;
        float xFinish = xLimits.y;

        myLineRenderer.positionCount = points;
        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = amplitude * Mathf.Sin((Tau * frequency * x) + (Time.timeSinceLevelLoad * movementSpeed));
            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
        }
    }

    private void Update()
    {
        Draw();
        switch (player.playerState)
        {
            case Player.PlayerState.walking:
                IncreaseHeartRate(Walking);
                break;

            case Player.PlayerState.running:
                IncreaseHeartRate(Running);
                break;

            default:
                break;
        }
    }
    private void IncreaseHeartRate(float Rate)
    {
        CurrentHeartRate = Mathf.SmoothDamp(CurrentHeartRate, Rate, ref currentVelocity, smoothTime, maxSpeed);
    }

}

