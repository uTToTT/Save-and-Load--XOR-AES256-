using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLocalProvider : IDataProvider
{
    private const string SAVE_FILE_PREFIX = "PlayerSave_";
    private const string ERROR_FILE_PREFIX = "error";
    private const string SAVE_FILE_EXTENSION = ".json";
    private const string ERROR_FILE_EXTENSION = ".txt";

    private IPersistentData _persistentData;
    private ICipherProvider _cipherProvider;

    public DataLocalProvider(
        IPersistentData persistentData,
        ICipherProvider cipherProvider)
    {
        _persistentData = persistentData;
        _cipherProvider = cipherProvider;
    }

    private string SavePath => Application.persistentDataPath;
    private string ErrorLogPath => Path.Combine(SavePath, $"{ERROR_FILE_PREFIX}{ERROR_FILE_EXTENSION}");
    private string GetFullPath(int slotId) => Path.Combine(SavePath, $"{SAVE_FILE_PREFIX}{slotId}{SAVE_FILE_EXTENSION}");

    public bool TryLoad(int slotId)
    {
        try
        {
            string path = GetFullPath(slotId);

            if (IsAlreadyExists(slotId) == false)
                return false;

            var cryptData = File.ReadAllText(path);
            var data = _cipherProvider.Decrypt(cryptData);
            var playerData = JsonConvert.DeserializeObject<PlayerData>(data);

            _persistentData.PlayerData = playerData;

            Debug.Log($"Loaded [{path}]");

            return true;
        }
        catch (Exception ex)
        {
            WriteErrorLog(ex);
            throw;
        }
    }

    public void Save(int slotId)
    {
        try
        {
            var path = GetFullPath(slotId);
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(_persistentData.PlayerData, Formatting.Indented, jsonSettings);
            var cryptData = _cipherProvider.Encrypt(json);
            File.WriteAllText(path, cryptData);

            Debug.Log($"Saved [{path}]");
        }
        catch (Exception ex)
        {
            WriteErrorLog(ex);
            throw;
        }
    }

    public void Delete(int slotId)
    {
        try
        {
            var path = GetFullPath(slotId);
            File.Delete(path);

            Debug.Log($"Deleted [{path}]");
        }
        catch (Exception ex)
        {
            WriteErrorLog(ex);
            throw;
        }

    }

    public IReadOnlyCollection<SavesSlotInfo> GetSavesSlotInfos()
    {
        try
        {
            var slots = new List<SavesSlotInfo>();

            foreach (var file in Directory.GetFiles(SavePath, $"{SAVE_FILE_PREFIX}*{SAVE_FILE_EXTENSION}"))
            {
                int slotId = ParseSlotId(file);
                var info = new FileInfo(file);

                slots.Add(new SavesSlotInfo
                {
                    SlotId = slotId,
                    LastModified = info.LastWriteTime,
                    Size = info.Length
                });
            }

            return slots;
        }
        catch (Exception ex)
        {
            WriteErrorLog(ex);
            throw;
        }
    }

    private int ParseSlotId(string path)
    {
        string fileName = Path.GetFileNameWithoutExtension(path);
        string idPart = fileName.Replace(SAVE_FILE_PREFIX, "");
        return int.TryParse(idPart, out var id) ? id : -1;
    }

    private bool IsAlreadyExists(int slotId) => File.Exists(GetFullPath(slotId));

    private void WriteErrorLog(Exception ex)
    {
        Debug.LogException(ex);
        File.AppendAllText(ErrorLogPath, $"[{DateTime.Now}] {ex}\n\n");
    }
}
