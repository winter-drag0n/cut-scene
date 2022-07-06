using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cylinder", menuName = "Cylinder")]
public class Cylinder : Shape
{
    public float Radius;
    public float Height;

    public override void MakeShape(Vector3 location, Quaternion Orientation)
    {
        GameObject Ball = Instantiate(ShapesCollection.Instance.ShapesObjects[(int)ShapesCollection.Shapes.Cyclinder], location, Orientation);
        ItemHolder holder = Ball.GetComponent<ItemHolder>();
        holder.Item = this;
    }

}
