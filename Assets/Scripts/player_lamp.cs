using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_lamp : MonoBehaviour
{

    const int lamp_rays = 90;
    const float lamp_step = 2*Mathf.PI / lamp_rays;

    List<Collider2D> objects;

    // Start is called before the first frame update
    void Start()
    {
        // add the tile collider to the list of things the ray collides with
        Collider2D t = GameObject.Find("Tilemap_Base").GetComponent(typeof(Collider2D)) as Collider2D;
        objects = new List<Collider2D>(t.GetComponentsInChildren<Collider2D>());

        Debug.Log("Start");
        Debug.Log(objects);
        foreach (Collider2D c in objects) {Debug.Log(c);}
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D collision;
        Vector2 point;

        Vector2 origin = transform.position;
        Vector2 direction = new Vector2(1, -1);

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

    void Lamp()
    {
        Collider2D collision;
        Vector2 point;
        float angle;
        Vector2 direction;

        for (int i = 0; i < lamp_rays; i++)
        {
            angle = lamp_step * i;
            direction = new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
            ray_marcher.cast(new Ray2D(transform.position, direction), ref objects, out collision, out point);
            Debug.DrawLine(transform.position, point, Color.white, 0);
        }
    }

}
