
// --- By VEOdev ---
// You are not allow to resell or show this asset.
// This asset is also under the unity asset store licence.

using UnityEngine;

namespace VEO
{
    [CreateAssetMenu(fileName = "DataConfig", menuName = "Data Configuration")]
    public class DataConfiguration : ScriptableObject
    {
        public string FileName = "game";
        public string FileExtension = ".save";
        public string BackupFileExtension = ".bak";
        [Space]
        [Tooltip("Encrypt data using an encryption key to make data non-redable for security, recommanded ON")]
        public bool EncryptionEnabled = true;
        [Tooltip("Encryption key that is used to encrypt the data, use a complex key, more complex is more secure, don't change it after you save data")]
        public string EncryptionKey = "0A1B2C3D4E5F6A7B8C9ER";
        [Space]
        [Tooltip("Load data automatically if exist when you create a new data object")]
        public bool AutoLoad = true;
        [Tooltip("Show debug messages in consol")]
        public bool Debug = false;
        public bool ColoredDebug = false;
    }
}
