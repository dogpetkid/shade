using UnityEngine;
using UnityEngine.SceneManagement;

public class player_movement : MonoBehaviour
{
    public float player_move_hspeed = 3f;
    public float player_move_jspeed = 2f;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(player_move_hspeed);
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(horizontal, 0, 0) * Time.deltaTime * player_move_hspeed;
        // TODO stop the player from sliding down slopes (or make it a mechanic maybe)
        // Debug.Log(horizontal);

        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            // Debug.Log("Jump");
            _rigidbody.AddForce(new Vector2(0, player_move_jspeed), ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Cancel")) SceneManager.LoadScene("SampleScene"); // temporary reset
    }
}
