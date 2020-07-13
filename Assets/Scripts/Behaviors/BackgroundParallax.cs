using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour {
    public float ParallaxFactor;
    public float Scale;
    public float StarScale;

    private MaterialPropertyBlock materialBlock;
    private SpriteRenderer spriteRenderer;

    void SetMaterial(Vector2 offset) {
        materialBlock.SetVector("_Offset", new Vector4(offset.x, offset.y));
        materialBlock.SetFloat("_OverallScale", Scale);
        materialBlock.SetFloat("_StarsScale", StarScale);
        spriteRenderer.SetPropertyBlock(materialBlock);
    }

    void Awake() {
        materialBlock = new MaterialPropertyBlock();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate() {
        Vector2 newOffset = Camera.main.transform.position * ParallaxFactor;

        SetMaterial(newOffset);
    }
}
