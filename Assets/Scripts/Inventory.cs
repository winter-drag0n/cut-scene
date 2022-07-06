using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Singleton Group For Saving Objects;
/// </summary>
public class Inventory : MonoBehaviour
{ 
    public static Inventory Instance;
    public List<IInventoriable> Items;
    public float WeightLimit;
    public float Weight { get; private set; }

    private void Awake()
    {
        Weight = 0;
        Items = new List<IInventoriable>();
        DontDestroyOnLoad(gameObject);
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    public bool AddObject( IInventoriable Item)
    {
        if (!Items.Contains(Item))
        {
            if (Weight + Item.GetWeight() >= WeightLimit)
            {
                Debug.Log("Exceeds limit");
                return false;
            }
            Items.Add(Item);
            UpdateWeight();
            return true;
        }
        else
        {
            Debug.Log("Already in Inventory");
            return false;
        }
    }

    public IInventoriable RemoveObject( IInventoriable Item )
    {
        if (Items.Contains(Item))
        {
            IInventoriable a = Item;
            Items.Remove(Item);
            UpdateWeight();
            return Item;
        }
        else return null;
    }

    void UpdateWeight()
    {
        Weight = 0;
        foreach( IInventoriable a in Items )
        {
            Weight += a.GetWeight();
        }
    }


}
