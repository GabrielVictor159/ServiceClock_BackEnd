﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Infraestructure.Data;

namespace ServiceClock_BackEnd.Infraestructure.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    public T? GetById(string id)
    {
        using var context = new Context();
        return context.Set<T>().Find(id);
    }

    public IEnumerable<T> GetAll()
    {
        using var context = new Context();
        return context.Set<T>().ToList();
    }

    public void Add(T entity)
    {
        using var context = new Context();
        context.Set<T>().Add(entity);
        context.SaveChanges();
    }

    public void Update(T entity)
    {
        using var context = new Context();
        context.Set<T>().Update(entity);
        context.SaveChanges();
    }

    public void Delete(string id)
    {
        using var context = new Context();
        var entity = context.Set<T>().Find(id);
        if (entity != null)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        using var context = new Context();
        return context.Set<T>().Where(predicate).ToList();
    }

    public T? FindSingle(Expression<Func<T, bool>> predicate)
    {
        using var context = new Context();
        return context.Set<T>().SingleOrDefault(predicate);
    }

    public int Count(Expression<Func<T, bool>> predicate)
    {
        using var context = new Context();
        return context.Set<T>().Count(predicate);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        using var context = new Context();
        context.Set<T>().AddRange(entities);
        context.SaveChanges();
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        using var context = new Context();
        context.Set<T>().UpdateRange(entities);
        context.SaveChanges();
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        using var context = new Context();
        context.Set<T>().RemoveRange(entities);
        context.SaveChanges();
    }
}


