using System;

namespace Macaw.DynamicLoading.Domain.Models
{
    public class UpdatePersonModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
    }
}