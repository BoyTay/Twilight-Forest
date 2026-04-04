using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = .4f;

    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            if (spriteRenderer) {
                FadeSpriteTo(transparencyAmount);
            } else if (tilemap) {
                FadeTilemapTo(transparencyAmount);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (spriteRenderer) {
                FadeSpriteTo(1f);
            } else if (tilemap) {
                FadeTilemapTo(1f);
            }
        }
    }

    private void FadeSpriteTo(float targetTransparency) {
        if (!gameObject.activeInHierarchy || !enabled) {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetTransparency);
            return;
        }

        StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, targetTransparency));
    }

    private void FadeTilemapTo(float targetTransparency) {
        if (!gameObject.activeInHierarchy || !enabled) {
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, targetTransparency);
            return;
        }

        StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, targetTransparency));
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetTransparency) {
        float elapsedTime = 0;     
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }
    }

    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
    }
}
