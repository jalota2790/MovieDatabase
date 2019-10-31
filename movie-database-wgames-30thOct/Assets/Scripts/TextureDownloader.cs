using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class TextureDownloader
{
    MonoBehaviour monoBehaviour;
    Coroutine coroutine;

    public void GetTexture(string cacheId, string url, MonoBehaviour monoBehaviour, System.Action<Sprite> callBack){
        //string dirPath = Path.Combine(Application.dataPath,"Assets");
        string dirPath = Path.Combine(Application.dataPath);

        string filePath = Path.Combine(dirPath, cacheId);
                    MonoBehaviour.print("Downloading tetures... "+filePath);

        if(Directory.Exists(dirPath)){
            if(File.Exists(filePath)){
                MonoBehaviour.print("Loading textures from cache");
                //loading texture from cache logic here
                LoadTexture(dirPath,cacheId,callBack);
                return;
            }
            MonoBehaviour.print("Downloading tetures... "+dirPath +"/"+filePath);
            this.monoBehaviour = monoBehaviour;
            coroutine = monoBehaviour.StartCoroutine(downloadTexture(url,dirPath,filePath,callBack));
        }
    }

    IEnumerator downloadTexture(string url, string dirPath, string path, System.Action<Sprite> callBack){
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        Texture2D texture = DownloadHandlerTexture.GetContent(www);
        if(!Directory.Exists(dirPath)){
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(path, texture.EncodeToPNG());
        callBack(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
    }


    void LoadTexture(string dirPath, string cacheId, System.Action<Sprite> callBack){
        Texture2D texture = new Texture2D(1,1);
        ImageConversion.LoadImage(texture, File.ReadAllBytes(Path.Combine(dirPath,cacheId)));
        callBack(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
    }

    public void Stop(){
        if(coroutine != null){
            monoBehaviour.StopCoroutine(coroutine);
        }
    }

}
