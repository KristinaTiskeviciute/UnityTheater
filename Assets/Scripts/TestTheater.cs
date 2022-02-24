using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SeatStatus { Available, Occupied}  //add unavailable, wheelchair accessible, other next
//could be its own class if needed
public struct Seat
{
    public int row { get; set; }
    public int seatNum { get; set; }
    public Image spriteImage { get; set; }
    public SeatStatus status { get; set; }
}

public class TestTheater : MonoBehaviour
{
    //some values used to generate a sample theater auditorium, varied number of seats per row just to showcase
    //ranges are arbitrary just dont go crazy with it
    [SerializeField, Range(7, 18)] int rowCount = 15;
    [SerializeField, Range(7, 19)] int seatsPerRowMin = 7;
    [SerializeField, Range(8, 20)] int seatsPerRowMax = 13;

    [SerializeField] GameObject seatUIobj_prefab;
    [SerializeField] GameObject rowUIobj_prefab;
    [SerializeField] GameObject layoutParentObj;

    [SerializeField] private Color defaultColor = Color.gray;
    [SerializeField] Color selectedColor = Color.green; //color of currently selected seat
    [SerializeField] Color occupiedSeatColor = Color.gray; // color of a seat already taken
    // consider also adding color for otherwise unavailable seat (broken, accessibility, social distancing etc.)
    [SerializeField] TMPro.TextMeshProUGUI seatNumberDisplayText;
    private List<Seat>[] seats;
    private int currentSelectionRow = 0; //should recommend better seat
    private int currentSelectionSeat = 0;
    bool initialized = false;
    const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    void Start()
    {
        if (!initialized)
            Initialize();
       
    }

    private void Initialize()
    {
        initialized = true;
        
        GenerateSeatCollection();
        GenerateLayoutUI();     
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SeatClicked(Vector2 seatLocation)
    {
        SelectSeat((int)seatLocation.x, (int)seatLocation.y);
    }

    void SelectSeat(int row, int seatNumber)
    {
        Seat clickedSeat = seats[row][seatNumber];
        if (clickedSeat.status == SeatStatus.Occupied)
        {
            //if admin then can refund tickets/free seats/mark as out of service etc etc
            return;
        }
        else
        {
            //reset prev selection to normal
            Seat prevSelection = seats[currentSelectionRow][currentSelectionSeat];
            if (prevSelection.status == SeatStatus.Occupied) 
            {
                prevSelection.spriteImage.color = occupiedSeatColor; 
            }
            else 
            {
                prevSelection.spriteImage.color = defaultColor; 
            }
            //update current selection to new seat
            currentSelectionRow = row;
            currentSelectionSeat = seatNumber;
            clickedSeat.spriteImage.color = selectedColor;
        }
        seatNumberDisplayText.text = letters[row] + "" + (seatNumber + 1);
    }

    public void ConfirmButtonPressed()
    {
        Seat current = seats[currentSelectionRow][currentSelectionSeat];
        current.spriteImage.color = occupiedSeatColor;
        current.status = SeatStatus.Occupied;
        seats[currentSelectionRow][currentSelectionSeat] = current;
    }

    void ChangeSeatColor(int row, int seatNum , Color newColor)
    {
        //check if row/col valid etc
        Image sprite = seats[row][seatNum].spriteImage;
        if (sprite != null)
            sprite.color = newColor;
    }

    void GenerateSeatCollection()
    {
        //generates sample auditorium layout, not sure what format they are entered/read in in real life but this should be fine for testing
        initialized = true;
        seats = new List<Seat>[rowCount];

        for (int r = 0; r<rowCount; r++)
        {        
            int seatCount = Random.Range(seatsPerRowMin, seatsPerRowMax);
            seats[r] = new List<Seat>();
            for (int s = 0; s<seatCount; s++)
            {
                Seat seatObj = new Seat();
                seatObj.row = r;
                seatObj.seatNum = s;
                seatObj.status = SeatStatus.Available; //lets make all seats available to start for now
                seats[r].Add(seatObj);
}
        }
    }
    void GenerateLayoutUI()
    {
        //makes auditorium layout UI from seat collection data
        for (int i = 0; i < seats.Length; i++)
        {
            GameObject row = Instantiate(rowUIobj_prefab);
            row.transform.SetParent(layoutParentObj.transform, false);
            for (int j = 0; j < seats[i].Count; j++)
            {
                GameObject seat = Instantiate(seatUIobj_prefab);
                seat.transform.SetParent(row.transform);
                Button seatBtn = seat.GetComponentInChildren<Button>();
                var r = i;
                var s = j;
                seatBtn.onClick.AddListener(() => SeatClicked(new Vector2(r, s)));
                Seat ss = seats[i][j];
                ss.spriteImage = seat.GetComponentsInChildren<Image>()[1];
                seats[i][j] = ss;
                switch (ss.status) //can map it if more colors needed
                {
                    case SeatStatus.Occupied:
                        ss.spriteImage.color = occupiedSeatColor;
                        break;
                    default:
                        ss.spriteImage.color = defaultColor;
                        break;
                }
                ss.spriteImage.color = defaultColor;
                seats[i][j] = ss;
            }
        }
        SelectSeat(currentSelectionRow, currentSelectionSeat);
    }
    void UpdateSeatsUI()
    {

    }
    
}
