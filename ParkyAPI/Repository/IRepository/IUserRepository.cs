﻿using ParkyAPI.Models;

namespace ParkyAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        User? Authenticate(string userName, string password);
        User Register(string userName, string password);
    }
}
