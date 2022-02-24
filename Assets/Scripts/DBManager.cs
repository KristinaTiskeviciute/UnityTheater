using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;


public struct Movie
{
    public int id { get; set; }
    public string name { get; set; }
    public int duration { get; set; }
}

public class DBManager : MonoBehaviour
{

    private static DBManager _instance;
    public static DBManager Instance { get { return _instance; } }
    IDbConnection dbcon;
    IDataReader reader;
    bool initialized = false;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(!initialized)
            InitializeValues();  
    }

    private void OnDestroy()
    {
        CloseDBConnection();
    }

    // Update is called once per frame
    private void InitializeValues()
    {
        initialized = true;
        CreateDBConnection();
        CreateMovieTable();
        AddMovies();
        CreateShowingsTable();
        AddShowings();
    }

    private void CreateDBConnection()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "My_Database";
        dbcon = new SqliteConnection(connection);
        dbcon.Open();
    }

    private void CreateTable(string commandString)
    {
        IDbCommand dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = commandString;
        reader = dbcmd.ExecuteReader();
    }


    /// <summary>
    /// Movie data
    /// </summary>

    private void CreateMovieTable() 
    {
        string s = "CREATE TABLE IF NOT EXISTS " + "movies" + " (" +
          "id_pk" + " INTEGER PRIMARY KEY, " +
          "name" + " VARCHAR, " +
          "duration" + " INTEGER "+ ")";
        CreateTable(s); 
    }
    void AddMovies() //load from imdb
    {
        AddMovie(0, "Uncharted", 115); 
        AddMovie(1, "Marry Me", 112); 
        AddMovie(2, "Spider-man: No Way Home", 150); 
        AddMovie(3, "Jackass Forever", 96); 
        AddMovie(4, "Blacklight", 105); 
        AddMovie(5, "Badhaai Do", 147); 
        AddMovie(6, "The Cursed", 112); 
        AddMovie(7, "Death on the Nile", 127); 
        AddMovie(8, "Dog", 101); 
    }

    private void AddMovie(int id, string title, int duration) 
    {
        IDbCommand cmnd = dbcon.CreateCommand();
        string s = $"INSERT OR IGNORE INTO movies (id_pk, name, duration ) VALUES ({id}, '{title}', {duration})";
        cmnd.CommandText = s;
        cmnd.ExecuteNonQuery();
    }


    /// <summary>
    /// Movie Showing data
    /// </summary>

    private void CreateShowingsTable()
    {
        string s = "CREATE TABLE IF NOT EXISTS " + "showings" + " (" +
          "showing_id_pk" + " INTEGER PRIMARY KEY, " +
          "movie_id" + " INTEGER, " +
          "start_date" + " TEXT, " +
          "start_time" + " TEXT, " +
          "auditorium" + " INTEGER " + ")";
        Debug.Log(s);
        CreateTable(s);
    }

    //Dummy Data
    //TODO: use some room allocation scheduling algorithm here given set number of auditoria, theater opening and closing time, preview time, and min amt of cleaning time between showings round up to 15 min mark
    // + auditorium size and movie popularity etc
    private void AddShowings()
    {
        AddShowing(0, 0, "2020-02-24", "09:00:00", 0); AddShowing(1, 2, "2020-02-24", "09:00:00", 1); AddShowing(2, 3, "2020-02-24", "09:00:00", 2); 
        AddShowing(3, 1, "2020-02-24", "09:00:00", 3); AddShowing(4, 7, "2020-02-24", "09:00:00", 4); AddShowing(5, 4, "2020-02-24", "11:30:00", 0); 
        AddShowing(6, 5, "2020-02-24", "12:30:00", 1); AddShowing(7, 6, "2020-02-24", "11:15:00", 2); AddShowing(8, 8, "2020-02-24", "11:30:00", 3); 
        AddShowing(9, 2, "2020-02-24", "11:45:00", 4); AddShowing(10, 3, "2020-02-24", "14:00:00", 0); AddShowing(11, 0, "2020-02-24", "15:15:00", 1); 
        AddShowing(12, 6, "2020-02-24", "13:45:00", 2); AddShowing(13, 4, "2020-02-24", "14:00:00", 3); AddShowing(14, 8, "2020-02-24", "15:15:00", 4); 
        AddShowing(15, 7, "2020-02-24", "14:15:00", 0); AddShowing(16, 1, "2020-02-24", "17:45:00", 1); AddShowing(17, 5, "2020-02-24", "16:15:00", 2); 
        AddShowing(18, 6, "2020-02-24", "16:30:00", 3); AddShowing(19, 4, "2020-02-24", "17:45:00", 4); AddShowing(20, 4, "2020-02-24", "17:00:00", 0); 
        AddShowing(21, 3, "2020-02-24", "20:15:00", 1); AddShowing(22, 6, "2020-02-24", "19:30:00", 2); AddShowing(23, 2, "2020-02-24", "19:00:00", 3); 
        AddShowing(24, 0, "2020-02-24", "20:15:00", 4); AddShowing(25, 7, "2020-02-24", "19:30:00", 0); 

    }
    
    private void AddShowing(int showingId, int movieId, string startDate, string startTime, int auditorium )
    {
        IDbCommand cmnd = dbcon.CreateCommand();
        string s = $"INSERT OR IGNORE INTO showings (showing_id_pk, movie_id, start_date, start_time, auditorium ) VALUES ({showingId}, {movieId}, '{startDate}','{startTime}', {auditorium} )";
        cmnd.CommandText = s;
        cmnd.ExecuteNonQuery();
    }

    private List<int> GetMovieIDsByDate(string date) 
    {
        if (!initialized)
            InitializeValues();
        IDbCommand cmnd_read = dbcon.CreateCommand();
        string query = $"SELECT DISTINCT movie_id FROM showings WHERE start_date = '{date}'";
        cmnd_read.CommandText = query;
        List<int> idList = new List<int>();
        reader = cmnd_read.ExecuteReader();
        while (reader.Read())
        {
            idList.Add(Convert.ToInt32(reader[0]));
        }
        return idList;
    }

    private List<Movie> MakeMovieListFromIDs(List<int> idList)
    {
        List<Movie> movies = new List<Movie>();
        IDbCommand cmnd_read = dbcon.CreateCommand();
    
        string query = $"SELECT * FROM movies where id_pk IN ({string.Join<int>(",", idList)})";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        while (reader.Read())
        {
            Movie movie = new Movie();
            movie.id = Convert.ToInt32(reader[0]);
            movie.name = reader[1].ToString();
            movie.duration = Convert.ToInt32(reader[2]);
            movies.Add(movie);
        }
        return movies;
    }
    

    //list of unique movies from this days showings
    public List<Movie> GetMoviesByDate(string date)
    {
        return MakeMovieListFromIDs(GetMovieIDsByDate(date));
    }


    private void DropTable(string tableName)
    {
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "DROP TABLE IF EXISTS "+tableName;
        Debug.Log(cmnd.CommandText);
        cmnd.ExecuteNonQuery();
    }

    private void CloseDBConnection()
    {
        dbcon.Close();
    }
}
