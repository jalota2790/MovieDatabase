using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Layout : MonoBehaviour
{

    public Transform content;
    public GameObject itemPrefab;
    public string lastQuery;

    public int rowsCount, columnCount;

    public Transform[] postionHolder;

    Vector3[] positions;

    public DescrptionLayout descrptionLayout;
    public List<Movie> currentArray;

    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3[postionHolder.Length];
        for(int i = 0; i < positions.Length; i++){
            positions[i] = postionHolder[i].position;
        }

    }

    public void AddList(Movie[] movies){
        StartCoroutine(AddSorted(movies));
    }


    IEnumerator AddSorted(Movie[] movies){
        print("Add Sorted");
        int itemAdded = 0;
        RemoveItemsExcept(movies);
        for(int i = 0; i < movies.Length; i++){
            System.Array.Sort(movies, (x,y) => x.Title.CompareTo(y.Title));

            Movie movie = movies[i];

            if(string.IsNullOrWhiteSpace (movie.Poster) || movie.Poster.Equals("N/A") || currentArray.Contains(movie) ){
                continue;
            }
            AddItem(movie, i);
            currentArray.Add(movie);
            itemAdded++;
        }

        yield return new WaitForEndOfFrame();

        Movie[] _movie = currentArray.ToArray();
        System.Array.Sort(_movie, (x,y) => x.Title.CompareTo(y.Title));
        currentArray = _movie.ToList();

        yield return new WaitForEndOfFrame();

        for(int i = 0; i < currentArray.Count; i++){
            foreach (Transform child in content.transform ){
                LayoutItem layoutItem = child.GetComponent<LayoutItem>();
                if(layoutItem.movieDetails.Equals(currentArray[i])){
                    child.SetSiblingIndex(i);
                    MoveItem(layoutItem,i);
                }
            }
        }
    }

    void MoveItem(LayoutItem item, int position){
        item.MoveTo(positions[position]);
    }

    public void RemoveItemsExcept(Movie[] items){
        List<Movie> itemsList = items.ToList();

        foreach(Transform child in content.transform){
            LayoutItem layoutItem = child.GetComponent<LayoutItem>();
            Movie childMovieDetails = layoutItem.movieDetails;

            if(!itemsList.Contains(childMovieDetails)){
                currentArray.Remove(childMovieDetails);
                layoutItem.Destroy();
            }
        }
    }

    public void AddItem(Movie movieDetails, int index){
        StartCoroutine(AddItemCor(movieDetails,index));
    }

    IEnumerator AddItemCor(Movie movieDetails, int index){
        yield return null;
        //Instatiate the layout item here
        GameObject objectGO = Instantiate(itemPrefab,content);
        objectGO.GetComponent<LayoutItem>().Create(movieDetails, this);
        objectGO.transform.SetSiblingIndex(index);
        objectGO.transform.position = positions[index];
    }

    public void clear(){
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        currentArray = new List<Movie>();
    }

    public void openDescrpiton(Movie movieDetails, Sprite image){
        descrptionLayout.openDescrption(movieDetails, image, lastQuery);
    }



}
