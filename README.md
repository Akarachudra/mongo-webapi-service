# Mongo.Service.Core
Mongo.Service.Core designed for the rapid implementation your own RESTFull service based on ASP.NET WebApi and Owin Self Host.
It uses MongoDB as a data store. Timeline (reading new, deleted and changed data from client) implementation based on optimistic loop.

Some other features:
* TopShelf for run as Windows Service.
* DI Container - Simple Injector.

## How to use

Add a new ApiType (your share this with client) to Mongo.Service.Core.Types project and inherit it from ApiBase:
```c#
using System.Runtime.Serialization;
using Mongo.Service.Core.Types.Base;

namespace Mongo.Service.Core.Types
{
    // Inherited from ApiBase
    [DataContract]
    public class ApiSample : ApiBase
    {
        public string SomeData { get; set; }
    }
}
```

Add a new EntityType (it's stored in MongoDB) to Mongo.Service.Core:
```c#
using Mongo.Service.Core.Storable.Base;
using Mongo.Service.Core.Storage;

namespace Mongo.Service.Core.Storable
{
    // Set collection name. Inherit class from BaseEntity
    [CollectionName("Sample")]
    public class SampleEntity : BaseEntity
    {
        public string SomeData { get; set; }
    }
}
```

Implement converter for converting ApiType and EntityType in both ways:
```c#
using Mongo.Service.Core.Storable;
using Mongo.Service.Core.Types;

namespace Mongo.Service.Core.Services.Converters
{
    // Inherited from base generic Converter class
    public class SampleConverter : Converter<ApiSample, SampleEntity>
    {
        public override ApiSample GetApiFromStorable(SampleEntity source)
        {
            return new ApiSample
            {
                Id = source.Id,
                SomeData = source.SomeData
            };
        }

        public override SampleEntity GetStorableFromApi(ApiSample source)
        {
            return new SampleEntity
            {
                Id = source.Id,
                SomeData = source.SomeData
            };
        }
    }
}
```

Create new controller class responsible for your new ApiType:
```c#
using System;
using System.Collections.Generic;
using System.Web.Http;
using Mongo.Service.Core.Services;
using Mongo.Service.Core.Storable;
using Mongo.Service.Core.Types;

namespace Mongo.Service.Core.Controllers
{
    public class SampleController : ApiController
    {
        private readonly IEntityService<ApiSample, SampleEntity> service;

        public SampleController(IEntityService<ApiSample, SampleEntity> service)
        {
            this.service = service;
        }

        public IEnumerable<ApiSample> GetAll()
        {
            return service.ReadAll();
        }
        
        public ApiSample Get(Guid id)
        {
            return service.Read(id);
        }

        public ApiSync<ApiSample> Get(long lastSync)
        {
            ApiSample[] newData;
            Guid[] deletedIds;
            
            // Synchronize client data with ticks. Client will get only new data
            var newSync = service.ReadSyncedData(lastSync, out newData, out deletedIds);

            var apiSync = new ApiSync<ApiSample>
            {
                Data = newData,
                DeletedData = deletedIds,
                LastSync = newSync
            };
            return apiSync;
        }

        public void Post(ApiSample apiSample)
        {
            service.Write(apiSample);
        }
    }
}
```

Configure DI container at Startup class:
```c#
private static void ConfigureContainer(Container container)
{
    container.RegisterSingleton<IMongoStorage, MongoStorage>();
    container.RegisterSingleton<IMongoSettings, MongoSettings>();
    container.RegisterSingleton<IEntityStorage<SampleEntity>, EntityStorage<SampleEntity>>();
    container.RegisterSingleton<IIndexes<SampleEntity>, Indexes<SampleEntity>>();
}
```

You may need to other changes:
* Implement your custom indexes. Just create new class inherited from Indexes and override CreateIndexes method. But please, don't forget to call base method in overrided, otherwise - data synchronization will work incorrect.
* Implement your custom EntityService and override read/write methods. May be helpfull if you need write or read custom entity data.