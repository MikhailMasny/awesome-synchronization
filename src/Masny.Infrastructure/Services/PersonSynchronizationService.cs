using Masny.Application.Interfaces;
using Masny.Application.Models;
using Masny.Domain.Models.App;
using Masny.Domain.Models.Cloud;
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
        public async Task AddNewPeople()
        {
            var peopleCloud = await _cloudManager.GetUsers().ToListAsync();
            var peopleApp = (await _personManager.GetPeople()).ToList();

            var peopleCloudIds = peopleCloud.Select(p => p.Id);
            var peopleAppIds = peopleApp.Select(p => p.Id);

            var newIds = peopleCloudIds.Except(peopleAppIds);

            //var people1 = new List<User>();
            //foreach (var id in newIds)
            //{
            //    people1.Add(peopleCloud.FirstOrDefault(p => p.Id == id));
            //}

            var people = peopleCloud.Join(
                newIds,
                personCloud => personCloud.Id,
                id => id,
                (personCloud, id) => personCloud);

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
}
