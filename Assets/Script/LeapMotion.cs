using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class LeapMotion : MonoBehaviour
{
    public GameObject birdPrefab;
    public float scaleFactor;
    public bool randomLife;
    public float T;
    LeapServiceProvider leap;
    ParticleSystem PS;
    bool pinching = false;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        leap = GetComponentInParent<LeapServiceProvider>();
        PS = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (leap.IsConnected())
        {
            leap.RetransformFrames();
            // leap.RetransformFrames();
            var hands = leap.CurrentFrame.Hands;
            Hand hand = null;
            foreach (Hand _hand in hands)
            {
                hand = _hand;
            }
            

            if (hand != null)
            {
                var handPos = hand.GetPredictedPinchPosition();
                Vector3 relativePos = Vector3.zero;
                relativePos.x = handPos.x * scaleFactor;
                relativePos.y = handPos.z * scaleFactor;
                transform.localPosition = relativePos;

                if (hand.IsPinching() && pinching == false)
                {
                    pinching = true;
                    timer = Time.time;
                    PS.Play();
                }

                if (!hand.IsPinching() && pinching == true)
                {
                    pinching = false;
                    var bird = GameObject.Instantiate(birdPrefab, transform.position, Quaternion.identity).GetComponent<Bird>();
                    var time = Time.time - timer;
                    if (randomLife)
                    {
                        bird.life = Random.Range(0F, 1F);
                    }
                    else
                    {
                        bird.life = 1 - Mathf.Exp(-time / T);
                    }
                    PS.Stop();
                }
            }
        }

    }
}
