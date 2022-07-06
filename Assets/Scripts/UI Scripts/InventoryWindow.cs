using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField] GameObject SimpleImageButton;
    [SerializeField] Transform InventoryArea;
    [SerializeField] Rigidbody Player;

    void OnEnable()
    {
        if (Inventory.Instance.Items != null)
            DrawUI();
    }

    private void OnDisable()
    {
        UIManger.ClearWindow(InventoryArea);
    }

    void ButtonPrepare(GameObject go , int i)
    {
        go.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.Instance.Items[i].GetSprite();
        Button button = go.GetComponent<Button>();
        if (typeof(Shape).IsAssignableFrom(Inventory.Instance.Items[i].GetType()))
        {
            go.transform.GetChild(0).GetComponent<Image>().color = ((Shape)Inventory.Instance.Items[i]).ShapeColor;
        }
        button.onClick.AddListener(delegate {
            if ( typeof(Shape).IsAssignableFrom( Inventory.Instance.Items[i].GetType() ) )
            {
                Destroy(go);
                Shape shape = (Shape)Inventory.Instance.Items[i];
                shape.MakeShape(Player.position + Player.transform.forward * 2 + new Vector3(0, 1, 0), Quaternion.identity);
                Inventory.Instance.RemoveObject(shape);
                DrawUI();
            }
        });
    }

    void DrawUI() => UIManger.FillWithButtons(Inventory.Instance.Items.Count, SimpleImageButton, InventoryArea, ButtonPrepare);


}
