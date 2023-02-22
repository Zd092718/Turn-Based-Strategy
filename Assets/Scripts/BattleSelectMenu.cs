using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSelectMenu : MonoBehaviour
{
    [SerializeField] private string mainMenu;
   
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
    
    public void LoadBattle(string battleToLoad)
    {
        SceneManager.LoadScene(battleToLoad);
    }
}
