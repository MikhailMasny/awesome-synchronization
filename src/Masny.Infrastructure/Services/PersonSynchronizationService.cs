using Masny.Application.Interfaces;
using Masny.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masny.Infrastructure.Services
{
    /// <inheritdoc cref="IPersonSynchronizationService"/>
    public class PersonSynchronizationService : IPersonSynchronizationService
    {
        private readonly ICloudManager _cloudManager;
        private readonly IPersonManager _personManager;

        public PersonSynchronizationService(ICloudManager cloudManager, IPersonManager personManager)
        {
            _cloudManager = cloudManager ?? throw new ArgumentNullException(nameof(cloudManager));
            _personManager = personManager ?? throw new ArgumentNullException(nameof(personManager));
        }

        /// <inheritdoc/>
        public async Task AddPeople()
        {
            var peopleCloud = await _cloudManager.GetUsers().ToListAsync();
            var peopleApp = (await _personManager.GetPeopleWithoutTracking()).ToList();

            var peopleCloudIds = peopleCloud.Select(p => p.Id);
            var peopleAppIds = peopleApp.Select(p => p.CloudId);

            var newIds = peopleCloudIds.Except(peopleAppIds);

            var people = peopleCloud.Join(
                newIds,
                personCloud => personCloud.Id,
                id => id,
                (personCloud, id) => personCloud);

            if (people.Any())
            {
                foreach (var user in people)
                {
                    var personDto = new PersonDto
                    {
                        CloudId = user.Id,
                        Name = user.Name,
                        Email = user.Email
                    };

                    await _personManager.CreatePerson(personDto);
                }
            }
        }

        /// <inheritdoc/>
        public async Task DeletePeople()
        {
            var peopleCloud = await _cloudManager.GetUsers().ToListAsync();
            var peopleApp = (await _personManager.GetPeopleWithoutTracking()).ToList();

            var peopleCloudIds = peopleCloud.Select(p => p.Id);
            var peopleAppIds = peopleApp.Select(p => p.CloudId);

            var deleteIds = peopleAppIds.Except(peopleCloudIds);

            if (deleteIds.Any())
            {
                foreach (var id in deleteIds)
                {
                    await _personManager.DeletePersonByCloudId(id);
                }
            }
        }

        /// <inheritdoc/>
        public async Task UpdatePeople()
        {
            var peopleCloud = await _cloudManager.GetUsers().ToListAsync();
            var peopleApp = (await _personManager.GetPeopleWithoutTracking()).ToList();

            foreach (var personApp in peopleApp)
            {
                var personCloud = peopleCloud.FirstOrDefault(p => p.Id == personApp.CloudId);
                var isUpdated = false;

                if (personApp.Email != personCloud.Email)
                {
                    personApp.Email = personCloud.Email;
                    isUpdated = true;
                }

                if (personApp.Name != personCloud.Name)
                {
                    personApp.Name = personCloud.Name;
                    isUpdated = true;
                }

                if (isUpdated)
                {
                    await _personManager.UpdatePerson(personApp);
                }
            }
        }
    }
}
