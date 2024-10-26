using UnityEngine;
using UnityEngine.SceneManagement;

public class ThisBoxCollision : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
        // Check if the DialogueTrigger component exists
        if (dialogueTrigger == null)
        {
            Debug.LogError("DialogueTrigger component is missing from this object.");
        }
    }

    // This method is called when another collider makes contact with this object's collider
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Check if the DialogueTrigger component exists before calling the method
        Debug.Log("Dialogue should be triggered!");
        dialogueTrigger.TriggerDialogue();
    }
}
