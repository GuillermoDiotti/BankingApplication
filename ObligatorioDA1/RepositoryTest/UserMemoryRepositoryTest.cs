using System.Collections;
using Dominio;

namespace RepositoryTest;

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

[TestClass]
public class UserMemoryRepositoryTest
{
    public UserMemoryRepository repository;
    
    [TestInitialize]
    public void SetUp()
    {
        repository = new UserMemoryRepository();
    }
    
    [TestMethod]
    public void Add_AddsUserToList()
    {
        Usuario user = new Usuario
        {
            Mail = "test@example.com",
            Name = "Original Name",
            LastName = "Original Last Name",
            Address = "Original Address",
            Password = "123456789A",
        };
        Usuario addedUser = repository.Add(user);
        Assert.AreEqual(user, addedUser);
        CollectionAssert.Contains((ICollection?)repository.FindAll(), user);
    }

    [TestMethod]
    public void NoEncontradoTest()
    {
        Usuario? foundUser = repository.Find(x => x.Mail == "nonexistent@example.com");
        Assert.IsNull(foundUser);
    }

    [TestMethod]
    public void UpdateUserTest()
    {
        Usuario originalUser = new Usuario
        {
            Mail = "test@example.com",
            Name = "Original Name",
            LastName = "Original Last Name",
            Address = "Original Address",
            Password = "123456789A"
        };
        repository.Add(originalUser);

        Usuario updatedUser = new Usuario
        {
            Mail = "test@examsple.com",
            Name = "Updated Name",
            LastName = "Updated Last Name",
            Address = "Updated Address",
            Password = "123456789A"
        };
        repository.Update(updatedUser);
        
        Usuario? foundUser = repository.Find(x => x.Mail == "test@example.com");
        Assert.AreEqual(updatedUser.Name, foundUser.Name);
        Assert.AreEqual(updatedUser.LastName, foundUser.LastName);
        Assert.AreEqual(updatedUser.Address, foundUser.Address);
        Assert.AreEqual(updatedUser.Password, foundUser.Password);
    }

    [TestMethod]
    public void DeleteUserTest()
    {
        Usuario u = new Usuario
        {
            Mail = "test@example.com", 
            Name = "Juan", 
            LastName = "Carlos", 
            Password = "123456789A",
        };

        repository.Add(u);
        repository.Delete(u.Id);
        
        Assert.AreEqual(0, repository.FindAll().Count);
    }
    
    [TestMethod]
    public void DeleteUserTest2()
    {
        Usuario u = new Usuario
        {
            Mail = "test@example.com", 
            Name = "Juan", 
            LastName = "Carlos", 
            Password = "123456789A",
        };
        Usuario u2 = new Usuario
        {
            Mail = "test2@example.com", 
            Name = "Juan", 
            LastName = "Carlos", 
            Password = "123456789A",
        };
        repository.Add(u);
        repository.Delete(u2.Id);
        
        Assert.AreEqual(1, repository.FindAll().Count);
    }
    
}
