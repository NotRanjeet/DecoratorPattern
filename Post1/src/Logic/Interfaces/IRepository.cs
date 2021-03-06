﻿using Logic.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interfaces
{
    public interface IRepository
    {
        IQueryable<T> Get<T>() where T : BaseEntity;

        T GetById<T>(int id) where T : BaseEntity;
        List<T> List<T>() where T : BaseEntity;
        T Add<T>(T entity) where T : BaseEntity;
        void Update<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
    }
}
