using System.Collections;
using UnityEngine;
using CodeBase.Infrastructure.DataProvider;
using CodeBase.Infrastructure.SceneLoad;
using VContainer;

namespace CodeBase.Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private SceneCatalog _scenes;
        private ISceneLoader _sceneLoader;
        private ILoadingScreen _loading;
        private IDataProvider _dataProvider;
        private IPersistentData _persistentData;

        [Inject]
        public void Construct(
            SceneCatalog scenes,
            ISceneLoader sceneLoader,
            ILoadingScreen loading,
            IDataProvider dataProvider,
            IPersistentData persistentData)
        {
            _scenes = scenes;
            _sceneLoader = sceneLoader;
            _loading = loading;
            _dataProvider = dataProvider;
            _persistentData = persistentData;
        }

        public void Run()
        {
            StartCoroutine(Bootstrap());
        }


        private IEnumerator Bootstrap()
        {
            _loading.Show();
            _loading.SetProgress(0f);

            LoadDataOrInit();
            _loading.SetProgress(0.2f);
            yield return null;

            _loading?.SetProgress(0.2f);
            _sceneLoader.Load(_scenes.Gameplay, _loading);
        }

        
        private void LoadDataOrInit()
        {
            if (_dataProvider.TryLoad() == false)
                _persistentData.PlayerData = new Data.PlayerData();
        }
    }
}