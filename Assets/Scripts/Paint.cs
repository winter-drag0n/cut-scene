using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Paint",menuName = "Paint")]
public class Paint : Shape
{ 
    public override void MakeShape(Vector3 location, Quaternion Orientation)
    {
        Instantiate(ShapesCollection.Instance.ShapesObjects[(int)ShapesCollection.Shapes.Paint], location, Orientation);
    }
}
