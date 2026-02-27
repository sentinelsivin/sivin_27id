using CodeBase.Infrastructure.DataProvider;
using CodeBase.Services;
using CodeBase.Services.StateMachine;
using UnityEngine;
using VContainer;

namespace CodeBase.Infrastructure
{
    public class BootstrapComponents : MonoBehaviour
    {
        [SerializeField] private MainSceneMode _mainSceneMode;
        [SerializeField] private GameCoordinator _gameCoordinator;

        private IDataProvider _dataProvider;
        private IPersistentData _persistentData;
        
        [Inject]
        public void Construct(IDataProvider dataProvider, IPersistentData persistentData)
        {
            _dataProvider = dataProvider;
            _persistentData = persistentData;
        }
        
        public void Run()
        {
            _mainSceneMode.Bootstrap(); 
            
            Debug.Log($"Starting {_dataProvider}");
            Debug.Log($"Starting {_persistentData}");
        }
        
    }
    
   
}
