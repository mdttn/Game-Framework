using RedSilver2.Framework.Dev;
using System.Threading;
using UnityEngine;


namespace RedSilver2.Framework.Subtitles
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Subtitle : MonoBehaviour {

        private CanvasGroup canvasGroup;
        private CancellationTokenSource tokenSource;

        protected virtual void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();  
        }

        private void Start()
        {
            Play("The Butcher", "What's going on folks? I hope you guys are having a wonderful Time?!", 1.5f);
        }



        public async void Play(string characterName, string contextText, float duration) {
             await AwaitPlay(characterName, contextText, duration);
        }


        public async Awaitable AwaitPlay(string characterName, string contextText, float duration) {
            await UpdateSubtitle(characterName, contextText, duration, GetCancellationToken());
        }

        public void Stop()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource = null;
            }

            UpdateDisplayers(string.Empty, string.Empty);
        }


        public void Skip()
        {
            // Add something over here
        }

        protected virtual async Awaitable UpdateSubtitle(string characterName, string context, float duration, CancellationToken token)
        {
            float  timeElapsed = 0f;

            if (!string.IsNullOrEmpty(context)) {
                while (timeElapsed < duration && !token.IsCancellationRequested)  {

                    await Awaitable.BackgroundThreadAsync();
                    string characterNameResult = await GetText($"{characterName}:", Mathf.Clamp01(timeElapsed / 0.5f), SubtitleDisplayMode.Progressif);
                    string contextResult       = await GetText(context, Mathf.Clamp01(timeElapsed / duration), SubtitleDisplayMode.Progressif);
                    await Awaitable.MainThreadAsync();

                    UpdateDisplayers(characterNameResult, contextResult);

                    timeElapsed = Mathf.Clamp(Time.deltaTime + timeElapsed, 0f, duration);
                    await Awaitable.NextFrameAsync();
                }

                if (timeElapsed >= duration && !token.IsCancellationRequested) {
                    UpdateDisplayers($"{characterName}:", context);
                }
            }
        }

        protected async Awaitable<string> GetText(string text, float progress, SubtitleDisplayMode displayMode)
        {
            if      (string.IsNullOrEmpty(text))                                   return string.Empty;
            else if (displayMode == SubtitleDisplayMode.Instant || progress >= 1f) return text;
            return await GetText(text, progress);
        }

        protected virtual async Awaitable<string> GetText(string text, float progress) {            

            if (string.IsNullOrEmpty(text)) return string.Empty;
            else if(progress >= 1f)         return text;

            await Awaitable.BackgroundThreadAsync();

            char[] characters = text.ToCharArray();
            int    maxIndex   = (int)(characters.Length * Mathf.Clamp01(progress));

            string resultat = string.Empty;
            for (int i = 0; i <= maxIndex; i++)  resultat += characters[i];

            await Awaitable.MainThreadAsync();
            return resultat;
        }

        private CancellationToken GetCancellationToken() {
            if (tokenSource != null) tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            return tokenSource.Token;
        }

        protected abstract void UpdateDisplayers(string characterName, string context);
    }
}
