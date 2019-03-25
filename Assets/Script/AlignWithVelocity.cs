using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithVelocity : MonoBehaviour
{
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Vector2.SignedAngle(Vector2.up, rb2d.velocity);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
