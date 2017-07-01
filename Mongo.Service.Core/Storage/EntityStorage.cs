﻿using System;
using System.Linq.Expressions;
using Mongo.Service.Core.Storable.Base;

namespace Mongo.Service.Core.Storage
{
    public class EntityStorage<TEntity> : IEntityStorage<TEntity> where TEntity : IBaseEntity
    {
        public TEntity Read(Guid id)
        {
            throw new NotImplementedException();
        }

        public TEntity[] Read(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public TEntity[] Read(int skip, int limit)
        {
            throw new NotImplementedException();
        }

        public TEntity[] Read(Expression<Func<TEntity, bool>> filter, int skip, int limit)
        {
            throw new NotImplementedException();
        }

        public bool TryRead(Guid id, out TEntity apiEntity)
        {
            throw new NotImplementedException();
        }

        public TEntity[] ReadAll()
        {
            throw new NotImplementedException();
        }

        public Guid[] ReadIds(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public long ReadSyncedData(long lastSync, out TEntity[] newData, out TEntity[] deletedData,
            Expression<Func<TEntity, bool>> additionalFilter = null)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Write(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Write(TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            throw new NotImplementedException();
        }

        public long Count(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public long GetLastTick()
        {
            throw new NotImplementedException();
        }
    }
}