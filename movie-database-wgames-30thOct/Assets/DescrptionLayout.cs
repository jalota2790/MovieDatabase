using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DescrptionLayout : MonoBehaviour,ISearchController
{
    public Image image;
    public Text detailText;
    public Layout layout;

    [Space]
    public string BaseUrl;
    public string lastQuery;

    Movie movieDetails;

    public void openDescrption(Movie movieDetails, Sprite image, string searchQuery){
        gameObject.SetActive(true);
        this.image.sprite = image;
        detailText.text = GetData(movieDetails);
        layout.clear();
        Search(BaseUrl + searchQuery+"&y="+movieDetails.Year);//All movies with same query , relased durng same year will be shown.
        lastQuery = searchQuery;
    }

    string GetData(Movie movieDetails){
            return movieDetails.Title + "\n Year: "+ movieDetails.Year;
    }

    public void closeBtnClicked(){
        this.gameObject.SetActive(false);
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
        searchResults.Search.ToList().Except(new List<Movie>{movieDetails}).ToArray();
        layout.AddList(searchResults.Search);
    }

    public void Error(string error){
        print("Search Error  " + error);
    }
    #endregion

}
