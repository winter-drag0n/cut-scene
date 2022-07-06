using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapesCollection : MonoBehaviour
{
    public static ShapesCollection Instance;
    public enum Shapes : byte { Ball, Cyclinder ,Paint}

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    [Tooltip( "Ball , Cyclinder, Paint" )]
    public GameObject[] ShapesObjects;

}
