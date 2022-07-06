using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManger : MonoBehaviour
{
    [SerializeField] GameObject SelectMenu;
    [SerializeField] GameObject InventoryWindow;

    private void FixedUpdate()
    {
        if ( Input.GetKeyDown(KeyCode.I) )
        {
            SelectMenu.SetActive(true);
            InventoryWindow.SetActive(true);
        }
    }

    public static void FillWithButtons( int Buttons ,GameObject ButtonPrefab , Transform Window , UnityAction<GameObject,int> ButtonPreparation )
    {
        ClearWindow(Window);
        for (int i = 0; i < Buttons; i++)
        {
            GameObject ButtonInst = Instantiate(ButtonPrefab,Window);
            ButtonPreparation(ButtonInst, i);
        }
    }

    public static void ClearWindow( Transform Window )
    {
        for (int i = Window.childCount - 1; i >= 0; i--)
        {
            Destroy(Window.GetChild(i).gameObject);
        }
    }




}
