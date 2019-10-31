using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour, ISearchController
{
    public Layout layout;

    public string baseUrl;

    public InputField queryInputField;

    // Start is called before the first frame update
    void Start()
    {
        queryInputField.onValueChanged.AddListener(SearchCurrentInput);
    }

    string lastQuery;
    void SearchCurrentInput(string text){
        lastQuery = text;
        Search(baseUrl + text);
    }

    #region ISearchController
    WebService webService;
    public void Search(string url){
        if(webService != null){
            webService.Stop();
        }
        webService = new WebService(this, url);
    }

    public void HandleSearchResults(SearchResults searchResults){
        layout.lastQuery = lastQuery;
        layout.AddList(searchResults.Search);
    }

    public void Error(string error){
        print("Search Error  " + error);
    }
    #endregion

}
