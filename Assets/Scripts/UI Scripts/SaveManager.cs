using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] Vector3 centre;
    [SerializeField] Vector3 size;
    [SerializeField] InputField SaveName;
    [SerializeField] Dropdown dropdown;
    List<string> SavedFiles;

    private void Awake()
    {
        SavedFiles = new List<string>();
        if (!Directory.Exists(Application.persistentDataPath + SaveList.SavePath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + SaveList.SavePath);
        }
    }

    private void OnEnable()
    {
        SaveName.text = null;
        UpdateSavedFiles();
    }

    /// <summary>
    /// Saves Objects on the Screen in the file
    /// </summary>
    public void SaveObjects()
    {
        if (string.IsNullOrWhiteSpace(SaveName.text)) return;
        Collider[] Shapes = Physics.OverlapBox(centre, size / 2,Quaternion.identity,LayerMask.GetMask("Pickable"));
        List<SaveObject> Slist = new List<SaveObject>();

        foreach ( Collider collider in Shapes )
        {
            ItemHolder shape = collider.GetComponent<ItemHolder>();
            Slist.Add(new SaveObject(shape));
        }

        SaveList saveList = new SaveList(Slist);
        saveList.Save(SaveName.text);
        UpdateSavedFiles();
    }

    public void LoadObjects()
    {
        SaveList savedData = SaveList.Load(Application.persistentDataPath + SaveList.SavePath + "/" + SavedFiles[dropdown.value]);
        foreach ( SaveObject so in savedData.saveObjects )
        {
            MakeObject(so);
        }
    }

    public void DeleteFile()
    {
        File.Delete(Application.persistentDataPath + SaveList.SavePath + "/" + SavedFiles[dropdown.value]);
        UpdateSavedFiles();
    }

    void UpdateSavedFiles()
    {

        SavedFiles.Clear();
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + SaveList.SavePath);
        FileInfo[] fi = di.GetFiles("*.svl");
        foreach (var f in fi)
        {
            SavedFiles.Add(f.Name);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(SavedFiles);
    }

    void MakeObject( SaveObject so)
    {
        Vector3 loc = new Vector3(so.position[0], so.position[1], so.position[2]);
        Quaternion rot = new Quaternion(so.oreintation[0], so.oreintation[1], so.oreintation[2], so.oreintation[3]);
        GameObject go = Instantiate(ShapesCollection.Instance.ShapesObjects[so.PrefabNo], loc, rot);
        ItemHolder ih = go.GetComponent<ItemHolder>();

        switch (so.PrefabNo)
        {
            case 0:
                Ball ball = ScriptableObject.CreateInstance<Ball>();
                ball.Bounciness = so.size[1];
                ball.Size = so.size[0];
                ball.ShapeColor = new Color(so.color[0], so.color[1], so.color[2], so.color[3]);
                ball.Weight = so.weight;
                ball.sprite = ((Ball)ih.Item).sprite;
                ih.Item = ball;
                break;
            case 1:
                Cylinder cyl = ScriptableObject.CreateInstance<Cylinder>();
                cyl.Height = so.size[1];
                cyl.Radius = so.size[0];
                cyl.ShapeColor = new Color(so.color[0], so.color[1], so.color[2], so.color[3]);
                cyl.Weight = so.weight;
                cyl.sprite = ((Cylinder)ih.Item).sprite;
                ih.Item = cyl;
                break;
            case 2:
                Paint pai = ScriptableObject.CreateInstance<Paint>();
                pai.ShapeColor = new Color(so.color[0], so.color[1], so.color[2], so.color[3]);
                pai.Weight = so.weight;
                pai.sprite = ((Paint)ih.Item).sprite;
                ih.Item = pai;
                break;
        }

    }


    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void SpawnRandomObject()
    {
        int rnno = Random.Range(0, 3);
        Vector3 position = new Vector3(Random.Range(-14f, 14f), Random.Range(1f, 7f), Random.Range(-14f, 14f));
        Quaternion quaternion = Random.rotation;
        GameObject CreatedShape;
        CreatedShape = Instantiate(ShapesCollection.Instance.ShapesObjects[rnno],position,quaternion);
        ItemHolder ih = CreatedShape.GetComponent<ItemHolder>();
        if ( ih.Item.GetType() == typeof(Ball) )
        {
            Ball ball = ScriptableObject.CreateInstance<Ball>();
            ball.sprite = ((Ball)ih.Item).sprite;
            ball.Bounciness = Random.Range(0.1f, 1.0f);
            ball.Size = Random.Range(1f, 4f);
            ball.Weight = Random.Range(0.1f, 2f);
            ball.ShapeColor = new Color(Random.value, Random.value, Random.value, 1);
            ih.Item = ball;
        }
        else if (ih.Item.GetType() == typeof(Cylinder))
        {
            Cylinder cyl = ScriptableObject.CreateInstance<Cylinder>();
            cyl.sprite = ((Cylinder)ih.Item).sprite;
            cyl.Radius = Random.Range(0.5f, 3.0f);
            cyl.Height = Random.Range(1f, 5f);
            cyl.Weight = Random.Range(0.1f, 2f);
            cyl.ShapeColor = new Color(Random.value, Random.value, Random.value, 1);
            ih.Item = cyl;
        }
        else if (ih.Item.GetType() == typeof(Paint))
        {
            Paint pai = ScriptableObject.CreateInstance<Paint>();
            pai.sprite = ((Paint)ih.Item).sprite;
            pai.ShapeColor = new Color(Random.value, Random.value, Random.value, 1);
            pai.Weight = Random.Range(0.1f, 2f);
            ih.Item = pai;
        }

    }

    
}


