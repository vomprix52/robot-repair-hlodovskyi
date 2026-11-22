using System.Collections;
using UnityEngine;

public class TimedSwitch : MonoBehaviour
{
    public GameObject door;
    public float openTime = 4.0f;
    public Color openColor = new Color(0.5f, 1f, 0.5f);

    SpriteRenderer spriteRenderer;
    Color baseColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() == null)
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(OpenDoorFor(openTime));
    }

    IEnumerator OpenDoorFor(float seconds)
    {
        door.SetActive(false);
        spriteRenderer.color = openColor;
        yield return new WaitForSeconds(seconds);
        door.SetActive(true);
        spriteRenderer.color = baseColor;
    }
}
