using UnityEngine;

[HideInInspector]
public class Cell
{
    private SpriteRenderer sr;

    private int cellValue = 0;

    public void SetSpriteRenderer(SpriteRenderer sr) => this.sr = sr;

    public void UpdateSprite(Sprite s) => sr.sprite = s;

    public void SetValue(int value) => cellValue = value;

    public int GetValue() => cellValue;

    public override string ToString()
    {
        return GetValue().ToString();
    }
}