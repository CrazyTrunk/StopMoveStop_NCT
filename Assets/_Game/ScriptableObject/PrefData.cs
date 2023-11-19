
using UnityEngine;

[System.Serializable]
public struct PlayerPrefsInt
{
    string key;
    [SerializeField] int value;

    public int Value { get => value; set { this.value = value; PlayerPrefs.SetInt(key, value); } }

    public PlayerPrefsInt(string key, int defaultValue)
    {
        this.key = key;
        this.value = PlayerPrefs.GetInt(key, defaultValue);
    }
}
[System.Serializable]
public struct PlayerPrefsFloat
{
    string key;
    [SerializeField] float value;

    public float Value { get => value; set { this.value = value; PlayerPrefs.SetFloat(key, value); } }

    public PlayerPrefsFloat(string key, float defaultValue)
    {
        this.key = key;
        this.value = PlayerPrefs.GetFloat(key, defaultValue);
    }
}
[System.Serializable]
public struct PlayerPrefsString
{
    string key;
    [SerializeField] string value;

    public string Value { get => value; set { this.value = value; PlayerPrefs.SetString(key, value); } }

    public PlayerPrefsString(string key, string defaultValue)
    {
        this.key = key;
        this.value = PlayerPrefs.GetString(key, defaultValue);
    }
}
