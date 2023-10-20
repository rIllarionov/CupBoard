using UnityEngine;

public class Chip : MonoBehaviour, ILightable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _prefab;

    private GameObject _sprite;
    public int CurrentPosition { get; set; }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
        _sprite = Instantiate(_prefab, transform, false);
    }

    public void SwitchLight(bool state)
    {
        switch (state)
        {
            case true:
                _sprite.gameObject.SetActive(true);
                break;
            case false:
                _sprite.gameObject.SetActive(false);
                break;
        }
    }
}