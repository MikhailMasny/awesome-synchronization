using AutoMapper;

namespace Masny.Application.Interfaces
{
    /// <summary>
    /// Map.
    /// </summary>
    /// <typeparam name="T">Model.</typeparam>
    public interface IMapFrom<T>
    {
        /// <summary>
        /// Mapping.
        /// </summary>
        /// <param name="profile">Model profile.</param>
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
