using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSettings : MonoBehaviour
{
    private BoxCollider carCollider;
    private Vector3 carCenter;
    private Vector3 carSize;
    private float deltaCenterY;
    private GameManager gm;
    private void Awake()
    {
        carCollider = GetComponent<BoxCollider>();
        carCenter = carCollider.center;
        carSize = carCollider.size;
        deltaCenterY = 0.5f*(carSize.y - (carSize.y * 1.15f));

        carCollider.size = new Vector3(carSize.x, carSize.y *1.15f, carSize.z);
        carCollider.center = new Vector3(carCenter.x, carCenter.y + deltaCenterY, carCenter.z);
    }
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }


    void Update()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x,(carCollider.size.y * 190),pos.z);

        if (transform.position.y <= -100)
        {
            gm.carList.Remove(gameObject);
            Destroy(gameObject);
        }

    }
}
