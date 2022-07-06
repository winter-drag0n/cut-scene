using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.Serialization.Json;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] InputField PlayerName;
    [SerializeField] InputField TeamName;
    [SerializeField] InputField Age;

    private void OnEnable()
    {
        PlayerName.text = PlayerPrefs.GetString("PlayerName", "No Name");
        TeamName.text = PlayerPrefs.GetString("TeamName", "No Team");
        Age.text = PlayerPrefs.GetInt("Age").ToString();
    }

    public void SaveProfile()
    {
        int i = -1;
        PlayerPrefs.SetString("PlayerName", PlayerName.text);
        PlayerPrefs.SetString("TeamName", TeamName.text);
        if (int.TryParse(Age.text, out i))
            PlayerPrefs.SetInt("Age", i);
        else
            Age.text = "#Error : Not Integer";
    }


    public void SendMessage ()
    {
        StartCoroutine(Request());
    }

    IEnumerator Request()
    {
        string a = " {\n \"requestData\": null,\n \"partnerName\":\"moonglabs\",\n \"versionNumber\":\"30011\",\n \"versionName\": \"3.11\",\n \"packageName\": \"com.moonglabss.epiccricket\",\n \"userId\": \"fyNIYUHBRBqE5Uo\"\n} { \"payload\": {\"status\": {\"message\": \"levels data has been successfully fetched.\",\"code\": 400,\"value\": true, \"heading\": \"success\"},\"data\":{\"maxReachedLevel\": 10,\"trophies\": [1,1,2,3,2,2,1,1,3,1] }}}";
        string b = "{ \"payload\": {\"status\": {\"message\": \"levels data has been successfully fetched.\",\"code\": 400,\"value\": true, \"heading\": \"success\"},\"data\":{\"maxReachedLevel\": 10,\"trophies\": [1,1,2,3,2,2,1,1,3,1] }}}";
        UnityWebRequest request = new UnityWebRequest("https://stglbq.moonglabs.com/v2/getLevelProgress", "POST" );
        byte[] bb = new byte[1024];
        byte[] cc = new byte[1024];
        bb = Encoding.ASCII.GetBytes(b);
        UploadHandler uh = new UploadHandlerRaw(bb);
        DownloadHandler dh = new DownloadHandlerBuffer();
        request.uploadHandler = uh;
        request.downloadHandler = dh;
        request.SendWebRequest();

        while (!request.isDone)
        {
            Debug.Log(JsonUtility.ToJson(new RequestObj(),true));
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log(request.result);
        Debug.Log(request.downloadHandler.text);

        DeserializedResult result = JsonUtility.FromJson<DeserializedResult>(b);

        Debug.Log(result.ToString());


        yield return null;
    }


}

[System.Serializable]
public class RequestObj
{
    public object requestData = null;
    public string partnerName = "moonglabs";
    public string versionNumber = "30011";
    public string versionName = "3.11";
    public string packageName = "com.moonglabss.epiccricket";
    public string userId = "fyNIYUHBRBqE5Uo";
}

[System.Serializable]
public class ResultObj
{
    public bool success;
    public string message;

    public new string ToString()
    {
        return success.ToString() + " " + message;
    }

}

[System.Serializable]
public class DeserializedResult
{
    public Payload payload;
    
    public new string ToString()
    {
        return payload.status.message + " " + payload.status.code + " " + payload.status.value.ToString() + " " + payload.status.heading + " " + payload.data.maxReachedLevel.ToString() + " " + payload.data.trophies.ToString();
    }

}

[System.Serializable]
public class Payload
{
    public Status status;
    public Data data;
}

[System.Serializable]
public class Status
{
    public string message;
    public int code;
    public bool value;
    public string heading;
}

[System.Serializable]
public class Data
{
    public int maxReachedLevel;
    public int[] trophies;
}


   





