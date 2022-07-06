using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : ScriptableObject , IInventoriable
{
    public Sprite sprite;
    public Color ShapeColor;
    public float Weight;

    public Sprite GetSprite()
    {
        return sprite;
    }

    public float GetWeight()
    {
        return Weight;
    }

    public abstract void MakeShape(Vector3 location, Quaternion Orientation);


}
