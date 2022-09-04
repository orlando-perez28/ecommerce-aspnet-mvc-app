﻿using eTickets.Data.Base;
using eTickets.Models;

namespace eTickets.Data.Services;

public class ProducersService : EntityBaseRepository<Producer>, IProducersService
{
	public ProducersService(AppDbContext appDbContext) : base (appDbContext)
	{
	}
}
