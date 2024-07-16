using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollingBackground : MonoBehaviour
{
    public List<RectTransform> sprites;
    public float scrollSpeed = 50f;
    public Vector2 scaleRange = new Vector2(0.5f, 1.5f);
    public Vector2 rotationSpeedRange = new Vector2(-30f, 30f); // Speed of rotation

    private Vector2 screenBounds;
    private Dictionary<RectTransform, Vector2> directions = new Dictionary<RectTransform, Vector2>();
    private Dictionary<RectTransform, float> rotationSpeeds = new Dictionary<RectTransform, float>();

    void Start()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            screenBounds = new Vector2(Screen.width, Screen.height) / 2f;
        }
        else
        {
            Debug.LogError("ScrollingBackgroundUI script requires the Canvas to be in Screen Space.");
            this.enabled = false;
            return;
        }

        foreach (RectTransform sprite in sprites)
        {
            RandomizeSprite(sprite);
            // Set a random direction towards the middle of the screen
            directions[sprite] = (Vector2.zero - sprite.anchoredPosition).normalized;
            // Set a random rotation speed
            rotationSpeeds[sprite] = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y);
        }
    }

    void Update()
    {
        foreach (RectTransform sprite in sprites)
        {
            sprite.anchoredPosition += directions[sprite] * scrollSpeed * Time.deltaTime;

            sprite.Rotate(new Vector3(0, 0, rotationSpeeds[sprite] * Time.deltaTime));

            if (IsSpriteOutOfBounds(sprite))
            {
                RepositionSprite(sprite);
                RandomizeSprite(sprite);
            }
        }
    }

    bool IsSpriteOutOfBounds(RectTransform sprite)
    {
        Vector2 pos = sprite.anchoredPosition;
        return pos.x < -screenBounds.x || pos.x > screenBounds.x || pos.y < -screenBounds.y || pos.y > screenBounds.y;
    }

    void RepositionSprite(RectTransform sprite)
    {
        float newX = screenBounds.x; // Reset to the right edge
        float newY = Random.Range(-screenBounds.y, screenBounds.y);
        sprite.anchoredPosition = new Vector2(newX, newY);

        // Update the direction towards the middle of the screen
        directions[sprite] = (Vector2.zero - sprite.anchoredPosition).normalized;
    }

    void RandomizeSprite(RectTransform sprite)
    {
        float scale = Random.Range(scaleRange.x, scaleRange.y);
        sprite.localScale = new Vector3(scale, scale, 1);

        // Randomize rotation speed
        rotationSpeeds[sprite] = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y);
    }
}
