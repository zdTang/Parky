﻿namespace ParkyWeb.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string url, int Id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<bool> CreateAsync(string url, T objectToCreate);
        Task<bool> UpdateAsync(string url, T objectToUpdate);
        Task<bool> DeleteAsync(string url, int Id);
    }
}
