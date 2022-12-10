using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GhostTrail : MonoBehaviour
{
    private Player player;
    public Color trailColor;
    public Color fadeColor;
    public float ghostInterval;
    public float fadeTime;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void ShowGhost()
    {
        Sequence secuence = DOTween.Sequence();

        transform.GetChildTransforms().ForEach(item =>
        {
            SpriteRenderer renderer = item.GetComponent<SpriteRenderer>();

            secuence.AppendCallback(() => item.position = player.transform.position)
            .AppendCallback(() => renderer.enabled = true)
            .AppendCallback(() => renderer.flipX = player.Sprite.flipX)
            .AppendCallback(() => renderer.sprite = player.Sprite.sprite)
            .Append(renderer.material.DOColor(trailColor, 0))
            .AppendCallback(() => FadeSprite(renderer))
            .AppendInterval(ghostInterval);
        });
    }

    private void FadeSprite(SpriteRenderer currentRenderer)
    {
        currentRenderer.material.DOKill();
        currentRenderer.material.DOColor(fadeColor, fadeTime);
    }
}
