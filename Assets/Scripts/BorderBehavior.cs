using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderBehavior : MonoBehaviour
{
    private GameManager gm;
    void Start()
    {
        gm = FindObjectOfType<GameManager>();

    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Car"))
        {
            gm.carList.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
        else if (other.transform.CompareTag("Food"))
        {
            gm.foodList.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
        else if (other.transform.CompareTag("Tree"))
        {
            gm.treeList.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
        else if (!other.transform.CompareTag("Ground") && !other.transform.CompareTag("Sky"))
        {
            Destroy(other.gameObject);
        }
    }
}
