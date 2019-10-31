using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutItem : MonoBehaviour
{
    public Image image;
    public Sprite sprite;
    public Movie movieDetails;
    TextureDownloader textureDownloader;
    public Layout parentLayout;
    
    public float animationSpeed = 1;

    public void Create(Movie movieDetails, Sprite sprite, Layout layout){
        this.movieDetails = movieDetails;
        this.sprite = sprite;
        parentLayout = layout;
        image.sprite = sprite;
    }

     public void Create(Movie movieDetails, Layout layout){
        this.movieDetails = movieDetails;
        parentLayout = layout;
        gameObject.name = movieDetails.Title;
    }

    // Start is called before the first frame update
    void Start()
    {
        image.enabled = false;
        textureDownloader = new TextureDownloader();
        textureDownloader.GetTexture(movieDetails.imdbID, movieDetails.Poster, this, OnTextureLoaded); 
    }

    void OnTextureLoaded(Sprite sprite){
        image.sprite = sprite;
        image.enabled = true;
        this.sprite = sprite;
        Appear();
    }

    public void onItemClicked(){
        //open descrption layout
        parentLayout.openDescrpiton(movieDetails,sprite);
    }

    void OnDestroy(){
        StopAllCoroutines();
        textureDownloader.Stop();
    }

    void OnDisable(){
        StopAllCoroutines();
        textureDownloader.Stop();
    }


    #region Thumbnail Movment
    void Appear(){
        StartCoroutine(AppearCor());
    }

    IEnumerator AppearCor(){
        transform.localScale = Vector3.zero;
        float lerper = 0;

        while(lerper < 1){
            transform.localScale = Vector3.Lerp(Vector3.zero,Vector3.one,lerper);
            lerper += Time.deltaTime * animationSpeed;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }


    public void MoveTo(Vector3 position){
        if(Vector3.Distance(transform.position,position)>1){
            StartCoroutine(MoveToCor(position));
        }
    }

    IEnumerator MoveToCor(Vector3 position){
        Vector3 initialPosition = transform.position;
        float lerper = 0;
        while (lerper < 1){

             transform.position = Vector3.Lerp(initialPosition,position,lerper);
             lerper += Time.deltaTime * animationSpeed;
             yield return null;
         }
        transform.position = position;
    }

    public void Destroy(){
        StartCoroutine(DestroyCor());

    }

    IEnumerator DestroyCor(){
        float lerper = 0;
        while(lerper < 1){
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, lerper);
            lerper += Time.deltaTime * animationSpeed;
            yield return null;
        }
        Destroy(gameObject);
    }
    #endregion

}
