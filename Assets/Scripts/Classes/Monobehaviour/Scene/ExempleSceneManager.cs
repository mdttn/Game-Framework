namespace RedSilver2.Framework.Scenes
{
    public sealed class ExempleSceneManager : SceneLoaderManager
    {
        protected override void OnValidate()
        {
           
        }

        protected sealed override void Start()
        {
            AddSceneData(new SceneData[]
            {
                new TestScene01("...", 1, true)
            });
        }
    }
}
