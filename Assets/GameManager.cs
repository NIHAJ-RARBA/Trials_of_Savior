using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int deathCount = 0;

    private void Awake()
    {
        // Ensure this GameObject persists between scenes
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager created!");
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddDeath()
    {
        deathCount++;
        Debug.Log("Total Deaths: " + deathCount);
    }

}
