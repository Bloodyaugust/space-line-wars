using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetMaterialProperties : MonoBehaviour {
    private MaterialPropertyBlock materialBlock;
    private SpriteRenderer spriteRenderer;

    public void SetMaterial(float flashing, float hue) {
        materialBlock.SetFloat("_Hue", hue);
        materialBlock.SetFloat("_Flashes", flashing);
        spriteRenderer.SetPropertyBlock(materialBlock);
    }

    public void SetMaterial(float flashing, float hue, Texture2D sprite) {
        materialBlock.SetTexture("_MainTex", sprite);
        materialBlock.SetFloat("_Hue", hue);
        materialBlock.SetFloat("_Flashes", flashing);
        spriteRenderer.SetPropertyBlock(materialBlock);
    }

    void Awake() {
        materialBlock = new MaterialPropertyBlock();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
