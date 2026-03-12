using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bulletTXT;
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
    public void quit()
    {
        Application.Quit();
    }
    public void updateBulletText(int loadedBullets, int totalBullets)
    {
        bulletTXT.text = loadedBullets.ToString() + "/" + totalBullets.ToString();
    }
}
