using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button button;
    
     void Awake()
     {
        button.onClick.AddListener(LoadScene);
     }
    // Update is called once per frame
    private void LoadScene()
    {
        SceneManager.LoadScene("Game");
    }
}
