    //Class that handles player settings (Mouse sensitivity, volume, etc)
    //All data is stored in PlayerPrefs

    using UnityEngine;

    public class PlayerSettings
    {
        public static float mouseSensitivity = 1f;
        public static float volume = 1f;
        
        public static void Load()
        {
            mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity", 1f);
            volume = PlayerPrefs.GetFloat("volume", 1f);
        }
        
        public static void Save()
        {
            PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivity);
            PlayerPrefs.SetFloat("volume", volume);
        }
        
        public static void Reset()
        {
            PlayerPrefs.DeleteAll();
        }
        
        public static void SetMouseSensitivity(float value)
        {
            mouseSensitivity = value;
            Save();
        }
        
        public static void SetVolume(float value)
        {
            volume = value;
            Save();
        }

    }