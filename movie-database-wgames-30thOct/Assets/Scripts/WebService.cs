using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class WebService 
{
    MonoBehaviour monoBehaviour;
    Coroutine coroutine;

    public WebService(ISearchController searchcontroller, string url){
        this.monoBehaviour = (MonoBehaviour)searchcontroller;
        coroutine = ((MonoBehaviour)searchcontroller).StartCoroutine(FetchData(url, searchcontroller));
    }  

    IEnumerator FetchData(string  url, ISearchController searchcontroller)
    {
        MonoBehaviour.print("Requesting...  "+ url);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if(www.isHttpError || www.isNetworkError){
            searchcontroller.Error(www.error);
        }else{
            SearchResults currentResults = JsonUtility.FromJson<SearchResults>(www.downloadHandler.text);
        
            if(currentResults.Response.Equals("True")){
                searchcontroller.HandleSearchResults(currentResults);
                MonoBehaviour.print(currentResults.Search.Length + "  movies found");
            }else{
                searchcontroller.Error("No Result Found");
            }
        }
    }

    public void Stop(){
        if(coroutine != null){
            monoBehaviour.StopCoroutine(coroutine);
        }
    }

}
