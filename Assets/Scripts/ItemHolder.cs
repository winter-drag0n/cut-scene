using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public ScriptableObject Item;

    private void Start()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if ( typeof(Shape).IsAssignableFrom( Item.GetType()))
        {
            Shape shape = (Shape)Item;
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            mpb.SetColor("_BaseMap", shape.ShapeColor);
            Renderer renderer = GetComponent<Renderer>();
            renderer.SetPropertyBlock(mpb);
            if (Item.GetType() == typeof(Ball))
            {
                Ball ball = (Ball)Item;
                transform.localScale = Vector3.one * ball.Size;
                Collider coll = GetComponent<Collider>();
                coll.material.bounciness = ball.Bounciness;
            }
            else if (Item.GetType() == typeof(Cylinder))
            {
                Cylinder ball = (Cylinder)Item;
                transform.localScale = new Vector3(ball.Radius, ball.Height, ball.Radius);
            }
        }
    }

    

}
