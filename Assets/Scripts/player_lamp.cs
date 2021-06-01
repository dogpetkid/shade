using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Use lamp.cs instead")]
public class player_lamp : MonoBehaviour
{

    const int lamp_rays = 90;
    const float lamp_step = 2*Mathf.PI / lamp_rays;

    List<Collider2D> objects;
    public GameObject pixel_prefab;
    List<GameObject> lamp_pixels;

    float hshift;
    float hsize;
    float vshift;
    float vsize;

    // Start is called before the first frame update
    void Start()
    {
        // add the tile collider to the list of things the ray collides with
        Collider2D t = GameObject.Find("Tilemap_Base").GetComponent(typeof(Collider2D)) as Collider2D;
        objects = new List<Collider2D>(t.GetComponentsInChildren<Collider2D>());

        Debug.Log("Start");
        Debug.Log(objects);
        foreach (Collider2D c in objects) {Debug.Log(c);}

        lamp_pixels = new List<GameObject>();
        for (int i = 0; i < lamp_rays; i++)
        {
            GameObject o = Instantiate(pixel_prefab, new Vector3(), Quaternion.identity);
            // (o.GetComponent<SpriteRenderer>() as SpriteRenderer).enabled = false; // disable rendering of the pixels until the lamp uses them
            lamp_pixels.Add(o);
        }
        // Debug.Log(lamp_pixels.Count);

        Camera main_camera = GameObject.Find("Main Camera").GetComponent(typeof(Camera)) as Camera;
        Vector2 lower_left = main_camera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 upper_right = main_camera.ViewportToWorldPoint(new Vector2(1, 1));
        hshift = lower_left.x;
        hsize = upper_right.x - lower_left.x;
        vshift = lower_left.y;
        vsize = upper_right.y - lower_left.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Collider2D collision;
        // Vector2 point;

        // Vector2 origin = transform.position;
        // Vector2 direction = new Vector2(1, -1);

        Lamp();

        // Debug.DrawRay(origin, direction, Color.red, 0);
        // if( ray_marcher.cast(new Ray2D(origin, direction), ref objects, out collision, out point) )
        // {
        //     Debug.DrawLine(origin, point, Color.green, 0);
        //     Debug.Log(collision);
        //     Debug.Log(point);
        // }
        // else
        // {
        //     Debug.DrawLine(origin, point, Color.grey, 0);
        // }

    }

    // public float qsin(float f)
    // {
    //     return f - f*f*f/6 + f*f*f*f*f/120 - f*f*f*f*f*f*f/5040 + f*f*f*f*f*f*f*f*f/362880;
    // }

    // public float qcos(float f)
    // {
    //     return 1 - f*f/2 + f*f*f*f/24 - f*f*f*f*f*f/720 + f*f*f*f*f*f*f*f/40320;
    // }

    void Lamp()
    {
        Collider2D collision;
        Vector2 point;
        float angle;
        Vector2 direction;
        SpriteRenderer pixel_renderer;
        bool show;

        // create a swaying affect
        float angle_start = Mathf.Sin((float)(Time.time%6.3))/30;

        for (int i = 0; i < lamp_rays; i++)
        {
            angle = lamp_step * i + angle_start;
            direction = new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
            show = ray_marcher.cast(new Ray2D(transform.position, direction), ref objects, out collision, out point);
            // Debug.DrawLine(transform.position, point, Color.white, 0);
            pixel_renderer = lamp_pixels[i].GetComponent<SpriteRenderer>() as SpriteRenderer;
            pixel_renderer.enabled = show;
            if (show)
            {
                lamp_pixels[i].transform.position = boundPixel(point);
                pixel_renderer.color = Color.Lerp(Color.white, new Color((float)0.22,(float)0.22,(float)0.22,1), Vector2.Distance(transform.position, point)/ray_marcher.max_distance);
                // Debug.Log(pixel_renderer.color);
            }
        }
    }

    float positiveMod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

    Vector2 boundPixel(Vector2 position)
    {
        return new Vector2(positiveMod(position.x-hshift,hsize)+hshift, positiveMod(position.y-vshift,vsize)+vshift);
    }

}
