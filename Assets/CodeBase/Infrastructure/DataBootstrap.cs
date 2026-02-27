using CodeBase.Data;
using CodeBase.Infrastructure.DataProvider;

namespace CodeBase.Infrastructure
{
    public class DataBootstrap : IDataBootstrap
    {
        private readonly IDataProvider _dataProvider;
        private readonly IPersistentData _persistentData;

        public DataBootstrap(IDataProvider dataProvider, IPersistentData persistentData)
        {
            _persistentData = persistentData;
            _dataProvider = dataProvider;
        }

        public void Initialize()
        {
            // 1) загрузка

            // 2) дефолты (гарантируем валидность)
            EnsureDefaults();

            // 3) миграции/валидация (по необходимости)
            // MigrateIfNeeded();
            // Validate();
        }

        private void EnsureDefaults()
        {
            if (_dataProvider.TryLoad() == false)
                _persistentData.PlayerData = new PlayerData();
        }
    }

}