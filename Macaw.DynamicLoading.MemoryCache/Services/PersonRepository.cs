using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Macaw.DynamicLoading.Domain.Models;
using Macaw.DynamicLoading.Domain.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Macaw.DynamicLoading.MemoryCache.Services
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IMemoryCache _memoryCache;
        private static readonly Guid IndexKey = Guid.NewGuid();

        public PersonRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            SeedRepository();
        }

        public string NameOfAssembly()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }

        public Guid CreatePerson(CreatePersonModel person)
        {
            var personModel = new PersonModel
            {
                Id = Guid.NewGuid(),
                Age = person.Age,
                Name = person.Name
            };

            PersistPerson(personModel);
            return personModel.Id;
        }

        private void PersistPerson(PersonModel personModel)
        {
            var personList = GetPersonList();
            personList.Add(personModel);

            StoreList(personList);
        }

        public PersonModel ReadPerson(Guid id)
        {
            var personList = GetPersonList();
            return personList.SingleOrDefault(x => x.Id.Equals(id));
        }

        public IEnumerable<PersonModel> ReadAllPersons()
        {
            return GetPersonList();
        }

        public void UpdatePerson(UpdatePersonModel person)
        {
            ThrowExceptionIfGuidIsEmpty(person.Id);
            var personList = GetPersonList();
            var personModel = personList.SingleOrDefault(x => x.Id == person.Id);

            if (personModel == null)
            {
                return;
            }

            if (person.Age.HasValue)
            {
                personModel.Age = person.Age.Value;
            }

            if (string.IsNullOrEmpty(person.Name))
            {
                personModel.Name = person.Name;
            }
            StoreList(personList);
        }

        public PersonModel DeletePerson(Guid id)
        {
            ThrowExceptionIfGuidIsEmpty(id);
            var personList = GetPersonList();
            var deletedPerson = personList.SingleOrDefault(x => x.Id.Equals(id));

            if (deletedPerson == null)
            {
                return null;
            }

            personList.Remove(deletedPerson);
            StoreList(personList);
            return deletedPerson;
        }

        private static void ThrowExceptionIfGuidIsEmpty(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new InvalidOperationException("Provide a valid Id.");
            }
        }

        private void StoreList(List<PersonModel> personList)
        {
            _memoryCache.Set(IndexKey, personList);
        }

        private List<PersonModel> GetPersonList()
        {
            return _memoryCache.GetOrCreate(IndexKey, entry => new List<PersonModel>());
        }

        private void SeedRepository()
        {
            var random = new Random(DateTime.UtcNow.Second);
            CreatePerson(new CreatePersonModel
            {
                Name = "Ronald",
                Age = random.Next(20, 50)
            });

            CreatePerson(new CreatePersonModel
            {
                Name = "Serge",
                Age = random.Next(20, 50)
            });

            CreatePerson(new CreatePersonModel
            {
                Name = "Marc",
                Age = random.Next(20, 50)
            });
        }
    }
}