using Microsoft.AspNetCore.Identity;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Blog.Domain;

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{ 
}