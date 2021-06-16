using UnityEngine;

public class ExperienceFollow : MonoBehaviour
{
    [SerializeField] private float speed = 0.03f;
    private GameObject player;

    void Start()
    {
        player = PlayerInformation.Player;
    }

    void Update()
    {
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }


        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 1));
        transform.position = Vector3.Lerp(transform.position, player.transform.position, speed);
        if (Vector3.Distance(transform.position, player.transform.position) < 1)
        {
            Destroy(gameObject);
            PlayerInformation.information.GiveComponents(1);
        }

    }
}
