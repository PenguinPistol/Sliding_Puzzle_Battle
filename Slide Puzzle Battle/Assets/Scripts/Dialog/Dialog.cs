using UnityEngine;
using UnityEngine.UI;

public abstract class Dialog : MonoBehaviour
{
    public Button positiveButton;
    public Button negativeButton;
    public Button neatralButton;

    private void Start()
    {
        if (positiveButton != null)
        {
            positiveButton.onClick.AddListener(PositiveAction);
        }

        if (negativeButton != null)
        {
            negativeButton.onClick.AddListener(NegativeAction);
        }

        if (neatralButton != null)
        {
            neatralButton.onClick.AddListener(NeatralAction);
        }
    }

    public virtual void PositiveAction()
    {}

    public virtual void NeatralAction()
    {}

    public virtual void NegativeAction()
    {
        gameObject.SetActive(false);
    }
}
