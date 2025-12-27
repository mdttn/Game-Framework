using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        [SerializeField] private GameObject root;

        [Space]
        [SerializeField][Range(0.1f, 0.5f)] private float loggedMessageDelay;        
        [SerializeField][Range(10, 1000)]   private uint  maxLoggedMessages;

        private LogMessageType currentMessagesType;

        private readonly static List<DevConsoleCommand> commands = new List<DevConsoleCommand>();
        private readonly static Dictionary<LogMessageType, List<LoggedMessageData>> messages = GetLoggedMessagesDictonnary();
        private          static DevConsole instance;

        protected virtual async void Awake()
        {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(instance);
            }
            else {
                Destroy(gameObject);
                return;
            }

            if (root != null) root.SetActive(false);
            gameObject.name = "Dev Console";
            currentMessagesType = LogMessageType.All;

            new TeleportCommand();
            new LogCommand();
        }

        protected abstract void Start();

        private async void Update(){
            UpdateInput();
        }

        private async void UpdateInput()
        {
            if (InputManager.GetKeyDown(KeyboardKey.Backquote) && root != null) {
                bool isActivated = !root.activeInHierarchy;
                root.SetActive(isActivated);

                if (isActivated) PlayerController.Disable();
                else             PlayerController.Enable();

                if (isActivated) CameraControllerModule.Disable();
                else             CameraControllerModule.Enable();

                await UpdateLoggedMessages();
            }
        }

        protected void SetCurrentMessagesType(LogMessageType messageType)
        {
            currentMessagesType = messageType;
        }

        private async Awaitable UpdateLoggedMessages() {
            float updateDelay = 0;
            bool canTriggerOnce = true;

            while (true) {

                if (root == null || !root.activeInHierarchy) break;

                if(updateDelay > loggedMessageDelay || canTriggerOnce)
                {
                    await Awaitable.BackgroundThreadAsync();
                    canTriggerOnce = false;

                    foreach (LogMessageType type in Enum.GetValues(typeof(LogMessageType))) {
                        await RemoveLoggedMessage(type);
                    }

                    await Awaitable.MainThreadAsync();

                    ShowLoggedMessages();
                    updateDelay = 0f;
                }

                
                float deltaTime = Time.deltaTime;
                await Awaitable.BackgroundThreadAsync();

                updateDelay += deltaTime;
                await Awaitable.MainThreadAsync();

                await Awaitable.NextFrameAsync();
            }
        }

        protected void ShowLoggedMessages()
        {
            if (messages.ContainsKey(currentMessagesType)) {
                List<string>            results = new List<string>();
                List<LoggedMessageData> datas   = messages[currentMessagesType];

                foreach (LoggedMessageData data in datas)
                    results.Add(data.text);

                UpdateDisplayerText(results.ToArray());
            }
        }

        protected void OnInputFieldValueChanged(string content) {

        }

        protected abstract void UpdateDisplayerText(string[] messages); 
       

        public static void Enable()
        {
            if (instance != null) instance.enabled = true;  
        }

        public static void Disable()
        {
            if (instance != null) instance.enabled = false;
        }


        public static void Log(string message, bool useUnityLog)
        {
            Log(message);
            if (useUnityLog) Debug.Log(message);
        }

        public static void Log(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            message = $"<color=white>[{DateTime.Now.ToString()}] {message}</color>";
            AddLoggedMessage    (LogMessageType.Log, message);
        }


        public static void LogWarning(string message, bool useUnityLog)
        {
            LogWarning(message);
            if (useUnityLog) Debug.LogWarning(message);
        }

        public static void LogWarning(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            message = $"<color=yellow>[{DateTime.Now.ToString()}] {message}</color>";
            AddLoggedMessage(LogMessageType.Warning, message);
        }

        public static void LogError(string message, bool useUnityLog)
        {
            LogError(message);
            if(useUnityLog) Debug.LogError(message);
        }

        public static void LogError(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            message = $"<color=red>[{DateTime.Now.ToString()}] {message}</color>";
            AddLoggedMessage(LogMessageType.Error, message);
        }

        private static Dictionary<LogMessageType, List<LoggedMessageData>> GetLoggedMessagesDictonnary() {
             Dictionary<LogMessageType, List<LoggedMessageData>> result = new Dictionary<LogMessageType, List<LoggedMessageData>>();

            foreach(LogMessageType type in Enum.GetValues(typeof(LogMessageType)))
                result.Add(type, new List<LoggedMessageData>());

            return result;
        }

        private static void AddLoggedMessage(LogMessageType messageType, string message)
        {
            if (string.IsNullOrEmpty(message) || messageType == LogMessageType.All) return;


            LoggedMessageData data = CreateAndGetLoggedMessageData(message);
            messages[LogMessageType.All].Add(data);
            messages[messageType].Add(data);
        }

        private async static Awaitable RemoveLoggedMessage(LogMessageType messageType)
        {
            if (messageType == LogMessageType.All) return;
            await Awaitable.BackgroundThreadAsync();

            int count = (int)(instance.maxLoggedMessages / (messages.Keys.Count - 1));

            while (messages[messageType].Count > count) {

                if (messages[LogMessageType.All] == null || messages[messageType] == null) return;

                if (messages[LogMessageType.All].Count > 0 && messages[messageType].Count > 0) {
                    
                    if(messages[messageType][0].index > 0)
                        messages[LogMessageType.All].RemoveAt(messages[messageType][0].index);
                    
                    messages[messageType].RemoveAt(0);
                }

                await UpdateRemovedLoggedMessagesIndexes();
                await Task.Yield();
            }

            await Awaitable.MainThreadAsync();
        }


        private static async Awaitable UpdateRemovedLoggedMessagesIndexes()
        {
            await Awaitable.BackgroundThreadAsync();

            foreach (LogMessageType type in Enum.GetValues(typeof(LogMessageType))) {
                List<LoggedMessageData> current = messages[type];
                if (current.Count <= 0) continue;

                for (int i = 0; i < current.Count; i++)
                    messages[type][i] = GetUpdateRemovedIndexData(current[i]);
            }

            await Awaitable.MainThreadAsync();
        }

        private static LoggedMessageData GetUpdateRemovedIndexData(LoggedMessageData data)
        {
            data.index--;
            return data;
        }

        private static LoggedMessageData CreateAndGetLoggedMessageData(string message)
        {
            LoggedMessageData data = new LoggedMessageData();
            data.text = message;
            data.index = messages[LogMessageType.All].Count;
            return data;
        }

        protected static void SetDropdown(UnityEngine.UI.Dropdown dropdown)
        {
            if (dropdown != null)
            {
                dropdown.ClearOptions();
                dropdown.onValueChanged.RemoveAllListeners();

                foreach (LogMessageType messageType in Enum.GetValues(typeof(LogMessageType)))
                    dropdown.options.Add(new UnityEngine.UI.Dropdown.OptionData(messageType.ToString()));

                dropdown.onValueChanged.AddListener(index => {
                    if (instance != null) instance.SetCurrentMessagesType((LogMessageType)index);
                });
            }
        }
        protected static void SetDropdown(TMP_Dropdown dropdown)
        {
            if (dropdown == null) return;

            dropdown.ClearOptions();
            dropdown.onValueChanged.RemoveAllListeners();

            foreach (LogMessageType messageType in Enum.GetValues(typeof(LogMessageType)))
                dropdown.options.Add(new TMP_Dropdown.OptionData(messageType.ToString()));

            dropdown.onValueChanged.AddListener(index => {
                if (instance != null) instance.SetCurrentMessagesType((LogMessageType)index);
            });
        }

        protected static void SetInputField(UnityEngine.UI.Text displayer, UnityEngine.UI.InputField inputField)
        {
            if (inputField == null || displayer == null) return;

            inputField.text = string.Empty;
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onSubmit.RemoveAllListeners();

            inputField.onValueChanged.AddListener(async context =>
            {
                if (displayer == null) return;
                displayer.text = string.Empty;

                if (string.IsNullOrEmpty(context)) {
                    foreach (DevConsoleCommand command in commands)
                        foreach (string preview in await command.GetActionPreviews())
                            displayer.text += $"{preview}\n";
                }
                else {
                    foreach (DevConsoleCommand command in await GetValidCommands(context))
                        foreach (string preview in await command.GetActionPreviews(context))
                                 displayer.text += $"{preview}\n";
                }
            });

            inputField.onSubmit.AddListener(async context =>
            {
                var results = await GetValidCommands(context);
                if (results.Count() > 0) results.First().Execute(context);
                inputField.text = string.Empty;
            });
        }
        protected static async void SetInputField(TextMeshProUGUI displayer, TMP_InputField inputField)
        {
            if (inputField == null || displayer == null) return;

            inputField.text = string.Empty;
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onSubmit.RemoveAllListeners();

            inputField.onValueChanged.AddListener(async context =>
            {
                if (displayer == null) return;
                displayer.text = string.Empty;

                if (string.IsNullOrEmpty(context))
                {
                    foreach (DevConsoleCommand command in commands)
                        foreach (string preview in await command.GetActionPreviews())
                            displayer.text += $"{preview}\n";
                }
                else
                {

                    foreach (DevConsoleCommand command in await GetValidCommands(context))
                        foreach (string preview in await command.GetActionPreviews(context))
                            displayer.text += $"{preview}\n";
                }
            });

            inputField.onSubmit.AddListener(async context =>
            {
                var results = await GetValidCommands(context);
                if (results.Count() > 0) results.First().Execute(context);
                inputField.text = string.Empty;
            });
        }

        private static async Awaitable<DevConsoleCommand[]> GetValidCommands(string context)
        {
            List<DevConsoleCommand> results  = new List<DevConsoleCommand>();
            if (commands == null || string.IsNullOrEmpty(context) || commands.Count == 0) return commands.ToArray();

            await Awaitable.BackgroundThreadAsync();

            foreach(DevConsoleCommand command in commands.Where(x => x != null)){
                if(await command.IsValid(context)) 
                    results.Add(command);
            }

            await Awaitable.MainThreadAsync();
            return results.ToArray();
        }
    }


    [System.Serializable]
    public struct LoggedMessageData
    {
        public string text;
        public int index;
    }
}
