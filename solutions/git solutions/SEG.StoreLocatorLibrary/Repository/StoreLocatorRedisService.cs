using System;
using System.Collections.Generic;
//using System.Text.Json;
using SEG.StoreLocatorLibrary.Shared.ResponseModels;
using Newtonsoft.Json;


namespace SEG.StoreLocatorLibrary.Repository
{
    //public class StoreLocatorRedisService<T> 
    //{
    //    public string Key { get; }
    //    //RedisConnection _connection;
    //    public StoreLocatorRedisService(RedisConnection connection, object request)
    //    {
    //        Key = JsonConvert.SerializeObject(request);
    //        //_connection = connection;
    //    }

    //    public IList<T> GetRecordsOrDefault() => GetRecordsOrDefault(Key);

    //    public IList<T> GetRecordsOrDefault(string key)
    //    {
    //        if (string.IsNullOrEmpty(key))
    //            throw new Exception("Key cannot be null");

    //        var value = _connection.GetValue(key);
    //        if (string.IsNullOrEmpty(value))
    //            return null;

    //        return JsonConvert.DeserializeObject<List<T>>(value);
    //    }


    //    public void SaveRecords(IList<T> stores) => SaveRecords(Key, stores);

    //    private void SaveRecords(string key, object stores)
    //    {
    //        if (string.IsNullOrEmpty(key) || stores == null)
    //            throw new Exception("Key and stores must be not null");

    //        var value = JsonConvert.SerializeObject(stores);
    //        _connection.SetValue(key, value);
    //    }

    //}
}
