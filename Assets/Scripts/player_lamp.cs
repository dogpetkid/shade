using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_lamp : MonoBehaviour
{
    List<Collider2D> objects;

    // Start is called before the first frame update
    void Start()
    {
        // add the 
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

        // Debug.DrawRay(origin, direction, Color.red, 0);
        if( ray_marcher.cast(new Ray2D(origin, direction), ref objects, out collision, out point) )
        {
            // Debug.DrawLine(origin, point, Color.green, 0);
            // Debug.Log(collision);
            // Debug.Log(point);
        }
        // else
        // {
        //     Debug.DrawLine(origin, point, Color.grey, 0);
        // }

    }

}
