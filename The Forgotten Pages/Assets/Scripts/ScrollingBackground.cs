using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/*
 * Anna Breuker
 * ScrollingBackground.cs
 * Project 7&8 - Final Game
 * Code for scrolling backgrounds.
 */
public class ScrollingBackground : MonoBehaviour
{
    public float speed;
    private Vector2 startPos;
    public float endPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < endPos)
        {
            transform.position = startPos;
        }
    }
}