[System.Serializable]
public class SaveList
{
    public const string SavePath = "/Saves";
    public SaveObject[] saveObjects;

    public SaveList( List<SaveObject> saveObjectsList )
    {
        saveObjects = saveObjectsList.ToArray();
    }

    public void Save(string SaveName)
    {
        FileStream fs = File.Open(Application.persistentDataPath + SavePath + "/" + SaveName + ".svl", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, this);
    }

    public static SaveList Load( string LoadName )
    {
        Debug.Log("Loaded : " + LoadName);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.OpenRead(LoadName);
        return (SaveList)bf.Deserialize(fs);
    }


}

[System.Serializable]
public class SaveObject
{
    public byte PrefabNo;
    public float[] position;
    public float[] oreintation;
    public float[] size;
    public float[] color;
    public float weight;

    public SaveObject(ItemHolder shape)
    {
        if ( shape.Item.GetType() == typeof(Ball) )
        {
            PrefabNo = 0;
            Ball ball = (Ball)shape.Item;
            Rigidbody rb = shape.GetComponent<Rigidbody>();
            position = new float[] { rb.position.x, rb.position.y, rb.position.z };
            oreintation = new float[] { rb.rotation.w, rb.rotation.x, rb.rotation.y, rb.rotation.z };
            size = new float[] { ball.Size, ball.Bounciness };
            color = new float[] { ball.ShapeColor.r, ball.ShapeColor.g, ball.ShapeColor.b, ball.ShapeColor.a };
            weight = ball.Weight;
        }
        else if (shape.Item.GetType() == typeof(Cylinder) )
        {
            PrefabNo = 1;
            Cylinder ball = (Cylinder)shape.Item;
            Rigidbody rb = shape.GetComponent<Rigidbody>();
            position = new float[] { rb.position.x, rb.position.y, rb.position.z };
            oreintation = new float[] { rb.rotation.w, rb.rotation.x, rb.rotation.y, rb.rotation.z };
            size = new float[] { ball.Radius, ball.Height };
            color = new float[] { ball.ShapeColor.r, ball.ShapeColor.g, ball.ShapeColor.b, ball.ShapeColor.a };
            weight = ball.Weight;
        }
        else if (shape.Item.GetType() == typeof(Paint))
        {
            PrefabNo = 2;
            Paint ball = (Paint)shape.Item;
            Rigidbody rb = shape.GetComponent<Rigidbody>();
            position = new float[] { rb.position.x, rb.position.y, rb.position.z };
            oreintation = new float[] { rb.rotation.w, rb.rotation.x, rb.rotation.y, rb.rotation.z };
            color = new float[] { ball.ShapeColor.r, ball.ShapeColor.g, ball.ShapeColor.b, ball.ShapeColor.a };
            weight = ball.Weight;
        }

    }


}
