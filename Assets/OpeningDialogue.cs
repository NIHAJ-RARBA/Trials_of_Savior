using UnityEngine;

public class OpeningDialogue : MonoBehaviour
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
        if (GameManager.instance.deathCount != 0)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the DialogueTrigger component exists before calling the method
            Debug.Log("Dialogue should be triggered!");
            dialogueTrigger.TriggerDialogue();
        }

    }
}
