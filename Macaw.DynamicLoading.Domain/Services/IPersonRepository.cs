using System;
using System.Collections.Generic;
using Macaw.DynamicLoading.Domain.Models;

namespace Macaw.DynamicLoading.Domain.Services
{
    public interface IPersonRepository
    {
        string NameOfAssembly();

        Guid CreatePerson(CreatePersonModel person);
        PersonModel ReadPerson(Guid id);
        IEnumerable<PersonModel> ReadAllPersons();
        void UpdatePerson(UpdatePersonModel person);
        PersonModel DeletePerson(Guid id);
    }
}