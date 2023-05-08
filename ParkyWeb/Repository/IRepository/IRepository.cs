﻿namespace ParkyWeb.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetAsync(string url, int Id, string token);

        Task<IEnumerable<T>?> GetAllAsync(string url, string token);

        Task<bool> CreateAsync(string url, T objectToCreate, string token);

        Task<bool> UpdateAsync(string url, T objectToUpdate, string token);

        Task<bool> DeleteAsync(string url, int Id, string token);
    }
}