using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartScene
{
    public class StartSceneController : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadScene("StartScene");
        }

        void SwitchScene()
        {
            SceneManager.LoadScene("PuzzleGame");
        }
    }
}