using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;

    public GameManager gameManager;

    [System.Serializable]
    public class MusicTrack
    {
        public string key;
        public AudioClip clip;
    }

    public List<MusicTrack> musicTracks;

    private Dictionary<string, AudioClip> musicDictionary;

    void Awake()
    {
        // Crear el diccionario a partir de la lista
        musicDictionary = new Dictionary<string, AudioClip>();

        foreach (var track in musicTracks)
        {
            if (!musicDictionary.ContainsKey(track.key))
                musicDictionary.Add(track.key, track.clip);
        }
    }

    public void Start()
    {
        PlayMusic("BaseSong");
    }

    public void Update()
    {
        if(gameManager.Dead == true){
            PlayMusic("DeathSong");
        }
    }

    public void PlayMusic(string key)
    {
        if (musicDictionary.ContainsKey(key))
        {
            AudioClip clipToPlay = musicDictionary[key];

            if (audioSource.clip != clipToPlay)
            {
                audioSource.clip = clipToPlay;
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("No se encontró música para: " + key);
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
