# Shared Code Data Library for Entity Framework

A library of code shared for free use to help with common data access scenarios using Entity Framework.

To use these, search NuGet for SharedCode.Data.EntityFramework

This specifically targets Entity Framework Core. There were some legacy Entity Framework helpers in old versions of the shared libraries, which can be found in older NuGet packages and in the git history. If someone wants to step up to maintain those I would happily take pull requests for adding them to the new solution as separate project(s).

---
## Reference Notes
### Resilient database context setup example:

```cs
services.AddDbContext<CatalogContext>(
	options =>
	{
		options.UseSqlServer(Configuration["ConnectionString"],
		sqlServerOptionsAction: sqlOptions =>
		{
			sqlOptions.EnableRetryOnFailure(
				maxRetryCount: 10,
				maxRetryDelay: TimeSpan.FromSeconds(30),
				errorNumbersToAdd: null);
		});
    });
```
