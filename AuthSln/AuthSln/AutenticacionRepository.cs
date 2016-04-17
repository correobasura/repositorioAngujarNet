using AuthSln.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSln.API
{
    /// <summary>
    /// Clase que gestiona las acciones que se realizan sobre los clientes y los tokens
    /// </summary>
    public class AutenticacionRepository : IDisposable
    {
        /// <summary>
        /// Objeto que relaciona al contesto de BD
        /// </summary>
        private AutenticacionContext _contexto;

        /// <summary>
        /// Objeto que gestiona las consultas de usuarios
        /// </summary>
        private UserManager<IdentityUser> _usuarioManager;

        public AutenticacionRepository()
        {
            _contexto = new AutenticacionContext();
            _usuarioManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_contexto));
        }

        /// <summary>
        /// Método que realiza las tareas de creación de usuarios
        /// </summary>
        /// <param name="usuario">Objeto con información de usuario</param>
        /// <returns>objeto usuario creado</returns>
        public async Task<IdentityResult> RegistrarUsuario(UsuarioModel usuario)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = usuario.Usuario
            };
            var result = await _usuarioManager.CreateAsync(user, usuario.Password);
            return result;
        }

        /// <summary>
        /// Método que realiza la búsqueda de usuarios
        /// </summary>
        /// <param name="usuario">usuario a ser buscado</param>
        /// <param name="contrasenia">contraseña del usuario a buscar</param>
        /// <returns>retornar el objeto usuario encontrado</returns>
        public async Task<IdentityUser> BuscarUsuario(string usuario, string contrasenia)
        {
            IdentityUser user = await _usuarioManager.FindAsync(usuario, contrasenia);
            return user;
        }

        /// <summary>
        /// Método que realiza las operaciones de cerrado de la base de datos
        /// </summary>
        public void Dispose()
        {
            //Se cierra el contexto
            _contexto.Dispose();
            //Se cierra el objeto usado para la consulta de usuarios
            _usuarioManager.Dispose();
        }

        /// <summary>
        /// Método que retorna un User de acuerdo a el clientId
        /// </summary>
        /// <param name="clientId">identificador del User</param>
        /// <returns></returns>
        public User BuscarCliente(string clientId)
        {
            var user = _contexto.Usuarios.Find(clientId);
            return user;
        }

        /// <summary>
        /// Método que agrega un RefreshToken
        /// </summary>
        /// <param name="token">Objeto con información del token</param>
        /// <returns>Bandera que indica si el refreshToken ha sido guardado</returns>
        public async Task<bool> AgregarRefreshToken(RefreshToken token)
        {
            //Primero se realiza la consulta de existencia del token
            var existingToken = _contexto.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();
            //Si el token existe se elimina para adicionar uno con la información actualizada
            if (existingToken != null)
            {
                var result = await RemoverRefreshToken(existingToken);
            }
            _contexto.RefreshTokens.Add(token);
            return await _contexto.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Método que remueve un token que será buscado de acuerdo a su id
        /// </summary>
        /// <param name="refreshTokenId">Identificador del token</param>
        /// <returns>Bandera que indica si el token ha sido borrado</returns>
        public async Task<bool> RemoverRefreshToken(string refreshTokenId)
        {
            //Se realiza la búsqueda del token
            var refreshToken = await _contexto.RefreshTokens.FindAsync(refreshTokenId);
            if (refreshToken != null)
            {
                _contexto.RefreshTokens.Remove(refreshToken);
                return await _contexto.SaveChangesAsync() > 0;
            }
            //Si no se encontró ningún token con el identificador dado, se retorna un false
            return false;
        }

        /// <summary>
        /// Método que remueve un token de acuerdo al RefreshToken
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<bool> RemoverRefreshToken(RefreshToken refreshToken)
        {
            _contexto.RefreshTokens.Remove(refreshToken);
            return await _contexto.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Método que realiza la búsqueda de un RefreshToken de acuerdo a  su id
        /// </summary>
        /// <param name="refreshTokenId">Identificador del RefreshToken</param>
        /// <returns>Objeto RefreshToken</returns>
        public async Task<RefreshToken> BuscarRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _contexto.RefreshTokens.FindAsync(refreshTokenId);
            return refreshToken;
        }

        /// <summary>
        /// Método que retorna todos los tokens existentes
        /// </summary>
        /// <returns></returns>
        public List<RefreshToken> ObtenerTodosRefreshTokens()
        {
            return _contexto.RefreshTokens.ToList();
        }
    }
}