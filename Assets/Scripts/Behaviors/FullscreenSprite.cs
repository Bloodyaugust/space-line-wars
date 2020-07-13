using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenSprite : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Vector2 spriteSize;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteSize = spriteRenderer.sprite.bounds.size;
    }
    
    void LateUpdate() {
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        
        Vector2 scale = new Vector2(1, 1);
        if (cameraSize.x >= cameraSize.y) { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        } else { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }

        transform.localScale = scale;
    }
}
