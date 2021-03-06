﻿using System;
using LifebyteMVC.Core.Interfaces;
using NHibernate;

namespace LifebyteMVC.Data.Repositories
{
    /// <summary>
    /// The base respository is responsible for managing the sessions, 
    /// and common methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Respository<T> where T : ICoreEntity
    {
        private ISession session;
        public ISession Session
        {
            get
            {
                return session;
            }
        }

        public Respository()
        {
            session = SessionManager.SessionFactory.OpenSession();
        }

        public Respository(ISession session)
        {
            this.session = session;
        }

        public T Get(Guid id)
        {
            using (session)
            {
                using (var transaction = session.BeginTransaction())
                {
                    return session.Get<T>(id);
                }
            }
        }

        /// <summary>
        /// This only inserts a record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Save(T entity)
        {
            using (session)
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(entity);
                        transaction.Commit();
                    }
                    catch
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }

                        throw;
                    }

                    return entity;
                }
            }
        }

        /// <summary>
        /// This only updates a record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Update(T entity)
        {
            using (session)
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Update(entity);
                        transaction.Commit();
                    }
                    catch
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
                        }

                        throw;
                    }

                    return entity;
                }
            }
        }
    }
}
