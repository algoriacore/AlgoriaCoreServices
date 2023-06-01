using AlgoriaCore.Application.Configuration;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace AlgoriaPersistence
{
    public class MongoDBContext : IMongoDBContext
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly MongoDbOptions _dbOptions;
        private readonly IAppLocalizationProvider _appLocalizationProvider;
        private bool _isActive;

        public MongoDBContext(IOptions<MongoDbOptions> dbOptions, IAppLocalizationProvider appLocalizationProvider)
        {
            _dbOptions = dbOptions.Value;
            _appLocalizationProvider = appLocalizationProvider;

            if (_dbOptions.IsEnabled)
            {
                _client = new MongoClient(_dbOptions.ConnectionString);
                _database = _client.GetDatabase(_dbOptions.DatabaseName);

                CheckConnection();
            }
        }

        public void CheckConnection()
        {
            if (_dbOptions.IsEnabled)
            {
                try
                {
                    var res = _database.RunCommand((Command<BsonDocument>)"{ping:1}");
                    _isActive = res["ok"].AsDouble == 1;
                }
                catch (Exception)
                {
                    _isActive = false;
                }
            }
        }

        public IMongoClient Client
        {
            get
            {
                ValidateConnection();
                return _client;
            }
        }

        public IMongoDatabase Database
        {
            get
            {
                ValidateConnection();
                return _database;
            }
        }

        public bool IsEnabled => _dbOptions.IsEnabled;

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
        }

        private void ValidateConnection()
        {
            if (!_dbOptions.IsEnabled)
            {
                throw new AlgoriaCoreGeneralException(_appLocalizationProvider.L("MongoDb.ConnectionIsNotEnabled"));
            }

            if (!_isActive)
            {
                throw new AlgoriaCoreGeneralException(_appLocalizationProvider.L("MongoDb.ConnectionIsNotActive"));
            }
        }
    }
}