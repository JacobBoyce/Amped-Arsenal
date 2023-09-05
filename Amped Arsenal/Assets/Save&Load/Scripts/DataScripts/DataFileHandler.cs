using System.IO;


// --- By VEOdev ---
// You are not allow to resell or show this asset.
// This asset is also under the unity asset store licence.

namespace VEO
{
    public static class DataFileHandler
    {
        // Local
        public static string LoadLocal(string path)
        {
            string JsonFile = null;

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                using StreamReader reader = new StreamReader(stream);
                JsonFile = reader.ReadToEnd();
            }

            return JsonFile;
        }
        public static void SaveLocal(string path, string jsonFile)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using StreamWriter writer = new StreamWriter(stream);
                writer.Write(jsonFile);
            }
        }


        // Online
        /*
        public static async Task<string> LoadOnline(string url)
        {
             Here you can return the string json file from your server using your method.
        }
        public static Task SaveOnline(string url, string jsonFile)
        {
             Here you can save the string json file to your server using your method.
        }
        */
    }
}
