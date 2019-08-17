using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb2d = collision.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            Player player = collision.GetComponent<Player>();
            if (player)
            {
                transform.parent.GetComponent<Water>().Splash(
                    transform.position.x, player.transform.position.y, player.controller.velocity.y / 120);

            }
            else
            {
                transform.parent.GetComponent<Water>().Splash(
                    transform.position.x, collision.transform.position.y, rb2d.velocity.y * rb2d.mass / 40f);
            }
        }
    }
}
