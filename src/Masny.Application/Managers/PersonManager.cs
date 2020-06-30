﻿using AutoMapper;
using Masny.Application.Interfaces;
using Masny.Application.Models;
using Masny.Domain.Models.App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masny.Application.Managers
{
    /// <inheritdoc cref="IPersonManager"/>
    public class PersonManager : IPersonManager
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public PersonManager(IAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreatePerson(PersonDto personDto)
        {
            var person = _mapper.Map<PersonDto, Person>(personDto);
            await _context.Persons.AddAsync(person);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<PersonDto> GetPerson(int id)
        {
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<Person, PersonDto>(person);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PersonDto>> GetPeople()
        {
            var people = await _context.Persons.ToListAsync();
            return _mapper.Map<IEnumerable<Person>, IEnumerable<PersonDto>>(people);
        }

        /// <inheritdoc/>
        public async Task<int> UpdatePerson(PersonDto personDto)
        {
            var person = _mapper.Map<PersonDto, Person>(personDto);
            _context.Persons.Update(person);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeletePerson(int id)
        {
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.Id == id);
            _context.Persons.Remove(person);
            return await _context.SaveChangesAsync();
        }
    }
}
