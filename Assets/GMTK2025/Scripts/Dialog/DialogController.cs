using System.Collections;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dialogText;

    // ...
    private string dialog;

    private float typeTime;

    public void StartDialog(string dialog, float typeTime)
    {
        this.dialog = dialog;
        this.typeTime = typeTime;

        StopAllCoroutines();
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(TypeDialogLine());
        }
    }

    private IEnumerator TypeDialogLine()
    {
        dialogText.text = dialog;
        dialogText.maxVisibleCharacters = 0;

        for (int i = 0; i < dialogText.text.Length; i++)
        {
            dialogText.maxVisibleCharacters++;
            yield return new WaitForSeconds(typeTime);
        }
    }
}
