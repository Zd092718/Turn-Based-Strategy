using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance { get; private set; }


    private void Awake()
    {
        instance = this; 
    }

    [SerializeField] private AudioSource deathHuman, deathRobot, impact, meleeHit, takeDamage, uiCancel, uiSelect;
    [SerializeField] private AudioSource[] shootSounds;

    public void PlayShoot()
    {
        shootSounds[Random.Range(0, shootSounds.Length)].Play();
    }


    #region !Getters and Setters!
    public AudioSource DeathHuman { get => deathHuman; set => deathHuman = value; }
    public AudioSource DeathRobot { get => deathRobot; set => deathRobot = value; }
    public AudioSource Impact { get => impact; set => impact = value; }
    public AudioSource MeleeHit { get => meleeHit; set => meleeHit = value; }
    public AudioSource TakeDamage { get => takeDamage; set => takeDamage = value; }
    public AudioSource[] ShootSounds { get => shootSounds; set => shootSounds = value; }
    public AudioSource UiCancel { get => uiCancel; set => uiCancel = value; }
    public AudioSource UiSelect { get => uiSelect; set => uiSelect = value; }
    #endregion
}
