﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatRay : MonoBehaviour 
{

    public enum Type
    {
        Single,
        Round
    }
    public Type type;

    private Animator anim;

    public float lifeTime;
    [Space(10)]
    public float moveSpeed;
    public float scaleIncreaseSpeed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            FadeOut();
        }

        if (type == Type.Single)
        {
            transform.Translate(Vector3.up * (Time.deltaTime * moveSpeed));
            transform.localScale += new Vector3(Time.deltaTime * scaleIncreaseSpeed, Time.deltaTime * scaleIncreaseSpeed);
        }
        else if (type == Type.Round)
        {
            transform.localScale += new Vector3(Time.deltaTime * scaleIncreaseSpeed, Time.deltaTime * scaleIncreaseSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle" || collision.tag == "Heart")
        {
            Obstacle obstacle = collision.GetComponent<Obstacle>();

            if (obstacle.detectedParticle != null)
            {
                obstacle.detectedParticle.SetActive(true);
            }

            if (!obstacle.isTriggered)
            {
                GameManager.instance.StartCoroutine(obstacle.TriggerObstacle());

                obstacle.myAnimator.SetTrigger("Highlight");
                obstacle.myAudioSource.Play();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "CaveWall" || collision.transform.tag == "Well")
        {
            AnimationEventDestroy();

            //GetComponent<Rigidbody2D>().isKinematic = true;
            //FadeOut();

            //Vector2 dir = collision.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
            //dir = -dir.normalized;

            //transform.rotation = Quaternion.Euler(dir);
        }
    }

    private void FadeOut()
    {
        anim.SetTrigger("FadeOut");
    }

    private void AnimationEventDestroy()
    {
        Destroy(gameObject);
    }
}
