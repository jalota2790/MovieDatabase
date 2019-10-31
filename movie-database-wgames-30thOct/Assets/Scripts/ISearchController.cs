

public interface ISearchController
{
    void Search(string url);
    void HandleSearchResults(SearchResults searchResults);
    void Error(string error);
} 
