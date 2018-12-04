using UnityEngine;

[System.Serializable]
public class sound
{

    public string name;
    public AudioClip clip;

    private float goalVolume = 0.7f;
    private float startVolume = 0.7f;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float RandomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float RandomPitch = 0.1f;

    private AudioSource source;

    public void setSource(AudioSource _scource)
    {
        source = _scource;
        source.clip = clip;
    }

    public void play(bool loop = true)
    {
        source.volume = volume * (1 + Random.Range(-RandomVolume / 2f, RandomVolume / 2f));
        Debug.Log("playing sound: " + name + " with volume: " + source.volume);
        source.pitch = pitch * (1 + Random.Range(-RandomPitch / 2f, RandomPitch / 2f));
        source.Play();
        source.loop = loop;
    }

    public void Stop()
    {
        source.Stop();
    }

    public void ChangeVolume(float change)
    {
        source.volume += change;
        if (source.volume < 0)
        {
            source.volume = 0;
        }
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }

    public bool HasVolume()
    {
        return source.volume > 0;
    }

    public void setVolume(float _volume)
    {
        volume = _volume;
        source.volume = _volume;
    }

    public void SetSpatialBlend(float sBlend)
    {
        source.spatialBlend = sBlend;
    }

    public void SetTime(float time)
    {
        source.time = time;
    }

    public float GetGoalVolume()
    {
        return goalVolume;
    }

    public float GetStartVolume()
    {
        return startVolume;
    }

    public void SetGoalVolume(float vol)
    {
       goalVolume = vol;
    }

    public void SetStartVolume(float vol)
    {
        startVolume = vol;
    }
}

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    private float transitionDuration = 3f;
    private float currentTime = 0f;
    private bool inTransition = false;

    [SerializeField]
    sound[] sounds;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one AudioManager in the scene");
        }
        else
        {
            instance = this;
        }

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound" + i + " " + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].setSource(_go.AddComponent<AudioSource>());
        }
    }

    public void PlaySound(string _name, float volume, float time = 0, bool loop = true)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].SetGoalVolume(volume);
                sounds[i].SetTime(time);
                sounds[i].setVolume(volume);
                sounds[i].play(loop);
                return;
            }
        }
        // the sound has not been found
        Debug.LogWarning("SoundManager: sound of name *" + _name + "* not found");
    }

    public void TransitionSound(string _name, float volume = 1f, float transTime = 0, float time = 0)
    {
        transitionDuration = transTime;
        currentTime = 0f;
        inTransition = true;
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].SetGoalVolume(volume);
                if (volume == 0)
                {
                    sounds[i].SetStartVolume(sounds[i].volume);
                }
                else
                {
                    sounds[i].play();
                    sounds[i].SetTime(time);
                    sounds[i].SetStartVolume(0f);
                    sounds[i].setVolume(0f);
                }
                
                return;
            }
        }
        // the sound has not been found
        Debug.LogWarning("SoundManager: sound of name *" + _name + "* not found");
    }

    public void PlaySpatialSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].SetSpatialBlend(1f);
                sounds[i].play();
                return;
            }
        }
        // the sound has not been found
        Debug.LogWarning("SoundManager: sound of name *" + _name + "* not found");
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }
        // the sound has not been found
        Debug.LogWarning("SoundManager: sound of name *" + _name + "* not found");
    }

    public bool IsPlaying(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                return sounds[i].IsPlaying();
            }
        }
        // the sound has not been found
        Debug.LogWarning("SoundManager: sound of name *" + _name + "* not found");
        return false;
    }

    public void ChangeVolume(string _name, float change)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].ChangeVolume(change);
                return;
            }
        }
        // the sound has not been found
        Debug.LogWarning("SoundManager: sound of name *" + _name + "* not found");
    }

    public bool HasVolume(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                return sounds[i].HasVolume();
            }
        }
        // the sound has not been found
        Debug.LogWarning("SoundManager: sound of name *" + _name + "* not found");
        return false;
    }

    void Update()
    {
        if (inTransition) {
            currentTime += Time.deltaTime;
            if(currentTime > transitionDuration)
            {
                currentTime = transitionDuration;
                inTransition = false;
            }
            for (int i = 0; i < sounds.Length; i++)
            {
                UpdateSound(sounds[i]);
            }
        }
    }

    private void UpdateSound(sound s)
    {
        if(s.volume != s.GetGoalVolume())
        {
            s.setVolume(s.GetStartVolume() + (currentTime / transitionDuration) * (s.GetGoalVolume() - s.GetStartVolume()));
            if(s.volume == 0)
            {
                s.Stop();
            }
        }
    }
}
