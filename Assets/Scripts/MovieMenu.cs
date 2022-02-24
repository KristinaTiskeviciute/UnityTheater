using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieMenu : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> movieCards;
    [SerializeField] GameObject movieCardPrefab;
    List<Movie> movies;
    bool init = false;
    int currentMovieIndex = 0;
    [SerializeField] GameObject arrowButton_next;
    [SerializeField] GameObject arrowButton_prev;
    [SerializeField] GameObject theater;

    private void OnEnable()
    {
        LoadMovies();
        arrowButton_prev.gameObject.SetActive(true);
        arrowButton_next.gameObject.SetActive(true);
    }

    public void LoadMovies()
    {
        // move to callback for when the db is ready
        if (movies == null || movies.Count < 1)
        {
            movies = DBManager.Instance.GetMoviesByDate("2020-02-24");
            movieCards = new List<GameObject>();
            AddMovieCards();
        }
    }
    void AddMovieCards()
    {
        for (int i = 0; i < movies.Count; i++)
        {
            AddMovieCard(movies[i]);
        }
        movieCards[0].SetActive(true);
        Next();
        Next();
    }
    public void AddMovieCard(Movie movie)
    {
        
        GameObject movieCard = (GameObject)Instantiate(movieCardPrefab);
        movieCard.transform.SetParent(transform,false);
        Button seatBtn = movieCard.GetComponentInChildren<Button>();
        seatBtn.onClick.AddListener(() => MovieClicked());

        Sprite img = Resources.Load<Sprite>($"MovieImagesById/{movie.id}") as Sprite;
        MovieCard mc = movieCard.GetComponent<MovieCard>();
        mc.SetCardValues(img, movie.name, movie.duration);
        movieCards.Add(movieCard);
        movieCard.SetActive(false);
    }
    public void Next()
    {
        Debug.Log(currentMovieIndex);
        if (currentMovieIndex >= (movies.Count - 2))
            arrowButton_next.gameObject.SetActive(false); //last
        else
            arrowButton_prev.gameObject.SetActive(true);

        movieCards[currentMovieIndex].SetActive(false);
        currentMovieIndex++;
        movieCards[currentMovieIndex].SetActive(true);
    }
    public void Prev()
    {
        if (currentMovieIndex <= 1)
            arrowButton_prev.gameObject.SetActive(false);
        else
            arrowButton_next.gameObject.SetActive(true);

        movieCards[currentMovieIndex].SetActive(false);
        currentMovieIndex--;
        movieCards[currentMovieIndex].SetActive(true);
    }

    public void MovieClicked()//pass in index later
    {
        theater.SetActive(true);
        arrowButton_prev.gameObject.SetActive(false);
        arrowButton_next.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
