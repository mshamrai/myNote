﻿using myNote.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer.Sql
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string connectionString;
        private readonly IGroupsRepository groupsRepository;

        public UsersRepository(string connectionString, IGroupsRepository groupsRepository)
        {
            this.connectionString = connectionString;
            this.groupsRepository = groupsRepository;
        }

        public User CreateUser(User user)
        {
            var db = new DataContext(connectionString);
            user.Id = Guid.NewGuid();
            db.GetTable<User>().InsertOnSubmit(user);
            db.SubmitChanges();
            return user;
        }

        public void DeleteUser(Guid id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "delete from Users where Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public User GetUser(Guid id)
        {
            var db = new DataContext(connectionString);

            var user = (from u in db.GetTable<User>()
                        where u.Id == id
                        select u).FirstOrDefault();
            if (user == default(User))            
                throw new ArgumentException($"Пользователь с id {id} не найден");
            user.UserGroups = groupsRepository.GetUserGroups(user.Id);
            return user;
        }
    }
}
