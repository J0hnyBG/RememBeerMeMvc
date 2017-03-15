﻿using System.Collections.Generic;
using System.Linq;

using Bytes2you.Validation;

using RememBeer.Data.Repositories.Base;
using RememBeer.Models;
using RememBeer.Models.Contracts;

namespace RememBeer.Services.Contracts
{
    public class BeerTypesService : IBeerTypesService
    {
        private readonly IRepository<BeerType> typesRepository;

        public BeerTypesService(IRepository<BeerType> typesRepository)
        {
            Guard.WhenArgument(typesRepository, nameof(typesRepository)).IsNull().Throw();

            this.typesRepository = typesRepository;
        }

        public IEnumerable<IBeerType> Search(string name)
        {
            return this.typesRepository.All.Where(t => t.Type.Contains(name)).ToList();
        }
    }
}
