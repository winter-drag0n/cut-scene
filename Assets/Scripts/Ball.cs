using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Ball" , menuName = "Ball" )]
public class Ball : Shape
{
    public float Size;
    public float Bounciness;



    public override void MakeShape(Vector3 location, Quaternion Orientation)
    {
        GameObject Ball = Instantiate(ShapesCollection.Instance.ShapesObjects[(int)ShapesCollection.Shapes.Ball], location, Orientation);
        ItemHolder holder = Ball.GetComponent<ItemHolder>();
        holder.Item = this;
    }

}
