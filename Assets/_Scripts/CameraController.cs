using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform objectTransform;


    private void Update()
    {
        transform.position =
            new Vector3(objectTransform.transform.position.x, objectTransform.position.y, transform.position.z);
    }
}
