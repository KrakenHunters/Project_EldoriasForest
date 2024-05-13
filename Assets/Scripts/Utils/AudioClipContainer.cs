using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class AudioClipContainer : ScriptableObject
{
  // public List<AudioPair> audioPairs;   // Might add later
    [Header("BGMusic")]

    public AudioClip MainMenu;
    public AudioClip BGMusic;
    public AudioClip WinBGMusic;
    //public AudioClip CaveBGMusic;

    /*//Might add later 
    public AudioClip Cave;
    public AudioClip DeepCave;
    public AudioClip Grassland;*/

    [Header("WorldSFX")]

    public AudioClip CollectedPickup;
    public AudioClip TeleportOut;
    public AudioClip TeleportIn;
    public AudioClip Teleport;
    public AudioClip BreakingWall;

    public AudioClip PlantGrowing;
    public AudioClip PlantWatering;



    [Header("PlayerSFX")]

    public AudioClip Jump;
    public AudioClip Death;
    public AudioClip Glide;
    public AudioClip Drown;
    public AudioClip Hit;

    public AudioClip WaterSplash;
    public AudioClip IcePunch;
    public AudioClip WindStun;

    [Header("EnemySFX")]

    public AudioClip EnemyDeath;
    public AudioClip EnemyStun;
    public AudioClip EnemyAttack;
    public AudioClip EnemyShoot;
    public AudioClip FireEstinguish;
    public AudioClip FireBurning;
}

//code f0or using modular audio container with types    
/*[Serializable]
    public struct AudioPair
    {
        public AudioType Key;
        public AudioClip Value;
    }

[Serializable]
public enum AudioType
{
    BGMusic,
    WorldSFX,
    PlayerSFX,
    EnemySFX
}*/