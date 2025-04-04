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
    public T? GetById(dynamic id)
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

    public void Delete(T entity)
    {
        using var context = new Context();
        context.Attach(entity);
        context.Set<T>().Remove(entity);
        context.SaveChanges();
    }


    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        using var context = new Context();
        return context.Set<T>().Where(predicate).ToList();
    }
    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
    {
        using var context = new Context();

        return context.Set<T>()
            .Where(predicate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
    public IEnumerable<T> FindContainIncludes(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes)
    {
        using var context = new Context();

        var query = context.Set<T>().Where(predicate);

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
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

    public int UpdateRange(IEnumerable<T> entities)
    {
        using var context = new Context();
        context.Set<T>().UpdateRange(entities);
        return context.SaveChanges();
    }

    public int DeleteRange(IEnumerable<T> entities)
    {
        using var context = new Context();
        context.Set<T>().RemoveRange(entities);
        return context.SaveChanges();
    }
    public int Update(T entity)
    {
        using var context = new Context();

        var idProperty = typeof(T).GetProperty("Id");
        if (idProperty == null)
        {
            throw new Exception("Entity does not have an Id property.");
        }

        var idValue = idProperty.GetValue(entity);
        var existingEntity = context.Set<T>().Find(idValue);
        if (existingEntity == null)
        {
            throw new Exception("Entity not found.");
        }

        context.Entry(existingEntity).CurrentValues.SetValues(entity);
        return context.SaveChanges();
    }

    public int Save()
    {
        using var context = new Context();
        return context.SaveChanges();
    }
}


