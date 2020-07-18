using AutoMapper;
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

        public PersonManager(IAppDbContext context,
                             IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateAsync(PersonDto personDto)
        {
            if (personDto == null)
            {
                throw new ArgumentNullException(nameof(personDto));
            }

            var person = _mapper.Map<PersonDto, Person>(personDto);
            await _context.Persons.AddAsync(person);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<PersonDto> GetAsync(int id)
        {
            var person = await _context.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<Person, PersonDto>(person);
        }

        /// <inheritdoc/>
        public async Task<PersonDto> GetByCloudIdAsync(int cloudId)
        {
            var person = await _context.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.CloudId == cloudId);
            return _mapper.Map<Person, PersonDto>(person);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PersonDto>> GetAllAsync()
        {
            var people = await _context.Persons.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<Person>, IEnumerable<PersonDto>>(people);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateAsync(PersonDto personDto)
        {
            if (personDto == null)
            {
                throw new ArgumentNullException(nameof(personDto));
            }

            var person = await _context.Persons.FindAsync(personDto.Id);
            if (person == null)
            {
                return 0;
            }

            // See comment at CommentManager file

            person.CloudId = personDto.CloudId;
            person.Email = personDto.Email;
            person.Name = personDto.Name;

            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteAsync(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return 0;
            }

            _context.Persons.Remove(person);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteByCloudIdAsync(int cloudId)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.CloudId == cloudId);
            if (person == null)
            {
                return 0;
            }

            _context.Persons.Remove(person);
            return await _context.SaveChangesAsync();
        }
    }
}
