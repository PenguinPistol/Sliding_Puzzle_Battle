using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.PlugStudio.Patterns;

public class DialogManager : Singleton<DialogManager>
{
    private const string DIALOG_MESSAGE = "Message";
    private const string DIALOG_BUTTONS = "Buttons";
    private const string DIALOG_POSITIVE_TEXT = "Positive Text";
    private const string DIALOG_NEGATIVE_TEXT = "Negative Text";
    private const string DIALOG_POSITIVE_BUTTON = "Positive";
    private const string DIALOG_NEGATIVE_BUTTON = "Negative";

    private Dictionary<string, GameObject> dialogs;

    public Canvas canvas;
    public GameObject dialogPrefab;

    private void Start()
    {
        dialogs = new Dictionary<string, GameObject>();
    }

    private void InitDialog(Transform _dialog, DialogData _data)
    {
        var message = _dialog.Find(DIALOG_MESSAGE).GetComponent<Text>();
        var buttons = _dialog.Find(DIALOG_BUTTONS);

        var positiveButton = buttons.Find(DIALOG_POSITIVE_BUTTON).GetComponent<Button>();
        var negativeButton = buttons.Find(DIALOG_NEGATIVE_BUTTON).GetComponent<Button>();

        var positiveText = positiveButton.transform.Find(DIALOG_POSITIVE_TEXT).GetComponent<Text>();
        var negativeText = negativeButton.transform.Find(DIALOG_NEGATIVE_TEXT).GetComponent<Text>();

        message.text = _data.Message;

        if(_data.PositiveAction != null)
        {
            positiveButton.onClick.AddListener(_data.PositiveAction);
        }
        positiveText.text = _data.PositiveText;

        if(_data.NegativeAction != null)
        {
            negativeButton.onClick.AddListener(_data.NegativeAction);
        }
        negativeText.text = _data.NegativeText;
    }

    public void AddDialog(DialogData _data, string _name)
    {
        if (dialogs.ContainsKey(_name))
        {
            return;
        }

        var dialog = Instantiate(dialogPrefab, canvas.transform);
        dialog.SetActive(false);

        InitDialog(dialog.transform, _data);

        dialogs.Add(_name, dialog);
    }

    public void ShowDialog(string _name)
    {
        if(dialogs.ContainsKey(_name) == false)
        {
            return;
        }

        dialogs[_name].SetActive(true);
    }
}
