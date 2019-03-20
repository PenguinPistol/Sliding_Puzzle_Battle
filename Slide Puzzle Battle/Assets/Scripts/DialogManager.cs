using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.PlugStudio.Patterns;

public class DialogManager : Singleton<DialogManager>
{
    private Dictionary<string, Dialog> dialogs;

    public List<DictionaryItem> items = new List<DictionaryItem>();
    public Canvas canvas;
    public int size = 0;

    private void Start()
    {
        dialogs = new Dictionary<string, Dialog>();
        
        for (int i = 0; i < items.Count; i++)
        {
            var dialog = Instantiate(items[i].value, canvas.transform);
            dialog.gameObject.SetActive(false);
            dialogs.Add(items[i].key, dialog);
        }
    }

    public void ShowDialog(string _name)
    {
        if(dialogs.ContainsKey(_name) == false)
        {
            return;
        }

        dialogs[_name].gameObject.SetActive(true);
    }

}
