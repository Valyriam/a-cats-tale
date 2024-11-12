using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    public float throwForce = 10f; // Initial force applied to the object
    public float mass = 1f; // Mass of the object
    public float throwAngle = 45f; // Initial angle at which the object is thrown
    public int segments = 50; // Number of segments in the trajectory line
    public LineRenderer lineRenderer; // LineRenderer component to draw the trajectory

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments;
    }

    public void DrawTrajectory()
    {
        Vector3[] points = CalculateTrajectoryPoints();

        for (int i = 0; i < segments; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    Vector3[] CalculateTrajectoryPoints()
    {
        Vector3[] points = new Vector3[segments];

        float timeStep = 0.05f; // Time step between points
        float gravity = Physics2D.gravity.y;

        float initialVelocityMagnitude = throwForce;
        float initialVelocityX = initialVelocityMagnitude * Mathf.Cos(throwAngle * Mathf.Deg2Rad);
        float initialVelocityY = initialVelocityMagnitude * Mathf.Sin(throwAngle * Mathf.Deg2Rad);

        for (int i = 0; i < segments; i++)
        {
            float t = i * timeStep;
            float x = transform.position.x + initialVelocityX * t;
            float y = transform.position.y + initialVelocityY * t + 0.5f * gravity * Mathf.Pow(t, 2);
            float z = 20;

            points[i] = new Vector3(x, y, z);
        }

        return points;
    }
}