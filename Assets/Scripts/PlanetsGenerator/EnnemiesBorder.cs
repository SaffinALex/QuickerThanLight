﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class EnnemiesBorder : MonoBehaviour
{
    EdgeCollider2D limits;

    public Vector2 marginScale = new Vector2(1.2f,1.2f);

    public static Vector2 size;
    // Start is called before the first frame update
    void Awake()
    {
        limits = GetComponent<EdgeCollider2D>();

        float halfFieldOfView = Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad;
        float height = (Mathf.Abs(Camera.main.transform.position.z)) * Mathf.Tan(halfFieldOfView) * 2.0f;
        float width = Camera.main.aspect * height;

        height *= marginScale.y;
        width *= marginScale.x;
        EnnemiesBorder.size.x = width;
        EnnemiesBorder.size.y = height;

        Vector2[] allPoints = new Vector2[5];
        allPoints[0] = (new Vector2(-width/2, height/2));
        allPoints[1] = (new Vector2(width/2, height/2));
        allPoints[2] = (new Vector2(width/2, -height/2));
        allPoints[3] = (new Vector2(-width/2, -height/2));
        allPoints[4] = (new Vector2(-width/2, height/2));

        limits.points = allPoints;
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player")
            other.gameObject.transform.GetComponent<Rigidbody>().velocity = new Vector2(0,0);
    }
}
