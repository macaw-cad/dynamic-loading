using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Macaw.DynamicLoading.Domain.Models;
using Macaw.DynamicLoading.Domain.Services;

namespace Macaw.DynamicLoading.List.Services
{
    public class PersonRepository : IPersonRepository
    {
        private readonly List<PersonModel> _persons;

        public PersonRepository()
        {
            _persons = new List<PersonModel>();
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
            _persons.Add(personModel);

            return personModel.Id;
        }

        public PersonModel ReadPerson(Guid id)
        {
            return _persons.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<PersonModel> ReadAllPersons()
        {
            return _persons;
        }

        public void UpdatePerson(UpdatePersonModel person)
        {
            ThrowExceptionIfGuidIsEmpty(person.Id);

            var personModel = _persons.SingleOrDefault(x => x.Id == person.Id);

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
        }

        public PersonModel DeletePerson(Guid id)
        {
            ThrowExceptionIfGuidIsEmpty(id);

            var deletedPerson = _persons.SingleOrDefault(x => x.Id.Equals(id));

            if (deletedPerson == null)
            {
                return null;
            }

            _persons.Remove(deletedPerson);
            return deletedPerson;
        }

        private static void ThrowExceptionIfGuidIsEmpty(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new InvalidOperationException("Provide a valid Id.");
            }
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