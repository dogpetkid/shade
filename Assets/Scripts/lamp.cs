using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class lamp : MonoBehaviour
{

    public float angle_start = 0;
    public float angle_end = 2*Mathf.PI;
    public float angle_rocking_factor = (float)1/30;
    public float angle_rocking_speed = 1;
    public int lamp_rays = 90;

    public float lamp_range = 5;
    public bool lamp_enabled = true;
    /// <summary>
    /// True if the light fades with distance from the player as well
    /// </summary>
    public bool player_fade = false;
    public float player_fade_dist = 13;

    public Color pixel_color = Color.white;
    public Color pixel_fade = new Color((float)0.22, (float)0.22, (float)0.22, 1); // a grey color

    public List<Collider2D> objects = new List<Collider2D>();
    public GameObject pixel_prefab = null;
    private List<GameObject> lamp_pixels;

    private float hshift;
    private float hsize;
    private float vshift;
    private float vsize;

    // Start is called before the first frame update
    void Start()
    {
        // make sure a pixel prefab is present
        if (!pixel_prefab) pixel_prefab = (GameObject) Resources.Load<GameObject>("lamp_pixel");

        // instantiate pixels list
        lamp_pixels = new List<GameObject>();
        for (int i = 0; i < lamp_rays; i++)
        {
            GameObject o = Instantiate(pixel_prefab, new Vector3(), Quaternion.identity);
            o.name = "Pixel" + i;
            lamp_pixels.Add(o);
        }

        noLamp();

        bindPixelsToCamera(GameObject.Find("Main Camera").GetComponent(typeof(Camera)) as Camera);
    }

    /// <summary>
    /// Sets the utility values to move the pixels into the specified cameras view
    /// </summary>
    /// <param name="c">Camera to bind pixels to</param>
    void bindPixelsToCamera(Camera c)
    {
        Vector2 lower_left = c.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 upper_right = c.ViewportToWorldPoint(new Vector2(1, 1));
        hshift = lower_left.x;
        hsize = upper_right.x - lower_left.x;
        vshift = lower_left.y;
        vsize = upper_right.y - lower_left.y;
    }

    void setPixelSortingOrder(int order)
    {
        foreach (GameObject pixel in lamp_pixels)
        {
            (pixel.GetComponent<SpriteRenderer>() as SpriteRenderer).sortingOrder = order;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (
            // if the lamp is disabled...
            !lamp_enabled ||
            // or if the lamp fades with distance to the player and the player is far enough from the lamp for it to not make light
            player_fade && Vector2.Distance(transform.position, GameObject.Find("player").transform.position) > player_fade_dist
            )
        {
            // don't bother running the lamp function
            noLamp();
            return;
        }

        Lamp();
    }

    /// <summary>
    /// noLamp turns of the renderer for all pixels belonging to the lamp
    /// </summary>
    public void noLamp() {
        foreach (GameObject pixel in lamp_pixels)
        {
            (pixel.GetComponent<SpriteRenderer>() as SpriteRenderer).enabled = false;
        }
    }

    public void Lamp()
    {
        Collider2D collision;
        Vector2 point;
        float angle;
        Vector2 direction;
        SpriteRenderer pixel_renderer;
        bool pixel_visible;
        float fading;

        GameObject player = GameObject.Find("player");
        ray_marcher.max_distance = lamp_range;

        // create a swaying affect
        float angle_rocking = Mathf.Sin((float)(angle_rocking_speed*Time.time%6.3))*angle_rocking_factor;
        float angle_step = (angle_end - angle_start) / lamp_rays;

        for (int i = 0; i < lamp_rays; i++)
        {
            angle = angle_step * i + angle_start + angle_rocking;
            direction = new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
            pixel_visible = ray_marcher.cast(new Ray2D(transform.position, direction), ref objects, out collision, out point);
            pixel_renderer = lamp_pixels[i].GetComponent<SpriteRenderer>() as SpriteRenderer;
            pixel_renderer.enabled = pixel_visible;
            if (pixel_visible)
            {
                lamp_pixels[i].transform.position = bindPixel(point);
                fading = Vector2.Distance(transform.position, point)/lamp_range;
                if (player_fade)
                {
                    fading = Mathf.Max(fading, Vector2.Distance(transform.position, player.transform.position)/player_fade_dist);
                }
                pixel_renderer.color = Color.Lerp(pixel_color, pixel_fade, fading);
            }
        }
    }

    /// <summary>
    /// Take a positive Modulus
    /// </summary>
    /// <param name="a">Dividend</param>
    /// <param name="b">Divisor</param>
    /// <returns>positive a%b</returns>
    float positiveMod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

    /// <summary>
    /// Bind a pixel translation to the camera specified prior
    /// </summary>
    /// <param name="position">Unbound pixel translation</param>
    /// <returns>Bounded pixel translation</returns>
    Vector2 bindPixel(Vector2 position)
    {
        return new Vector2(positiveMod(position.x-hshift,hsize)+hshift, positiveMod(position.y-vshift,vsize)+vshift);
    }

}
