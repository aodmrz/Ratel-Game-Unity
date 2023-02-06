using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehavior : MonoBehaviour
{
    private GameManager gm;
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (transform.position.z <= 0)
        {
            gm.foodList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
