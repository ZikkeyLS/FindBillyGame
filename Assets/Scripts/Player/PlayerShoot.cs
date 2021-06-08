using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private GameObject bullet;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Shoot()
    {
        CameraController.Camera.GetComponent<CameraController>().OnAttack();
        animator.SetTrigger("shoot");

        GameObject currentBullet = Instantiate(bullet, transform.position + new Vector3(-transform.localScale.x * 1.25f, -0.6f), transform.rotation);
        currentBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-transform.localScale.x * 5, 0), ForceMode2D.Impulse);
    }

    public void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0) && canShoot)
        {
            canShoot = false;
            StartCoroutine(sprayTime());
            Shoot();
        }
        Debug.DrawLine(transform.position, transform.position + new Vector3(10 * -transform.localScale.x, 0, 0), Color.yellow);
    }

    IEnumerator sprayTime()
    {
        yield return new WaitForSeconds(0.35f);
        canShoot = true;
    }
}

