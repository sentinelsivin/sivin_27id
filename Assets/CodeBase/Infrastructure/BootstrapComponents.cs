using CodeBase.Infrastructure.DataProvider;
using UnityEngine;
using VContainer;

namespace CodeBase.Infrastructure
{
    public class BootstrapComponents : MonoBehaviour
    {
        private IDataProvider _dataProvider;
        private IPersistentData _persistentData;
        
        [Inject]
        public void Construct(IDataProvider dataProvider, IPersistentData persistentData)
        {
            _dataProvider = dataProvider;
            _persistentData = persistentData;
        }
        
        private void Start()
        {
          Debug.Log($"Starting {_dataProvider}");
          Debug.Log($"Starting {_persistentData}");
        }
        
    }
    
   
}
