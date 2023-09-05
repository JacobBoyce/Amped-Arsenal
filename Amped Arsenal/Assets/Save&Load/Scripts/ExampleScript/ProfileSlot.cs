
// --- By VEOdev ---
// You are not allow to resell or show this asset.
// This asset is also under the unity asset store licence.

using UnityEngine;
using UnityEngine.UI;
using DataManagement; // Use this namespace to get access to the Data Script.

// This in an example script for a profile slot.
// You can have multiple save profiles, make sure each one has unique profileID.

namespace VEO
{
    public class ProfileSlot : MonoBehaviour
    {
        // DATA
        [HideInInspector] public Data data;

        // Config
        [Header("Config")]
        [SerializeField] private string profileID = "0";

        // Text
        [Header("Text")]
        [SerializeField] private Text profileText;
        [SerializeField] private Text progressText;
        [SerializeField] private Text positionText;

        // Buttons
        [Header("Buttons")]
        [SerializeField] private GameObject CreateButton;
        [SerializeField] private GameObject RandomizeButton;
        [SerializeField] private GameObject SaveButton;
        [SerializeField] private GameObject DeleteButton;
        [SerializeField] private GameObject OpenFileButton;

        // Properties, example Data. 
        // You can also store classes, lists, dictionary ... 
        private int progress;
        private Vector3 position;


        // Start
        void Start()
        {
            // get the data with the specified profile.
            // if the profile exist, the data will be auto loaded.
            data = new Data(profileID);

            // here we check if there is a saved data.
            // if there is we load the slot.
            // in this example we chose the data "Progress", because it saves as soon as we play.
            if (data.HasData("Progress"))
            {
                LoadSlotData();

                // Update Button
                OpenFileButton.SetActive(true);
                DeleteButton.SetActive(true);
            }
        }

        private void LoadSlotData()
        {
            // this is how you get the data.
            // the tag is key sensitive make sure to type it right.
            // also it is a type sensitive so make sure you type the correct data type inside the <>.
            progress = data.GetData<int>("Progress");
            position = data.GetData<Vector3>("Position");

            // Update Buttons
            CreateButton.SetActive(false);
            RandomizeButton.SetActive(true);
            SaveButton.SetActive(true);

            // Update Text
            profileText.text = "Profile " + profileID;
            progressText.text = progress + "%";
            positionText.text = position.ToString();
        }
        public void NewGame()
        {
            // set the slot data for a new game.
            progress = 0;
            position = new Vector3(0, 0, 0);

            // this is how you set a data.
            // if the data already exist it will get updated. Else it will get created.
            data.SetData("Progress", progress);
            data.SetData("Position", position);

            // this to reload the slot.
            // if new game button will load new scene, change it with loading your game scene.
            LoadSlotData();
        }
        public void Randomize()
        {
            // we randomize the data just to mimic that the player is playing.
            progress = Random.Range(0, 100);
            position = new Vector3(Random.Range(0, 50), Random.Range(0, 50), 0);

            // this is how you set data, as you can see we set an int and a vector2 with the same method.
            // we recommand you to set a class for a large type of data, something like player stats ... 
            data.SetData("Progress", progress);
            data.SetData("Position", position);

            // Update Text
            progressText.text = progress + "%";
            positionText.text = position.ToString();

            // you don't need this function for a game this is just to show case the asset.
            // you can change this function with "Continue Game".
            // make sure to pass the DataHolder reference to your data manager to access it.
        }
        public void Delete()
        {
            // this is how you delete an existing profile.
            // all the data will be lost! always make a confirmation popup.
            data.DeleteProfile();

            // update buttons
            CreateButton.SetActive(true);
            RandomizeButton.SetActive(false);
            SaveButton.SetActive(false);
            DeleteButton.SetActive(false);
            OpenFileButton.SetActive(false);

            // update the texts
            profileText.text = "EMPTY";
            progressText.text = "";
            positionText.text = "";
        }
        public void Save()
        {
            // this is how you save all the data into a local file.
            // if you don't call this function all the game data will be lost.
            // make sure to make a check points to save data, also call it OnApplicationQuit.
            data.SaveProfile();

            // update buttons
            OpenFileButton.SetActive(true);
            DeleteButton.SetActive(true);
        }
        public void Open()
        {
            data.OpenProfileLocation();
        }
    }
}
