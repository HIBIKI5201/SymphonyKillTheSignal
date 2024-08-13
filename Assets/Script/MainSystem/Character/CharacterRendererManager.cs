using UnityEngine;

public class CharacterRendererManager : MonoBehaviour
{
    SpriteRenderer[] _spriteRenderers;
    void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void ChangeColor(Color color)
    {
        if (_spriteRenderers != null)
        {
            foreach (var renderer in _spriteRenderers)
            {
                if (renderer != null) renderer.color = color;
            }
        }
    }
}
