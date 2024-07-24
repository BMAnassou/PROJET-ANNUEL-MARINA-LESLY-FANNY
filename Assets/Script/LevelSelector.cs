using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
   public void LoadLevelPassed()
   {
    SceneManager.LoadScene(1);
   }
   public void LoadLevel1()
   {
       SceneManager.LoadScene(1);
   }
   
   public void LoadLevel2()
   {
       SceneManager.LoadScene(2);
   }
   public void LoadLevel3()
   {
       SceneManager.LoadScene(3);
   }
   
}
