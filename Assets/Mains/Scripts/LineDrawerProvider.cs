using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawerProvider : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] public Image _barFill;
    [SerializeField] public Image Star1;
    [SerializeField] public Image Star2;
    [SerializeField] public Image Star3;

    [Header("Time")]
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI LevelText;
}