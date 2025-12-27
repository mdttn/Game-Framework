using System;
using TMPro;
using UnityEngine;


namespace RedSilver2.Framework.Dev
{
    public class TMPDevConsole : DevConsole
    {
        [Space]
        [SerializeField] private TextMeshProUGUI loggedMessagesDisplayer;
        [SerializeField] private TextMeshProUGUI commandsDisplayer;

        [Space]
        [SerializeField] private TMP_InputField  inputField;
        [SerializeField] private TMP_Dropdown    dropdown;

        protected override void Awake()
        {
            base.Awake();
            SetDropdown(dropdown);
            SetInputField(commandsDisplayer, inputField);
            if (loggedMessagesDisplayer != null) loggedMessagesDisplayer.text = string.Empty;

        }

        protected override void Start()
        {
           
        }



        protected sealed override void UpdateDisplayerText(string[] messages)
        {
            if (loggedMessagesDisplayer != null && messages != null)
            {
                loggedMessagesDisplayer.text = string.Empty;

                foreach (string message in messages)
                    loggedMessagesDisplayer.text += $"{message}\n";
            }
        }
    }
}
