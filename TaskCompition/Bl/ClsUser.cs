using System;
using TaskCompition.Models;

namespace TaskCompition.Bl
{

        public interface IUser
        {
            public List<User> GetAll();
            public List<User> GetSelect(int num);
            public User GetById(int id);
            public bool Save(User user);
            public bool Delete(int id);
        }
    public class ClsUser : IUser
    {
        TaskContext context;

        public ClsUser(TaskContext ctx)
        {
            context = ctx;
        }

        public List<User> GetAll()
        {
            try
            {
                var users = context.TbUsers.ToList();
                return users;
            }
            catch {

                return new List<User>();
            }
        }
        public List<User> GetSelect(int num)
        {
            try
            {
                var users = context.TbUsers.OrderBy(x=> Guid.NewGuid())
                            .Take(num)                    
                            .ToList();
                return users;
            }
            catch
            {

                return new List<User>();
            }
        }
        public User GetById(int id)
        {
            try
            {
                var user = context.TbUsers.FirstOrDefault(a => a.Id == id);
                return user;
            }
            catch
            {   
                return new User();
                
            }

        }

        public bool Save(User users) {
            try
            {
                if (users.Id == 0)
                {
                    context.TbUsers.Add(users);
                }
                else
                {
                    context.Entry(users).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                context.SaveChanges();
                return true;
            }
            catch {
                return false; 
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var user = GetById(id);
                context.Remove(user);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }
    }
}
