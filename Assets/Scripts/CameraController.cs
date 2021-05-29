using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float cameraSpeed = 50;
    [SerializeField] private bool follow = true;
    private Camera cam;
    private float lastScreenSize = -1;
    private readonly float cameraModificator = 15;

    private void Start()
    {
        cam = GetComponent<Camera>();
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
    }

    public void SetFollowState(bool state)
    {
        follow = state;
    }
}
