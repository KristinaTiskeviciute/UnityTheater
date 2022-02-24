using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MovieCard : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMPro.TextMeshProUGUI titleText;
    [SerializeField] private TMPro.TextMeshProUGUI durationText;

    public void SetCardValues(Sprite sprite, string title, int duration)
    {
        image.sprite = sprite;
        titleText.text = title;
        durationText.text = $"{duration%60}h {duration-(duration%60)}m";
    }
}
