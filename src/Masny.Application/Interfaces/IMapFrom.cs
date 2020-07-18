using AutoMapper;
using System;

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
        void Mapping(Profile profile)
        {
            profile = profile ?? throw new ArgumentNullException(nameof(profile));

            profile.CreateMap(typeof(T), GetType()).ReverseMap();
        }
    }
}
