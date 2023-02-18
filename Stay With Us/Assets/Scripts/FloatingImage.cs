/* Caleb Kahn
 * FloatingImage
 * Project 1
 * Image that floats in a certain direction while fading
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingImage : MonoBehaviour
{
    //public TMPro.TextMeshProUGUI text;
    public SpriteRenderer sprite;
    //public Sprite[] currencyTypeImage;
    public Vector3 distence = new Vector3(2f, -2f, 0f);
    public float noticeTime = 2f;
    private float timer = 0f;
    //private Color textColor;

    /*void Start()
    {
        textColor = text.color;
    }*/

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > noticeTime)
        {
            Destroy(gameObject);
        }
        transform.position += distence * Time.deltaTime / noticeTime;
        sprite.color = new Color(1f, 1f, 1f, 1f - (timer / noticeTime));
        //textColor.a -= Time.deltaTime / noticeTime;
        //text.color = textColor;
    }
}
