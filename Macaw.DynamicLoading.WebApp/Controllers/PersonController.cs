using System;
using Macaw.DynamicLoading.Domain.Models;
using Macaw.DynamicLoading.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Macaw.DynamicLoading.WebAppDynamic.Controllers
{
    public class PersonController : Controller
    {
        public const string AssemblyIndexName = "AssemblyName";
        private readonly IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public IActionResult Index()
        {
            var persons = _personRepository.ReadAllPersons();
            ViewData[AssemblyIndexName] = _personRepository.NameOfAssembly();

            return View(persons);
        }

        public IActionResult View(Guid id)
        {
            var person = _personRepository.ReadPerson(id);

            return View(person);
        }

        public IActionResult Delete(Guid id)
        {
            var person = _personRepository.ReadPerson(id);
            return View(person);
        }

        public IActionResult DeleteDeleted(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return RedirectToAction("Index");
            }

            var deletedPerson = _personRepository.DeletePerson(id);
            return View(deletedPerson);
        }

        public IActionResult Create()
        {
            return View(new CreatePersonModel());
        }

        [HttpPost]
        public IActionResult Create([FromForm] CreatePersonModel person)
        {
            var personId = _personRepository.CreatePerson(person);

            return RedirectToAction("View", new {Id = personId});
        }
    }
}