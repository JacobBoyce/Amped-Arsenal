using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using VEO;


// --- By VEOdev ---
// You are not allow to resell or show this asset.
// This asset is also under the unity asset store licence.

// Avoid touching this code only if you know what are you doing.


namespace DataManagement
{
    public class Data
    {
        // Configuration
        private readonly DataConfiguration config;
        private readonly string fileName;
        private readonly string backupFileName;

        // Private
        private Dictionary<string, object> data;
        private readonly string profileID;
        private bool triedToBack = false;

        // Public
        public string FilePath => Path.Combine(Application.persistentDataPath, profileID, fileName);
        public string BackupFilePath => Path.Combine(Application.persistentDataPath, profileID, backupFileName);
        public string JsonFile { get; private set; }
        public bool isEmpty => !File.Exists(Path.Combine(Application.persistentDataPath, profileID, fileName));
        public bool isLoaded { get; private set; }
        public bool isSaved { get; private set; }

        // Constructor
        public Data(string profileID)
        {
            // set the profile id.
            this.profileID = profileID;

            // init the data dictionary.
            data = new Dictionary<string, object>();

            // set data settings.
            config = Resources.Load<DataConfiguration>("DataConfig");
            fileName = config.FileName + config.FileExtension;
            backupFileName = config.FileName + config.BackupFileExtension;


            // set the global converter settings
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // if the profile Exist load the data profile
            if (config.AutoLoad && !isEmpty)
                LoadProfile();
        }

        // Data
        public void SetData<T>(string tag, T value)
        {
            if (data.ContainsKey(tag))
            {
                data[tag] = value;
                Message("The data '" + tag + "' updated succefully!", "#FFB200");
            }
            else
            {
                data.Add(tag, value);
                Message("The data '" + tag + "' added succefully!", "#FFB200");
            }

            isSaved = false;
        }
        public T GetData<T>(string tag)
        {
            if (!data.ContainsKey(tag))
            {
                Debug.LogError("Failed to get data. The data with the tag (" + tag + ") does not exist");
                return default;
            }

            return JToken.FromObject(data[tag]).ToObject<T>();
        }
        public void DeleteData(string tag)
        {
            if (this.data.ContainsKey(tag))
            {
                this.data.Remove(tag);
                Message("The data '" + tag + "' Deleted Succefully!", "#FF0000");

                isSaved = false;
            }
            else
            {
                Debug.LogError("Failed to delete data. The data with the tag ((" + tag + ")) does not exist");
            }
        }
        public bool HasData(string tag)
        {
            return data.ContainsKey(tag);
        }

        // Profile
        public void SaveProfile()
        {
            try
            {
                // Create the directory.
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

                // Serializing the data into JSON.
                JsonFile = JsonConvert.SerializeObject(data);

                // Encrypt the data.
                if (config.EncryptionEnabled)
                {
                    PlayerPrefs.SetInt("Encrypted", 1);
                    JsonFile = DataEncryption(JsonFile);
                }
                else
                {
                    PlayerPrefs.SetInt("Encrypted", 0);
                }

                // Writing the data into local JSON file.
                DataFileHandler.SaveLocal(FilePath , JsonFile);
                //DataFileHandler.SaveOnline(YourURL, JsonFile);

                // Debug
                Message("Data saved succefully. ProfileID : " + profileID, "#00CD19");
                isSaved = true;

                // Make a backup file.
                CreateBackup();
            }
            catch (Exception error) // In case something went wrong.
            {
                Debug.LogError("Failed to save data in " + FilePath + "\n" + error.ToString());
            }
        }
        public void LoadProfile()
        {
            if (!isEmpty)
            {
                try
                {
                    data = null;

                    // Read the JSON file.
                    JsonFile = DataFileHandler.LoadLocal(FilePath);
                    // JsonFile = DataFileHandler.LoadOnline(YourURL);

                    // Decrypt the data
                    if (PlayerPrefs.GetInt("Encrypted") == 1)
                        JsonFile = DataEncryption(JsonFile);

                    // Deserialize the data from JSON.
                    data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonFile);

                    // Debug
                    Message("Data loaded succefully. ProfileID : " + profileID, "#00CD19");
                    isLoaded = true;

                    // Reset backup tries
                    triedToBack = false;
                }

                catch (Exception error)
                {
                    // In case something went wrong.
                    Debug.LogWarning("Failed to load data. Attempting to use the backup file" + "\n" + error.ToString());

                    // Try to load from the backup file.
                    if (!triedToBack && TryToBackup(FilePath))
                    {
                        triedToBack = true;
                        LoadProfile();
                    }
                    else
                    {
                        Debug.LogError("Failed to load the data from the backup file!" + "\n" + error.ToString() + "\n" + FilePath);
                    }
                }
            }
            else
            {
                // In case file path doesn't exist.
                Debug.LogWarning("The path " + FilePath + " does not exist");
            }
        }
        public void DeleteProfile()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    Directory.Delete(Path.GetDirectoryName(FilePath), true);
                }
                else
                {
                    Debug.LogWarning("Failed to delete the profile (" + profileID + "). The profile path does not exist");
                }

                Message("The profile (" + profileID + ") was succefully deleted", "#FF0000");
            }
            catch (Exception error)
            {
                Debug.LogError("Failed to delete profile (" + profileID + ") \n" + error.ToString());
            }
        }
        public void OpenProfileLocation()
        {
            string path = Path.Combine(Application.persistentDataPath, profileID);

            if (File.Exists(FilePath))
            {
                System.Diagnostics.Process.Start(path);
            }
            else
            {
                Debug.LogWarning("Failed to open profile location. Path does not exist.");
            }
        }

        // Helpers
        private string DataEncryption(string jsonData)
        {
            string modifiedJsonData = "";

            for (int i = 0; i < jsonData.Length; i++)
                modifiedJsonData += (char)(jsonData[i] ^ config.EncryptionKey[i % config.EncryptionKey.Length]);

            return modifiedJsonData;
        }
        private bool TryToBackup(string path)
        {
            bool successful = false;

            try
            {
                if (File.Exists(BackupFilePath))
                {
                    File.Copy(BackupFilePath, path, true);
                    Debug.LogWarning("Data loaded using the backup file! original file was currepted");
                    successful = true;
                }
                else
                {
                    throw new Exception("Failed to backup the data. No backup file found!");
                }
            }
            catch (Exception error)
            {
                Debug.LogError("Error when trying to load the data from the backup file!" + "\n" + error);
            }

            return successful;
        }
        private void CreateBackup()
        {
            if (data != null)
                File.Copy(FilePath, BackupFilePath, true);
            else
                throw new Exception("Failed to created a backup file");
        }
        private void Message(string message, string hexColor)
        {
            if (config.Debug)
                Debug.Log(SetColor(message, hexColor));
        }
        private string SetColor(string s, string hex)
        {
            if (config.ColoredDebug)
                return $"<color=" + hex + ">" + s + "</color>";
            else
                return s;
        }
    }
}