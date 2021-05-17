using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this ray marcher is based on Sebastion Lague's ray marcher: https://github.com/SebLague/Ray-Marching

public class ray_marcher : MonoBehaviour
{

    public const float max_distance = 5f;
    public const float epsilon = 0.001f;

    // find the distance
    public static float signCircleDistance(Vector2 eye, Vector2 center, float radius)
    {
        return Vector2.Distance(eye, center) - radius;
    }

    /// <summary>
    /// Casts a ray and sees what it with from the list of objects
    /// </summary>
    /// <param name="start_ray"> The ray to be cast </param>
    /// <param name="objects"> List of objects to check from which the ray collides with </param>
    /// <param name="collision"> The object that the ray collides with, null if max range hit before collision </param>
    /// <param name="point"> Point the ray collides with, point at max distance if max range hit before collision </param>
    /// <returns> Returns true if the ray collides with an object </returns>
    public static bool cast(Ray2D start_ray, ref List<Collider2D> objects, out Collider2D collision, out Vector2 point)
    {
        // Random.InitState(10); // arbitrary debug seed

        collision = new Collider2D();
        point = start_ray.GetPoint(max_distance);

        Collider2D closest;
        float distance;
        float ray_dist = 0;
        Ray2D current_ray = start_ray;
        while (ray_dist < max_distance)
        {
            // continue to take steps of distance of the closest object until some object is closer than epsilon
            closestFromList(current_ray.origin, ref objects, out closest, out distance);
            // Debug.DrawLine(current_ray.origin, current_ray.GetPoint(distance), Random.ColorHSV(), 0);
            ray_dist += distance;
            current_ray = new Ray2D(current_ray.GetPoint(distance), start_ray.direction);
            if (distance < epsilon)
            {
                collision = closest;
                point = start_ray.GetPoint(ray_dist);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Finds the closest object from a list to the eye.
    /// </summary>
    /// <param name="eye"> Origin that distances are measured from </param>
    /// <param name="objects"> List of objects to check from which is the closes to the eye </param>
    /// <param name="closest"> The object closest to the eye </param>
    /// <param name="dist"> The distance from the eye to the closest object </param>
    /// <returns> Returns true if an object is found that is the closest </returns>
    public static bool closestFromList(Vector2 eye, ref List<Collider2D> objects, out Collider2D closest, out float dist)
    {
        dist = float.MaxValue;
        closest = null;
        float temp_dist = 0;
        for (int i = 0; i < objects.Count; i++)
        {
            // NOTE this seems like a bodged way of achiving the desired functionality, perhaps using something other than colliders is wanted
            temp_dist = Vector2.Distance(eye, objects[i].ClosestPoint(eye));
            if (temp_dist < dist)
            {
                // check if the current object is closer than the already closest object
                closest = objects[i];
                dist = temp_dist;
            }
        }
        if (!closest.Equals(null)) return true;
        return false;
    }

}
