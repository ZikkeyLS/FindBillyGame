using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float cameraSpeed = 50;
    [SerializeField] private bool follow = true;
    private Camera cam;

    public static Camera Camera;


    private float lastScreenSize = -1;
    private readonly float cameraModificator = 15;

    public float shakeDuration = 0f;
    public float shakeAmount = 100f;
    public float stabilizator = 10f;


    private void Start()
    {
        cam = GetComponent<Camera>();
        Camera = cam;
    }

    private void LateUpdate()
    {
        float screenSize = (float)Screen.height / (float)Screen.width;
        if (screenSize != lastScreenSize)
        {
            cam.orthographicSize = screenSize * cameraModificator;
            lastScreenSize = screenSize;
        }

    }

    private void FixedUpdate()
    {
        if (!follow)
            return;

        transform.position = Vector3.Lerp(new Vector3(player.position.x, player.position.y, transform.position.z), transform.position, Time.fixedDeltaTime * cameraSpeed) + cameraOffset;

        if (shakeDuration > 0)
        {

            transform.position += (Random.insideUnitSphere * shakeAmount) / stabilizator;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    public void SetFollowState(bool state)
    {
        follow = state;
    }
}
