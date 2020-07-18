using Masny.Application.Interfaces;
using Masny.Application.Models;
using Masny.Application.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masny.Infrastructure.Services
{
    /// <inheritdoc cref="IPersonSynchronizationService"/>
    public class PersonSynchronizationService : IPersonSynchronizationService
    {
        private readonly ILogger<PersonSynchronizationService> _logger;
        private readonly ICloudManager _cloudManager;
        private readonly IPersonManager _personManager;

        public PersonSynchronizationService(ILogger<PersonSynchronizationService> logger,
                                            ICloudManager cloudManager,
                                            IPersonManager personManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cloudManager = cloudManager ?? throw new ArgumentNullException(nameof(cloudManager));
            _personManager = personManager ?? throw new ArgumentNullException(nameof(personManager));
        }

        /// <inheritdoc/>
        public async Task AddAsync()
        {
            _logger.LogInformation(SyncMessage.PersonAddStart);

            var peopleCloud = await _cloudManager.GetUsers().ToListAsync();
            var peopleApp = (await _personManager.GetAllAsync()).ToList();

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

                    await _personManager.CreateAsync(personDto);
                }
            }

            _logger.LogInformation(SyncMessage.PersonAddEnd);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync()
        {
            _logger.LogInformation(SyncMessage.PersonDeleteEnd);

            var peopleCloud = await _cloudManager.GetUsers().ToListAsync();
            var peopleApp = (await _personManager.GetAllAsync()).ToList();

            var peopleCloudIds = peopleCloud.Select(p => p.Id);
            var peopleAppIds = peopleApp.Select(p => p.CloudId);

            var deleteIds = peopleAppIds.Except(peopleCloudIds);

            if (deleteIds.Any())
            {
                foreach (var id in deleteIds)
                {
                    await _personManager.DeleteByCloudIdAsync(id);
                }
            }

            _logger.LogInformation(SyncMessage.PersonDeleteStart);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync()
        {
            _logger.LogInformation(SyncMessage.PersonUpdateStart);

            var peopleCloud = await _cloudManager.GetUsers().ToListAsync();
            var peopleApp = (await _personManager.GetAllAsync()).ToList();

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
                    await _personManager.UpdateAsync(personApp);
                }
            }

            _logger.LogInformation(SyncMessage.PersonUpdateEnd);
        }
    }
}
