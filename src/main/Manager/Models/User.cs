﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using Thunder.ComponentModel.DataAnnotations;
using Thunder.Data;
using Thunder.Security;

namespace Manager.Models
{
    /// <summary>
    /// Usuário do sistema
    /// </summary>
    public class User : ActiveRecord<User, int>
    {
        private const string PasswordKey = "@9#7$5%W*&1WpC#@&2*%4$6#8@";

        /// <summary>
        /// Initialize new instance off <see cref="User"/>.
        /// </summary>
        public User()
        {
            State = State.Active;
        }

        /// <summary>
        /// Recupera ou define perfil
        /// </summary>
        [Display(Name = "Perfil")]
        public virtual UserProfile Profile { get; set; }

        /// <summary>
        /// Get or set name
        /// </summary>
        [Display(Name = "Nome"), Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Get or set e-mail
        /// </summary>
        [Email(ErrorMessage = "E-mail informado é inválido.")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Get or set login
        /// </summary>
        [Display(Name = "Login"), Required]
        public virtual string Login { get; set; }

        /// <summary>
        /// Get or set password
        /// </summary>
        [Display(Name = "Senha"), Required]
        public virtual string Password { get; set; }

        /// <summary>
        /// Get or set status
        /// </summary>
        public virtual State State { get; set; }

        #region Public Static Methods
        /// <summary>
        /// Encript password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncriptPassword(string password)
        {
            return password.Encrypt(PasswordKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static User Find(string login, string password)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var user = Session.GetNamedQuery("users-find-by-login-password")
                    .SetString("login", login.ToLower())
                    .SetString("password", EncriptPassword(password))
                    .UniqueResult<User>();

                transaction.Commit();

                return user;
            }
        }

        #endregion

        
    }
}