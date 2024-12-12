using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBullet : MonoBehaviour
{
    public InputAction gunTrigger;

    public GameObject bulletPrefab;

    public Transform spawnPoint;

    public float bulletSpeed = 20;

    private void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(ShootGun);
    }

    public void ShootGun(ActivateEventArgs arg)
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab);
        spawnedBullet.transform.position = spawnPoint.position;
        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;
        Destroy(spawnedBullet, 5);
    }
}
