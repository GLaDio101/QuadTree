/*using System.Collections.Generic;
using Assets.Scripts.Service.Authentication;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Assets.Scripts.Service.Save.Imp
{
    public class GoogleCloudSaveService : AbstractSaveService
    {
        private string _loadedFilename;

        private int _saveId = 1;

        private PlayGamesPlatform _service;

        private ISavedGameMetadata _metadata;

        private bool _debugMode;

        [Inject(ServiceType.GooglePlay)]
        public IAuthenticationService authService { get; set; }

        private void Log(string message)
        {
            if (_debugMode) Debug.Log(message);
        }

        [PostConstruct]
        public void OnPostConstruct()
        {
            _debugMode = false;

            Log("GoogleCloudSaveService.PostConstruct");

            dispatcher.AddListener(AuthenticationEvent.LoggedIn, OnLoggedIn);
            dispatcher.AddListener(AuthenticationEvent.Logout, OnLogout);
        }

        private void OnLogout(IEvent payload)
        {
            if ((ServiceType)payload.data != ServiceType.GooglePlay)
                return;

            _loadedFilename = string.Empty;
            _metadata = null;
            dispatcher.AddListener(AuthenticationEvent.LoggedIn, OnLoggedIn);
        }

        public sealed override void LoadData()
        {
            if (!authService.Connected)
            {
                return;
            }

            Log("GoogleCloudSaveService.LoadData");

            _service.SavedGame.FetchAllSavedGames(DataSource.ReadCacheOrNetwork, OnSavedGamesLoaded);
        }

        private void OnLoggedIn(IEvent payload)
        {
            if ((ServiceType)payload.data != ServiceType.GooglePlay)
                return;

            _service = PlayGamesPlatform.Instance;

            Log("GoogleCloudSaveService.OnLoggedIn");

            dispatcher.RemoveListener(AuthenticationEvent.LoggedIn, OnLoggedIn);
            
            LoadData();
        }

        private void OnSavedGamesLoaded(SavedGameRequestStatus status, List<ISavedGameMetadata> metadatas)
        {
            Log("GoogleCloudSaveService.OnSavedGamesLoaded");
            if (status != SavedGameRequestStatus.Success)
                return;

            if (metadatas.Count <= 0)
            {
                _service.SavedGame.OpenWithAutomaticConflictResolution("snapshot_" + _saveId, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseUnmerged, OnSaveLoaded);
                return;
            }


            _saveId = metadatas.Count;

            // select last modified
            ISavedGameMetadata metadata = metadatas[0];
            for (int i = 1; i < metadatas.Count; i++)
            {
                if (metadata.LastModifiedTimestamp < metadatas[i].LastModifiedTimestamp)
                    metadata = metadatas[i];
            }

            _service.SavedGame.OpenWithAutomaticConflictResolution(metadata.Filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseUnmerged, OnSaveLoaded);
        }

        private void OnSaveLoaded(SavedGameRequestStatus status, ISavedGameMetadata metadata)
        {
            Log("GoogleCloudSaveService.OnSaveLoaded");
            if (metadata.Filename == _loadedFilename)
            {
                Debug.LogWarning("################# > Already Loaded " + metadata.Filename + " - " + _loadedFilename);
                return;
            }

            if (status != SavedGameRequestStatus.Success)
                return;

            _metadata = metadata;
            _service.SavedGame.ReadBinaryData(metadata, OnSaveDataLoaded);
        }

        private void OnSaveDataLoaded(SavedGameRequestStatus status, byte[] data)
        {
            Log("GoogleCloudSaveService.OnSaveDataLoaded");
            if (status != SavedGameRequestStatus.Success)
                return;

            _loadedFilename = _metadata.Filename;
            _tempData = data;
            dispatcher.Dispatch(SaveEvent.DataReady);
        }

        public sealed override void SaveGame(object savedData)
        {
            Log("GoogleCloudSaveService.SaveGame");
            if (!authService.Connected)
                return;

            if (_metadata != null && _metadata.IsOpen)
            {
                _service.SavedGame.CommitUpdate(_metadata, new SavedGameMetadataUpdate(), ToByteArray(savedData), OnSaveCompleted);
            }
            else
            {
                _service.SavedGame.OpenWithAutomaticConflictResolution(_metadata.Filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseUnmerged,
                    (status, metadata) =>
                    {
                        if (status != SavedGameRequestStatus.Success)
                            return;

                        _metadata = metadata;
                        if (metadata.IsOpen)
                        {
                            _service.SavedGame.CommitUpdate(_metadata, new SavedGameMetadataUpdate(),
                                ToByteArray(savedData), OnSaveCompleted);
                        }
                    });
            }
        }

        private void OnSaveCompleted(SavedGameRequestStatus status, ISavedGameMetadata metadata)
        {
            Log("GoogleCloudSaveService.OnSaveCompleted");
            if (status != SavedGameRequestStatus.Success)
                return;

            _metadata = metadata;
            dispatcher.Dispatch(SaveEvent.Saved);
        }

        public sealed override void SaveGame(object savedData, Texture2D icon)
        {
            SaveGame(savedData);
        }

        public new string Filename
        {
            get { return _loadedFilename; }
        }

        public sealed override void Clear()
        {
            Log("GoogleCloudSaveService.Clear");
            if (!authService.Connected)
                return;

            _service.SavedGame.FetchAllSavedGames(DataSource.ReadCacheOrNetwork, OnSavedGamesLoadedForClear);
        }

        private void OnSavedGamesLoadedForClear(SavedGameRequestStatus status, List<ISavedGameMetadata> metadatas)
        {
            Log("GoogleCloudSaveService.OnSavedGamesLoadedForClear");
            if (status != SavedGameRequestStatus.Success)
                return;

            if (metadatas.Count == 0)
                return;

            if (metadatas.Count > 1)
            {
                Debug.LogWarning("Exessive save meta data: " + metadatas.Count);
            }
            if (metadatas[0].IsOpen)
            {
                _service.SavedGame.Delete(metadatas[0]);
                dispatcher.Dispatch(SaveEvent.Cleared);
            }
            else
            {
                _service.SavedGame.OpenWithAutomaticConflictResolution(metadatas[0].Filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseUnmerged,
                (stat, metadata) =>
                {
                    if (stat != SavedGameRequestStatus.Success)
                        return;

                    if (metadata.IsOpen)
                    {
                        _service.SavedGame.Delete(metadata);
                        dispatcher.Dispatch(SaveEvent.Cleared);
                    }
                });
            }
        }

        public sealed override void ShowSaveManager()
        {
            if (!authService.Connected)
                return;

            _service.SavedGame.ShowSelectSavedGameUI(Application.productName, 5, true, true, OnSavePanelOpened);
        }

        private void OnSavePanelOpened(SelectUIStatus status, ISavedGameMetadata metadata)
        {
            Log("GoogleCloudSaveService.OnSavePanelOpened");
            if (status != SelectUIStatus.SavedGameSelected)
                return;

            if (metadata.Filename == _loadedFilename)
            {
                Debug.LogWarning("################# > Already Loaded " + metadata.Filename + " - " + _loadedFilename);
                return;
            }

            //_loadedFilename = metadata.Filename;
            //_service.SavedGame.ReadBinaryData(metadata, OnSaveDataLoaded);
        }
    }
}*/