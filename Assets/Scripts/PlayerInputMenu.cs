using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMenu : MonoBehaviour
{
    public static PlayerInputMenu Instance { get; private set; }

    [SerializeField] private GameObject inputMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void HideMenus()
    {
        inputMenu.SetActive(false); 
    }

    public void ShowInputMenu()
    {
        inputMenu.SetActive(true);
    }
}
