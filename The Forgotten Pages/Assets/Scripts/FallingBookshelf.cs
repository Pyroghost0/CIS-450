/* Caleb Kahn, Cooper Denault
 * FallingBookshelf
 * Project 5
 * Bookshelf that falls once triggered
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBookshelf : MonoBehaviour
{
    public AudioSource initialSoundEffect;
    public AudioSource finalSoundEffect;
    public Vector3 transformDistance;
    public Vector3 transformRotation;
    public bool gravity = false;
    private bool enetered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ! enetered)
        {
            enetered = true;
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        if (gravity)
        {
            if (initialSoundEffect != null)
            {
                initialSoundEffect.Play();
            }
            float timer = 0f;
            Vector3 initialPos = transform.position;
            Vector3 initialRotation = transform.rotation.eulerAngles;
            while (timer < .5f)
            {
                timer += Time.deltaTime;
                float pos = (timer * timer) * 4f;
                transform.position = initialPos + (transformDistance * pos);
                transform.rotation = Quaternion.Euler(initialRotation + (transformRotation * pos));
                yield return new WaitForFixedUpdate();
            }
            transform.position = initialPos + transformDistance;
            transform.rotation = Quaternion.Euler(initialRotation + transformRotation);
            if (finalSoundEffect != null)
            {
                finalSoundEffect.Play();
            }
        }
        else
        {
            if (initialSoundEffect != null)
            {
                initialSoundEffect.Play();
            }
            float timer = 0f;
            Vector3 initialPos = transform.position;
            Vector3 initialRotation = transform.rotation.eulerAngles;
            while (timer < .5f)
            {
                timer += Time.deltaTime;
                float pos = Mathf.Sin(Mathf.PI * timer);
                transform.position = initialPos + (transformDistance * pos);
                transform.rotation = Quaternion.Euler(initialRotation + (transformRotation * pos));
                yield return new WaitForFixedUpdate();
            }
            transform.position = initialPos + transformDistance;
            transform.rotation = Quaternion.Euler(initialRotation + transformRotation);
            if (finalSoundEffect != null)
            {
                finalSoundEffect.Play();
            }
        }
    }
}
