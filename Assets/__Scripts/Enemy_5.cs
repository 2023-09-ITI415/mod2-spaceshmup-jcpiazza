using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_5 : Enemy
{ // Enemy_3 extends Enemy
    // Enemy_3 will move following a Bezier curve, which is a linear
    // interpolation between more than two points.
    [Header("Set in Inspector: Enemy_5")]
    public float lifeTime = 5;

    [Header("Set Dynamically: Enemy_5")]
    public Vector3[] points;
    public float birthTime;
    private Vector3 p0, p1; // The two points to interpolate
    private float timeStart; // Birth time for this Enemy_4
    private float duration = 4; // Duration of movement

    void InitMovement()
    {
        p0 = p1; // Set p0 to the old p1
        // Assign a new on-screen location to p1
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        // Reset the time
        timeStart = Time.time;
    }

    private void Start()
    {
        InitMovement();

        points = new Vector3[3]; // Initialize points

        // The start position has already been set by Main.SpawnEnemy()
        points[0] = pos;

        // Set xMin and xMax the same way that Main.SpawnEnemy() does
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        Vector3 v;
        // Pick a random middle position in the bottom half of the screen
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;

        // Pick a random final position above the top of the screen
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        // Set the birthTime to the current time
        birthTime = Time.time;
    }

    public override void Move()
    {
        // This completely overrides Enemy.Move() with a linear interpolation
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2); // Apply Ease Out easing to u
        pos = ((1 - u) * p0) + (u * p1);// Simple linear interpolation
    }
}
