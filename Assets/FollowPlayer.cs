using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float cameraSpeed = 50;
    [SerializeField] private bool follow = true;

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
