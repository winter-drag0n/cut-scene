using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickHand : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickable") )
        {
            if (other.tag == "Shape")
            {
                ItemHolder itemHolder = other.GetComponent<ItemHolder>();
                Shape shape = (Shape)itemHolder.Item;
                if (shape.Weight + Inventory.Instance.Weight > Inventory.Instance.WeightLimit) return;
                Inventory.Instance.AddObject((IInventoriable)itemHolder.Item);
                Destroy(other.gameObject);
            }
        }
    }
}
